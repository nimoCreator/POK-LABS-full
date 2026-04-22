"""******************************"""
"""* Asercja i obsluga wyjatkow *"""
"""******************************"""

a = 12
try:
    dana = input("podaj liczbe: ")
    b = int(dana)
    assert b!=0, "dzielenie przez zero"
    x = a/b
    print("wynik dzielenia:",x)
except ValueError:
    print("Bledny typ danej")
except AssertionError as blad:
    print("Blad:",blad)
