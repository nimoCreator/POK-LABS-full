from math import e, factorial


def approximate_e(n):
    suma = 0.0
    for k in range(n + 1):
        suma += 1.0 / factorial(k)
    return suma


def main():
    n = 15
    result = approximate_e(n)

    print("n =", n)
    print("e_N = {0:.15f}".format(result))
    print("math.e = {0:.15f}".format(e))
    print("blad = {0:.3e}".format(abs(result - e)))


if __name__ == "__main__":
    main()
