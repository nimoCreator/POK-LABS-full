"""********************************"""
"""* Wysylanie i obsluga wyjatkow *"""
"""********************************"""

try:
    x = int(input("x = "))
    if x == 0: raise ValueError
except ValueError:
    print("Bladna wartosc")
else:
    y = 1/x
    print("1/x =",y)
finally:
    print("-koniec-")
