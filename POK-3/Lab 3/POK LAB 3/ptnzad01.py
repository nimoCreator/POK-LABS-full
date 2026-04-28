from math import sqrt
from assets import DoubleWriter, inputWithDefault


def SQRT(b, n=10, x0=1.5):
    x = x0
    for _ in range(n):
        x = 0.5 * (x + b / x)
    return x


def main():
    doubleWriter = DoubleWriter("ptnzad01")

    b = inputWithDefault("Podaj b", 2.0, float)
    x0 = inputWithDefault("Podaj przyblizenie poczatkowe", 1.5, float)
    n = inputWithDefault("Podaj ilosc powtorzen", 10, int)

    doubleWriter.print("b =", b, " n =", n, " x0 =", x0)
    doubleWriter.print()
    doubleWriter.print("| {0:>3} | {1:>16} | {2:>16} | {3:>16} |".format("i", "x", "sqrt(b)", "roznica"))
    doubleWriter.print("+-{0:>3}-+-{1:>16}-+-{2:>16}-+-{3:>16}-+".format("---", "----------------", "----------------", "----------------"))

    x = x0
    for i in range(n + 1):
        doubleWriter.print("| {0:3d} | {1:16.10f} | {2:16.10f} | {3:16.10f} |".format(i, x, sqrt(b), abs(x - sqrt(b))))
        x = 0.5 * (x + b / x)

    result = SQRT(b, n, x0)
    doubleWriter.print()
    doubleWriter.print("x({0}) = {1:.15f}".format(n, result))
    doubleWriter.print("sqrt(b) = {0:.15f}".format(sqrt(b)))
    doubleWriter.print("blad = {0:.3e}".format(abs(result - sqrt(b))))


if __name__ == "__main__":
    main()
