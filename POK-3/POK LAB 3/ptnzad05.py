from math import pi, sqrt
from random import Random
from assets import DoubleWriter, inputWithDefault


def pi_iterations(n, seed):
    random = Random(seed)
    inside = 0

    for i in range(1, n + 1):
        x = random.random() - 0.5
        y = random.random() - 0.5
        r = sqrt(x * x + y * y)
        if r < 0.5:
            inside += 1

        result = 4.0 * inside / i
        yield i, inside, result, abs(result - pi)


def main():
    doubleWriter = DoubleWriter("ptnzad05")

    n = inputWithDefault("Podaj ilosc punktow", 100000, int)
    seed = 301954

    doubleWriter.print("N =", n, " seed =", seed)
    doubleWriter.print()
    doubleWriter.print("| {0:>3} | {1:>20} | {2:>13} |".format("i", "pi_i", "roznica"))
    doubleWriter.print("+-{0:>3}-+-{1:>20}-+-{2:>13}-+".format("---", "--------------------", "-------------"))

    inside = 0
    result = 0.0
    for i, current_inside, value, difference in pi_iterations(n, seed):
        inside = current_inside
        result = value
        doubleWriter.print("| {0:3d} | {1:20.15f} | {2:13.6e} |".format(i, value, difference))

    doubleWriter.print()
    doubleWriter.print("No =", inside)
    doubleWriter.print("pi_N = {0:.15f}".format(result))
    doubleWriter.print("math.pi = {0:.15f}".format(pi))
    doubleWriter.print("blad = {0:.3e}".format(abs(result - pi)))


if __name__ == "__main__":
    main()
