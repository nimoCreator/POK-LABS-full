from math import exp


def f(x):
    return exp(x - 1.0) + x * x - 2.0


def secant(a, b):
    fa = f(a)
    fb = f(b)
    return (a * fb - b * fa) / (fb - fa)


def secant_steps(a, b, fixed_a, n):
    result = []
    for i in range(1, n + 1):
        x = secant(a, b)
        fx = f(x)
        result.append((i, a, b, x, fx))
        if fixed_a:
            b = x
        else:
            a = x
    return result


def secant_eps(a, b, fixed_a, eps):
    result = []
    i = 0
    while True:
        i += 1
        x = secant(a, b)
        fx = f(x)
        result.append((i, a, b, x, fx))
        if abs(fx) <= eps:
            break
        if fixed_a:
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
    a = 0.0
    b = 2.0
    fixed_a = False
    n = 12
    eps = 1.0e-10

    print("exp(x - 1) + x^2 - 2 = 0")
    print("punkt staly =", "a" if fixed_a else "b")
    print()
    print("wersja z liczba ciec: n =", n)
    rows = secant_steps(a, b, fixed_a, n)
    print_table(rows)
    print("x({0}) = {1:.15f}".format(n, rows[-1][3]))
    print()

    print("wersja z dokladnoscia: eps = {0:.1e}".format(eps))
    rows = secant_eps(a, b, fixed_a, eps)
    print_table(rows)
    print("x = {0:.15f}".format(rows[-1][3]))
    print("liczba krokow =", rows[-1][0])


if __name__ == "__main__":
    main()
