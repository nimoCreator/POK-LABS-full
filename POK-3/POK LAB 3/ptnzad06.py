from math import tan


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


def print_table(rows):
    print("| {0:>3} | {1:>14} | {2:>14} | {3:>14} | {4:>13} |".format("i", "a", "b", "x", "f(x)"))
    print("+-{0:>3}-+-{1:>14}-+-{2:>14}-+-{3:>14}-+-{4:>13}-+".format("---", "--------------", "--------------", "--------------", "-------------"))
    for i, a, b, x, fx in rows:
        print("| {0:3d} | {1:14.10f} | {2:14.10f} | {3:14.10f} | {4:13.6e} |".format(i, a, b, x, fx))


def main():
    a = 1.0
    b = 1.2
    n = 20
    eps = 1.0e-10

    print("tan^3(x) - 4*tan(x) = 0")
    print()
    print("wersja z liczba polowien: n =", n)
    rows = bisect_steps(a, b, n)
    print_table(rows)
    print("x({0}) = {1:.15f}".format(n, rows[-1][3]))
    print()

    print("wersja z dokladnoscia: eps = {0:.1e}".format(eps))
    rows = bisect_eps(a, b, eps)
    print_table(rows)
    print("x = {0:.15f}".format(rows[-1][3]))
    print("liczba krokow =", rows[-1][0])


if __name__ == "__main__":
    main()
