from math import sqrt


def SQRT(b, n=10, x0=1.5):
    x = x0
    for _ in range(n):
        x = 0.5 * (x + b / x)
    return x


def main():
    b = 2.0
    n = 10
    x0 = 1.5

    print("b =", b, " n =", n, " x0 =", x0)
    print()
    print("| {0:>3} | {1:>16} | {2:>16} |".format("i", "x", "sqrt(b)"))
    print("+-{0:>3}-+-{1:>16}-+-{2:>16}-+".format("---", "----------------", "----------------"))

    x = x0
    for i in range(n + 1):
        print("| {0:3d} | {1:16.10f} | {2:16.10f} |".format(i, x, sqrt(b)))
        x = 0.5 * (x + b / x)

    result = SQRT(b, n, x0)
    print()
    print("x({0}) = {1:.15f}".format(n, result))
    print("sqrt(b) = {0:.15f}".format(sqrt(b)))
    print("blad = {0:.3e}".format(abs(result - sqrt(b))))


if __name__ == "__main__":
    main()
