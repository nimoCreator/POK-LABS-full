from __future__ import annotations

from dataclasses import dataclass
from pathlib import Path


def require_module(module_name: str, install_hint: str):
    try:
        return __import__(module_name)
    except ImportError as exc:
        raise SystemExit(
            f"Brak modulu '{module_name}'. Zainstaluj go poleceniem: {install_hint}"
        ) from exc


np = require_module("numpy", "python -m pip install numpy")
sp = require_module("sympy", "python -m pip install sympy")
torch = require_module("torch", "python -m pip install torch")
require_module("matplotlib", "python -m pip install matplotlib")

from matplotlib import pyplot as plt
from matplotlib.patches import Rectangle

nn = torch.nn
TensorDataset = torch.utils.data.TensorDataset
DataLoader = torch.utils.data.DataLoader


@dataclass(frozen=True)
class Config:
    coeff_min: float = -1.0
    coeff_max: float = 1.0
    train_size: int = 24000
    valid_size: int = 6000
    batch_size: int = 512
    epochs: int = 250
    learning_rate: float = 1e-3
    seed: int = 42
    example_count: int = 10
    heatmap_min: float = -10.0
    heatmap_max: float = 10.0
    heatmap_resolution: int = 201


def set_seed(seed: int) -> None:
    np.random.seed(seed)
    torch.manual_seed(seed)
    if torch.cuda.is_available():
        torch.cuda.manual_seed_all(seed)


def algebraic_roots_real_parts(b_values, c_values):
    discriminant = b_values**2 - 4.0 * c_values
    sqrt_term = np.sqrt(np.maximum(discriminant, 0.0))
    root_1 = (-b_values - sqrt_term) / 2.0
    root_2 = (-b_values + sqrt_term) / 2.0
    return np.stack([root_1, root_2], axis=1).astype(np.float32)


def generate_dataset(sample_count: int, seed: int, coeff_min: float, coeff_max: float):
    rng = np.random.default_rng(seed)
    coeffs = rng.uniform(coeff_min, coeff_max, size=(sample_count, 2)).astype(np.float32)
    b_values = coeffs[:, 0]
    c_values = coeffs[:, 1]
    targets = algebraic_roots_real_parts(b_values, c_values)
    x_tensor = torch.tensor(coeffs, dtype=torch.float32)
    y_tensor = torch.tensor(targets, dtype=torch.float32)
    return x_tensor, y_tensor


def build_model():
    return nn.Sequential(
        nn.Linear(2, 64),
        nn.Tanh(),
        nn.Linear(64, 64),
        nn.Tanh(),
        nn.Linear(64, 32),
        nn.ReLU(),
        nn.Linear(32, 2),
    )


def train_model(model, train_loader, valid_loader, device, config: Config):
    criterion = nn.MSELoss()
    optimizer = torch.optim.Adam(model.parameters(), lr=config.learning_rate)
    history = []

    for epoch in range(1, config.epochs + 1):
        model.train()
        train_loss_total = 0.0
        train_items = 0

        for batch_x, batch_y in train_loader:
            batch_x = batch_x.to(device)
            batch_y = batch_y.to(device)

            optimizer.zero_grad()
            prediction = model(batch_x)
            loss = criterion(prediction, batch_y)
            loss.backward()
            optimizer.step()

            train_loss_total += loss.item() * batch_x.size(0)
            train_items += batch_x.size(0)

        model.eval()
        valid_loss_total = 0.0
        valid_items = 0

        with torch.no_grad():
            for batch_x, batch_y in valid_loader:
                batch_x = batch_x.to(device)
                batch_y = batch_y.to(device)
                prediction = model(batch_x)
                loss = criterion(prediction, batch_y)
                valid_loss_total += loss.item() * batch_x.size(0)
                valid_items += batch_x.size(0)

        train_loss = train_loss_total / max(train_items, 1)
        valid_loss = valid_loss_total / max(valid_items, 1)
        history.append((epoch, train_loss, valid_loss))

        if epoch == 1 or epoch % 25 == 0 or epoch == config.epochs:
            print(
                f"Epoka {epoch:>3}/{config.epochs}: "
                f"train_loss = {train_loss:.8f}, valid_loss = {valid_loss:.8f}"
            )

    return history


def numpy_roots_real_parts(b_value: float, c_value: float):
    roots = np.roots([1.0, b_value, c_value])
    return np.sort(np.real(roots)).astype(np.float64)


