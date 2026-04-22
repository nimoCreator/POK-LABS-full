from math import tan
from assets import DoubleWriter, inputWithDefault


def f(x):
    return tan(x) ** 3 - 4.0 * tan(x)


def bisect_steps(a, b, n):
    result = []
    for i in range(1, n + 1):
        x = 0.5 * (a + b)
        fx = f(x)
        result.append((i, a, b, x, fx))
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
    doubleWriter = DoubleWriter("ptnzad06a")

    a = 1.0
    b = 1.2
    default_n = 20

    n = inputWithDefault("Podaj liczbe iteracji", default_n, int)

    doubleWriter.print("tan^3(x) - 4*tan(x) = 0")
    doubleWriter.print("wersja a - liczba iteracji: n =", n)
    doubleWriter.print()

    rows = bisect_steps(a, b, n)
    print_table(rows, doubleWriter)
    doubleWriter.print("x({0}) = {1:.15f}".format(n, rows[-1][3]))


if __name__ == "__main__":
    main()
