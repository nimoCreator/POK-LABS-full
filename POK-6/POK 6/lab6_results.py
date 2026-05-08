from __future__ import annotations

import csv
import math
from dataclasses import dataclass
from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
OUTPUT_DIR = ROOT / "POK_results"
OUTPUT_DIR.mkdir(parents=True, exist_ok=True)


def rc_analytical(t: float) -> float:
    return math.exp(-t)


def rc_euler_step(q: float, dt: float) -> float:
    return q * (1.0 - dt)


def rc_midpoint_step(q: float, dt: float) -> float:
    k1 = -dt * q
    k2 = -dt * (q + 0.5 * k1)
    return q + k2


def rc_runge_kutta_step(q: float, dt: float) -> float:
    k1 = -dt * q
    k2 = -dt * (q + 0.5 * k1)
    k3 = -dt * (q + 0.5 * k2)
    k4 = -dt * (q + k3)
    return q + (k1 + 2.0 * k2 + 2.0 * k3 + k4) / 6.0


def generate_pmkzad01(dt: float = 0.1, tk: float = 4.0) -> dict[str, float]:
    path = OUTPUT_DIR / "pmkzad01.csv"
    q_euler = 1.0
    q_mid = 1.0
    q_rk = 1.0
    max_errors = {"euler": 0.0, "midpoint": 0.0, "rungekutta": 0.0}

    with path.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.writer(handle, delimiter=";")
        writer.writerow(["czas", "analit", "euler", "midpoint", "rungekutta"])

        step_count = int(round(tk / dt))
        for i in range(step_count + 1):
            t = i * dt
            analytical = rc_analytical(t)
            writer.writerow([f"{t:.6f}", f"{analytical:.10f}", f"{q_euler:.10f}", f"{q_mid:.10f}", f"{q_rk:.10f}"])

            max_errors["euler"] = max(max_errors["euler"], abs(analytical - q_euler))
            max_errors["midpoint"] = max(max_errors["midpoint"], abs(analytical - q_mid))
            max_errors["rungekutta"] = max(max_errors["rungekutta"], abs(analytical - q_rk))

            q_euler = rc_euler_step(q_euler, dt)
            q_mid = rc_midpoint_step(q_mid, dt)
            q_rk = rc_runge_kutta_step(q_rk, dt)

    max_errors["czas_koncowy"] = tk
    max_errors["analityczne_koniec"] = rc_analytical(tk)
    max_errors["euler_koniec"] = q_euler
    max_errors["midpoint_koniec"] = q_mid
    max_errors["rungekutta_koniec"] = q_rk
    return max_errors


@dataclass
class PendulumState:
    fi: float
    om: float


def pendulum_derivatives(state: PendulumState, g_div_l: float) -> PendulumState:
    return PendulumState(state.om, -g_div_l * math.sin(state.fi))


def pendulum_euler_step(state: PendulumState, dt: float, g_div_l: float) -> PendulumState:
    derivative = pendulum_derivatives(state, g_div_l)
    return PendulumState(state.fi + dt * derivative.fi, state.om + dt * derivative.om)


def pendulum_midpoint_step(state: PendulumState, dt: float, g_div_l: float) -> PendulumState:
    k1 = pendulum_derivatives(state, g_div_l)
    middle = PendulumState(state.fi + 0.5 * dt * k1.fi, state.om + 0.5 * dt * k1.om)
    k2 = pendulum_derivatives(middle, g_div_l)
    return PendulumState(state.fi + dt * k2.fi, state.om + dt * k2.om)