def sympy_roots_real_parts(b_value: float, c_value: float):
    x = sp.symbols("x")
    b_sym = sp.Rational(str(b_value))
    c_sym = sp.Rational(str(c_value))
    equation = x**2 + b_sym * x + c_sym
    roots_map = sp.roots(equation, x)

    real_parts = []
    for root, multiplicity in roots_map.items():
        real_value = float(sp.N(sp.re(root), 30))
        real_parts.extend([real_value] * multiplicity)

    real_parts.sort()
    return np.array(real_parts, dtype=np.float64)


def neural_roots_real_parts(model, b_value: float, c_value: float, device):
    model.eval()
    with torch.no_grad():
        coeffs = torch.tensor([[b_value, c_value]], dtype=torch.float32, device=device)
        prediction = model(coeffs).cpu().numpy()[0]
    return np.sort(prediction.astype(np.float64))


def neural_roots_real_parts_batch(model, coeffs, device):
    model.eval()
    with torch.no_grad():
        coeffs_tensor = torch.tensor(coeffs, dtype=torch.float32, device=device)
        predictions = model(coeffs_tensor).cpu().numpy()
    return np.sort(predictions.astype(np.float64), axis=1)


def relative_error(exact_values, approx_values):
    exact_values = np.asarray(exact_values, dtype=np.float64)
    approx_values = np.asarray(approx_values, dtype=np.float64)
    denominator = max(np.linalg.norm(exact_values), 1e-12)
    return np.linalg.norm(approx_values - exact_values) / denominator


def sample_random_examples(model, device, sample_count: int, seed: int):
    rng = np.random.default_rng(seed)
    coeffs = rng.uniform(-1.0, 1.0, size=(sample_count, 2))
    rows = []

    for index, (b_value, c_value) in enumerate(coeffs, start=1):
        b_value = float(b_value)
        c_value = float(c_value)
        exact_symbolic = sympy_roots_real_parts(b_value, c_value)
        approx_numpy = numpy_roots_real_parts(b_value, c_value)
        approx_neural = neural_roots_real_parts(model, b_value, c_value, device)

        numpy_error = relative_error(exact_symbolic, approx_numpy)
        neural_error = relative_error(exact_symbolic, approx_neural)

        rows.append(
            {
                "nr": str(index),
                "b": f"{b_value:.6f}",
                "c": f"{c_value:.6f}",
                "sympy": format_root_pair(exact_symbolic),
                "numpy": format_root_pair(approx_numpy),
                "mlp": format_root_pair(approx_neural),
                "err_numpy": f"{numpy_error:.6e}",
                "err_mlp": f"{neural_error:.6e}",
            }
        )

    return rows


def format_root_pair(values) -> str:
    values = np.asarray(values, dtype=np.float64)
    return f"[{values[0]: .6f}, {values[1]: .6f}]"


def build_text_table(rows) -> str:
    headers = {
        "nr": "nr",
        "b": "b",
        "c": "c",
        "sympy": "SymPy",
        "numpy": "NumPy",
        "mlp": "MLP",
        "err_numpy": "blad NumPy",
        "err_mlp": "blad MLP",
    }
    keys = list(headers.keys())
    widths = {
        key: max(len(headers[key]), *(len(row[key]) for row in rows))
        for key in keys
    }

    def make_line(values):
        return " | ".join(values[key].ljust(widths[key]) for key in keys)

    separator = "-+-".join("-" * widths[key] for key in keys)
    table_lines = [make_line(headers), separator]
    table_lines.extend(make_line(row) for row in rows)
    return "\n".join(table_lines)


def evaluate_examples(model, device, config: Config):
    rows = sample_random_examples(
        model=model,
        device=device,
        sample_count=config.example_count,
        seed=config.seed + 100,
    )
    table = build_text_table(rows)
    print("\nLosowe przyklady dla rownan x^2 + b*x + c = 0:\n")
    print(table)
    print()
    return rows, table


