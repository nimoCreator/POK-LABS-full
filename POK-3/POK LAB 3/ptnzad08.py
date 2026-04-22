from math import cos, exp, log, sin, tan


def SolveNonLinEq(x0, e, f, Df, max_iter=100):
    x = x0
    for step in range(1, max_iter + 1):
        x_next = x - f(x) / Df(x)
        if abs(x_next - x) < e:
            return x_next, step
        x = x_next
    return x, max_iter


def main():
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

    eps = 1.0e-10
    print("eps = {0:.1e}".format(eps))
    print()
    print("| {0:36} | {1:>10} | {2:>16} | {3:>6} | {4:>13} |".format("funkcja", "x0", "x", "kroki", "f(x)"))
    print("+-{0:36}-+-{1:>10}-+-{2:>16}-+-{3:>6}-+-{4:>13}-+".format("-" * 36, "-" * 10, "-" * 16, "-" * 6, "-" * 13))

    for name, x0, f, Df in tests:
        x, steps = SolveNonLinEq(x0, eps, f, Df)
        print("| {0:36} | {1:10.6f} | {2:16.10f} | {3:6d} | {4:13.6e} |".format(name, x0, x, steps, f(x)))


if __name__ == "__main__":
    main()
