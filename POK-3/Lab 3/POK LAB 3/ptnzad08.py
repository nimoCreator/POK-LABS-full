from math import cos, exp, log, sin, tan
from assets import DoubleWriter, inputWithDefault


def newton_steps(x0, e, f, Df, max_iter=100):
    x = x0
    rows = []
    for step in range(1, max_iter + 1):
        fx = f(x)
        dfx = Df(x)
        x_next = x - fx / dfx
        difference = abs(x_next - x)
        rows.append((step, x, fx, dfx, x_next, difference))
        if difference < e:
            break
        x = x_next
    return rows


def SolveNonLinEq(x0, e, f, Df, max_iter=100):
    rows = newton_steps(x0, e, f, Df, max_iter)
    return rows[-1][4], rows[-1][0]


def print_steps_table(rows, doubleWriter):
    doubleWriter.print("| {0:>3} | {1:>16} | {2:>13} | {3:>13} | {4:>16} | {5:>13} |".format("i", "x_i", "f(x_i)", "Df(x_i)", "x_i+1", "roznica"))
    doubleWriter.print("+-{0:>3}-+-{1:>16}-+-{2:>13}-+-{3:>13}-+-{4:>16}-+-{5:>13}-+".format("---", "----------------", "-------------", "-------------", "----------------", "-------------"))

    for step, x, fx, dfx, x_next, difference in rows:
        doubleWriter.print("| {0:3d} | {1:16.10f} | {2:13.6e} | {3:13.6e} | {4:16.10f} | {5:13.6e} |".format(step, x, fx, dfx, x_next, difference))


def main():
    doubleWriter = DoubleWriter("ptnzad08")

    log2 = log(2.0)
    tests = [
        (
            "f(x) = 2*sin(x) - x",
            1.57,
            lambda x: 2.0 * sin(x) - x,
            lambda x: 2.0 * cos(x) - 1.0,
        ),
        (
            "f(x) = tan^3(x) - 4*tan(x)",
            1.1,
            lambda x: tan(x) ** 3 - 4.0 * tan(x),
            lambda x: (1.0 / cos(x) ** 2) * (3.0 * tan(x) ** 2 - 4.0),
        ),
        (
            "f(x) = exp(x - 1) + x^2 - 2",
            0.5,
            lambda x: exp(x - 1.0) + x * x - 2.0,
            lambda x: exp(x - 1.0) + 2.0 * x,
        ),
        (
            "f(x) = 2^(x^2 - 5*x + 10)*log(2)*(2*x - 5)",
            3.0,
            lambda x: 2.0 ** (x * x - 5.0 * x + 10.0) * log2 * (2.0 * x - 5.0),
            lambda x: 2.0 ** (x * x - 5.0 * x + 10.0) * log2 * (2.0 + log2 * (2.0 * x - 5.0) ** 2),
        ),
    ]

    eps = inputWithDefault("Podaj dokladnosc", 1.0e-10, float)
    doubleWriter.print("eps = {0:.1e}".format(eps))
    doubleWriter.print()

    for name, x0, f, Df in tests:
        rows = newton_steps(x0, eps, f, Df)
        x = rows[-1][4]

        doubleWriter.print(name)
        doubleWriter.print("x0 = {0:.10f}".format(x0))
        print_steps_table(rows, doubleWriter)
        doubleWriter.print("x = {0:.15f}".format(x))
        doubleWriter.print("liczba krokow =", rows[-1][0])
        doubleWriter.print("f(x) = {0:.6e}".format(f(x)))
        doubleWriter.print()


if __name__ == "__main__":
    main()