def pendulum_runge_kutta_step(state: PendulumState, dt: float, g_div_l: float) -> PendulumState:
    k1 = pendulum_derivatives(state, g_div_l)
    s2 = PendulumState(state.fi + 0.5 * dt * k1.fi, state.om + 0.5 * dt * k1.om)
    k2 = pendulum_derivatives(s2, g_div_l)
    s3 = PendulumState(state.fi + 0.5 * dt * k2.fi, state.om + 0.5 * dt * k2.om)
    k3 = pendulum_derivatives(s3, g_div_l)
    s4 = PendulumState(state.fi + dt * k3.fi, state.om + dt * k3.om)
    k4 = pendulum_derivatives(s4, g_div_l)
    return PendulumState(
        state.fi + dt * (k1.fi + 2.0 * k2.fi + 2.0 * k3.fi + k4.fi) / 6.0,
        state.om + dt * (k1.om + 2.0 * k2.om + 2.0 * k3.om + k4.om) / 6.0,
    )


def pendulum_analytical(t: float, fi0: float, sqrt_g_div_l: float) -> float:
    return fi0 * math.cos(t * sqrt_g_div_l)


def generate_pmkzad02(length: float = 1.2, fi0_deg: float = 5.0, dt_periods: float = 0.02, tk_periods: float = 3.0) -> dict[str, float]:
    path = OUTPUT_DIR / "pmkzad02.csv"

    g = 9.81
    g_div_l = g / length
    sqrt_g_div_l = math.sqrt(g_div_l)
    period = 2.0 * math.pi / sqrt_g_div_l
    dt = dt_periods * period
    tk = tk_periods * period
    fi0 = math.radians(fi0_deg)

    euler = PendulumState(fi0, 0.0)
    midpoint = PendulumState(fi0, 0.0)
    rungekutta = PendulumState(fi0, 0.0)

    max_errors = {"euler": 0.0, "midpoint": 0.0, "rungekutta": 0.0}

    with path.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.writer(handle, delimiter=";")
        writer.writerow(["czas", "analit_deg", "euler_deg", "midpoint_deg", "rungekutta_deg"])

        step_count = int(round(tk / dt))
        for i in range(step_count + 1):
            t = i * dt
            analytical = pendulum_analytical(t, fi0, sqrt_g_div_l)
            analytical_deg = math.degrees(analytical)
            euler_deg = math.degrees(euler.fi)
            midpoint_deg = math.degrees(midpoint.fi)
            rungekutta_deg = math.degrees(rungekutta.fi)

            writer.writerow([f"{t:.6f}", f"{analytical_deg:.10f}", f"{euler_deg:.10f}", f"{midpoint_deg:.10f}", f"{rungekutta_deg:.10f}"])

            max_errors["euler"] = max(max_errors["euler"], abs(analytical_deg - euler_deg))
            max_errors["midpoint"] = max(max_errors["midpoint"], abs(analytical_deg - midpoint_deg))
            max_errors["rungekutta"] = max(max_errors["rungekutta"], abs(analytical_deg - rungekutta_deg))

            euler = pendulum_euler_step(euler, dt, g_div_l)
            midpoint = pendulum_midpoint_step(midpoint, dt, g_div_l)
            rungekutta = pendulum_runge_kutta_step(rungekutta, dt, g_div_l)

    max_errors["okres"] = period
    max_errors["czas_koncowy"] = tk
    max_errors["euler_koniec"] = math.degrees(euler.fi)
    max_errors["midpoint_koniec"] = math.degrees(midpoint.fi)
    max_errors["rungekutta_koniec"] = math.degrees(rungekutta.fi)
    return max_errors


def main() -> None:
    rc = generate_pmkzad01()
    pendulum = generate_pmkzad02()

    print("PMKZAD01")
    print(f"max blad Euler = {rc['euler']:.6f}")
    print(f"max blad Midpoint = {rc['midpoint']:.6f}")
    print(f"max blad Runge-Kutta = {rc['rungekutta']:.6f}")
    print()
    print("PMKZAD02")
    print(f"okres malych wychylen = {pendulum['okres']:.6f} s")
    print(f"max blad Euler = {pendulum['euler']:.6f} deg")
    print(f"max blad Midpoint = {pendulum['midpoint']:.6f} deg")
    print(f"max blad Runge-Kutta = {pendulum['rungekutta']:.6f} deg")


if __name__ == "__main__":
    main()
