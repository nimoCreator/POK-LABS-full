from math import pi, sin
from assets import DoubleWriter, inputWithDefault


def iterate(x0, nmax):
    x = x0
    values = [x]
    for _ in range(nmax):
        x = 2.0 * sin(x)
        values.append(x)
    return values


def main():
    doubleWriter = DoubleWriter("ptnzad03")

    x0 = inputWithDefault("Podaj x0", pi / 2.0, float)
    nmax = inputWithDefault("Podaj nmax", 30, int)
    values = iterate(x0, nmax)

    doubleWriter.print("x0 = {0:.10f}  nmax = {1}".format(x0, nmax))
    doubleWriter.print()
    doubleWriter.print("| {0:>3} | {1:>16} | {2:>13} |".format("i", "x", "f(x)"))
    doubleWriter.print("+-{0:>3}-+-{1:>16}-+-{2:>13}-+".format("---", "----------------", "-------------"))

    for i, x in enumerate(values):
        fx = 2.0 * sin(x) - x
        doubleWriter.print("| {0:3d} | {1:16.10f} | {2:13.6e} |".format(i, x, fx))

    x = values[-1]
    doubleWriter.print()
    doubleWriter.print("x({0}) = {1:.15f}".format(nmax, x))
    doubleWriter.print("|(2*sin(x) - x)/x| = {0:.3e}".format(abs((2.0 * sin(x) - x) / x)))


if __name__ == "__main__":
    main()
