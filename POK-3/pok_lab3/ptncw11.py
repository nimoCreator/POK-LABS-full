"""**************************************"""
"""* Kontrola bledow a obsluga wyjatkow *"""
"""**************************************"""

a = int( input("podaj a: ") )
b = int( input("podaj b: ") )

assert b!=0, "dzielenie przez zero"
x = a/b
print(x)

try:
    print(a/b)
except Exception as przyczyna:
    raise RuntimeError("Blad") from przyczyna
