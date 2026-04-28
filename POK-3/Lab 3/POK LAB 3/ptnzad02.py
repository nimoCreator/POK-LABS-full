from assets import DoubleWriter, inputWithDefault


def primes_to(n):
    pierwsza = [True] * (n + 1)
    pierwsza[0] = False
    pierwsza[1] = False

    for liczba in range(2, n + 1):
        if pierwsza[liczba]:
            for wielokrotnosc in range(liczba + liczba, n + 1, liczba):
                pierwsza[wielokrotnosc] = False

    return [liczba for liczba in range(2, n + 1) if pierwsza[liczba]]


def main():
    doubleWriter = DoubleWriter("ptnzad02")

    n = inputWithDefault("Podaj n", 100, int)
    liczby = primes_to(n)

    doubleWriter.print("N =", n)
    doubleWriter.print("liczby pierwsze:")
    for i, liczba in enumerate(liczby, 1):
        doubleWriter.print("{0:4d}".format(liczba), end="\n" if i % 10 == 0 else "")

    doubleWriter.print()
    doubleWriter.print("ilosc liczb pierwszych =", len(liczby))


if __name__ == "__main__":
    main()
