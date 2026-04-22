from math import tan
from assets import DoubleWriter, inputWithDefault


def f(x):
    return tan(x) ** 3 - 4.0 * tan(x)


def bisect_eps(a, b, eps):
    result = []
    i = 0
    while True:
        i += 1
        x = 0.5 * (a + b)
        fx = f(x)
        result.append((i, a, b, x, fx))
        if abs(fx) <= eps:
            break
        if f(a) * fx <= 0.0:
            b = x
        else:
            a = x
    return result


def print_table(rows, doubleWriter):
    doubleWriter.print("| {0:>3} | {1:>14} | {2:>14} | {3:>14} | {4:>13} |".format("i", "a", "b", "x", "f(x)"))
    doubleWriter.print("+-{0:>3}-+-{1:>14}-+-{2:>14}-+-{3:>14}-+-{4:>13}-+".format("---", "--------------", "--------------", "--------------", "-------------"))
    for i, a, b, x, fx in rows:
        doubleWriter.print("| {0:3d} | {1:14.10f} | {2:14.10f} | {3:14.10f} | {4:13.6e} |".format(i, a, b, x, fx))


def main():
    doubleWriter = DoubleWriter("ptnzad06b")

    a = 1.0
    b = 1.2
    default_eps = 1.0e-10

    eps = inputWithDefault("Podaj dokladnosc", default_eps, float)

    doubleWriter.print("tan^3(x) - 4*tan(x) = 0")
    doubleWriter.print("wersja b - dokladnosc: eps = {0:.1e}".format(eps))
    doubleWriter.print()

    rows = bisect_eps(a, b, eps)
    print_table(rows, doubleWriter)
    doubleWriter.print("x = {0:.15f}".format(rows[-1][3]))
    doubleWriter.print("liczba krokow =", rows[-1][0])


if __name__ == "__main__":
    main()