def create_error_heatmap(model, device, config: Config, output_path: Path):
    b_axis = np.linspace(config.heatmap_min, config.heatmap_max, config.heatmap_resolution)
    c_axis = np.linspace(config.heatmap_min, config.heatmap_max, config.heatmap_resolution)
    grid_b, grid_c = np.meshgrid(b_axis, c_axis, indexing="xy")
    coeffs = np.column_stack([grid_b.ravel(), grid_c.ravel()]).astype(np.float32)

    exact_roots = algebraic_roots_real_parts(coeffs[:, 0], coeffs[:, 1]).astype(np.float64)
    exact_roots = np.sort(exact_roots, axis=1)
    predicted_roots = neural_roots_real_parts_batch(model, coeffs, device)

    denominator = np.maximum(np.linalg.norm(exact_roots, axis=1), 1e-12)
    errors = np.linalg.norm(predicted_roots - exact_roots, axis=1) / denominator
    error_grid = errors.reshape(config.heatmap_resolution, config.heatmap_resolution)

    fig, ax = plt.subplots(figsize=(8, 7))
    image = ax.imshow(
        error_grid,
        extent=[
            config.heatmap_min,
            config.heatmap_max,
            config.heatmap_min,
            config.heatmap_max,
        ],
        origin="lower",
        interpolation="nearest",
        aspect="equal",
        cmap="viridis",
    )
    colorbar = fig.colorbar(image, ax=ax)
    colorbar.set_label("blad wzgledny MLP")
    ax.set_xlabel("b")
    ax.set_ylabel("c")
    ax.add_patch(
        Rectangle(
            (config.coeff_min, config.coeff_min),
            config.coeff_max - config.coeff_min,
            config.coeff_max - config.coeff_min,
            fill=False,
            edgecolor="red",
            linewidth=1.5,
            linestyle="--",
        )
    )
    ax.set_title(
        "Mapa bledu wzglednego MLP dla rownania x^2 + b*x + c = 0\n"
        "czerwony kwadrat = zakres uczenia [-1, 1]"
    )
    plt.tight_layout()
    fig.savefig(output_path, dpi=180)
    plt.show()
    plt.close(fig)

    return {
        "min_error": float(np.min(errors)),
        "mean_error": float(np.mean(errors)),
        "max_error": float(np.max(errors)),
        "resolution": config.heatmap_resolution,
        "heatmap_min": config.heatmap_min,
        "heatmap_max": config.heatmap_max,
        "output_path": str(output_path),
    }


def write_results_report(
    output_path: Path,
    device,
    config: Config,
    history,
    table: str,
    heatmap_stats,
) -> None:
    last_epoch, train_loss, valid_loss = history[-1]
    lines = [
        "pnmzad05 - raport wynikow",
        "",
        f"urzadzenie: {device}",
        f"epoki: {last_epoch}",
        f"train_loss_koncowy: {train_loss:.8f}",
        f"valid_loss_koncowy: {valid_loss:.8f}",
        f"liczba_losowych_przykladow: {config.example_count}",
        f"rozdzielczosc_heatmapy: {heatmap_stats['resolution']} x {heatmap_stats['resolution']}",
        f"zakres_uczenia_b_c: [{config.coeff_min}, {config.coeff_max}]",
        f"zakres_heatmapy_b_c: [{heatmap_stats['heatmap_min']}, {heatmap_stats['heatmap_max']}]",
        f"minimalny_blad_heatmapy: {heatmap_stats['min_error']:.6e}",
        f"sredni_blad_heatmapy: {heatmap_stats['mean_error']:.6e}",
        f"maksymalny_blad_heatmapy: {heatmap_stats['max_error']:.6e}",
        f"plik_heatmapy: {heatmap_stats['output_path']}",
        "",
        "Tabela losowych przykladow:",
        table,
        "",
    ]
    output_path.write_text("\n".join(lines), encoding="utf-8")


def main() -> int:
    config = Config()
    set_seed(config.seed)
    output_dir = Path(__file__).resolve().parent
    report_path = output_dir / "pnmzad05.txt"
    heatmap_path = output_dir / "pnmzad05_heatmap.png"

    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
    print(f"Uzywane urzadzenie: {device}")

    train_x, train_y = generate_dataset(
        config.train_size,
        config.seed,
        config.coeff_min,
        config.coeff_max,
    )
    valid_x, valid_y = generate_dataset(
        config.valid_size,
        config.seed + 1,
        config.coeff_min,
        config.coeff_max,
    )

    train_dataset = TensorDataset(train_x, train_y)
    valid_dataset = TensorDataset(valid_x, valid_y)

    train_loader = DataLoader(
        train_dataset,
        batch_size=config.batch_size,
        shuffle=True,
    )
    valid_loader = DataLoader(
        valid_dataset,
        batch_size=config.batch_size,
        shuffle=False,
    )

    model = build_model().to(device)
    history = train_model(model, train_loader, valid_loader, device, config)
    rows, table = evaluate_examples(model, device, config)
    heatmap_stats = create_error_heatmap(
        model=model,
        device=device,
        config=config,
        output_path=heatmap_path,
    )
    write_results_report(
        output_path=report_path,
        device=device,
        config=config,
        history=history,
        table=table,
        heatmap_stats=heatmap_stats,
    )

    print(f"Zapisano raport tekstowy: {report_path}")
    print(f"Zapisano heatmape bledu: {heatmap_path}")

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
