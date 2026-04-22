from math import pi, sqrt
from random import Random


def approximate_pi(n, seed=301954):
    random = Random(seed)
    inside = 0

    for _ in range(n):
        x = random.random() - 0.5
        y = random.random() - 0.5
        r = sqrt(x * x + y * y)
        if r < 0.5:
            inside += 1

    return 4.0 * inside / n, inside


def main():
    n = 100000
    seed = 301954
    result, inside = approximate_pi(n, seed)

    print("N =", n, " seed =", seed)
    print("No =", inside)
    print("pi_N = {0:.15f}".format(result))
    print("math.pi = {0:.15f}".format(pi))
    print("blad = {0:.3e}".format(abs(result - pi)))


if __name__ == "__main__":
    main()
