from math import e, factorial
from assets import DoubleWriter, inputWithDefault


def approximate_e(n):
    suma = 0.0
    for k in range(n + 1):
        suma += 1.0 / factorial(k)
    return suma


def e_iterations(n):
    suma = 0.0
    for i in range(n + 1):
        suma += 1.0 / factorial(i)
        yield i, suma, abs(suma - e)


def main():
    doubleWriter = DoubleWriter("ptnzad04")

    n = inputWithDefault("Podaj n", 15, int)

    doubleWriter.print("n =", n)
    doubleWriter.print()
    doubleWriter.print("| {0:>3} | {1:>20} | {2:>13} |".format("i", "e_i", "roznica"))
    doubleWriter.print("+-{0:>3}-+-{1:>20}-+-{2:>13}-+".format("---", "--------------------", "-------------"))

    result = 0.0
    for i, value, difference in e_iterations(n):
        result = value
        doubleWriter.print("| {0:3d} | {1:20.15f} | {2:13.6e} |".format(i, value, difference))

    doubleWriter.print()
    doubleWriter.print("e_N = {0:.15f}".format(result))
    doubleWriter.print("math.e = {0:.15f}".format(e))
    doubleWriter.print("blad = {0:.3e}".format(abs(result - e)))


if __name__ == "__main__":
    main()
