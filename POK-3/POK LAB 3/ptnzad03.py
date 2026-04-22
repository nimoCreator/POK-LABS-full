from math import pi, sin


def iterate(x0, nmax):
    x = x0
    values = [x]
    for _ in range(nmax):
        x = 2.0 * sin(x)
        values.append(x)
    return values


def main():
    x0 = pi / 2.0
    nmax = 30
    values = iterate(x0, nmax)

    print("x0 = {0:.10f}  nmax = {1}".format(x0, nmax))
    print()
    print("| {0:>3} | {1:>16} | {2:>13} |".format("i", "x", "f(x)"))
    print("+-{0:>3}-+-{1:>16}-+-{2:>13}-+".format("---", "----------------", "-------------"))

    for i, x in enumerate(values):
        fx = 2.0 * sin(x) - x
        print("| {0:3d} | {1:16.10f} | {2:13.6e} |".format(i, x, fx))

    x = values[-1]
    print()
    print("x({0}) = {1:.15f}".format(nmax, x))
    print("|(2*sin(x) - x)/x| = {0:.3e}".format(abs((2.0 * sin(x) - x) / x)))


if __name__ == "__main__":
    main()
