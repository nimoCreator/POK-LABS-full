"""****************************************"""
"""* Instrukcja wyrazeniowa i przypisania *"""
"""****************************************"""

6*7+8
a = 6*7+8
b = c = a
a+=11
print(a,b,c)

u = (v:=123) # osobny operator przypisania := daje wyrazenie zwracajace przypisana wartosc, wymaga wersji Python 3.8
print(u,v)

x = [1,2,3]
print(x)
x[1] = 6*7+8
print(x)

x, y, z = [1,2,3]
print(x)
print(y)
print(z)

s: str = "tekst" # adnotacja od Python 3.5 dotyczaca statycznej kontroli typow jest jedynie podpowiedzia dla osobnych analizatorow poprawnosci typow np. Mypy
print(s)
s = 3.14 # analizator skryptu sprawdza tekstowo jego poprawnosc i zglasza tutaj blad zgodnosci typow bez koniecznosci uruchomienia skryptu
print(s)

