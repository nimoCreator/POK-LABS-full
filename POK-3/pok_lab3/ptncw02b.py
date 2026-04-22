"""**********************************************************"""
"""* Obiekty danych zlozonych i ich przetwarzanie mutacyjne *"""
"""**********************************************************"""

import sys

a = [1,2,3]
print("tozsamosc a:",id(a))
print("typ a:",type(a))
print("wartosc a:",a)
print("licznik a:",sys.getrefcount(a)-1)

b = [1,2,3]
print("tozsamosc b:",id(b))
print("typ b:",type(b))
print("wartosc b:",b)
print("licznik b:",sys.getrefcount(b)-1)

a = [5,6,7]
b = a
print("tozsamosc a,b:",id(a),id(b))
print("tozsame a i b:",a is b)
print("typ a,b:",type(a),type(b))
print("wartosc a,b:",a,b)
print("licznik a:",sys.getrefcount(a)-1)
print("licznik b:",sys.getrefcount(b)-1)
print("licznik [5,6,7]:",sys.getrefcount([5,6,7])-1)

a.append(8)
print("tozsamosc a,b:",id(a),id(b))
print("tozsame a i b:",a is b)
print("typ a,b:",type(a),type(b))
print("wartosc a,b:",a,b)
print("licznik a:",sys.getrefcount(a)-1)
print("licznik b:",sys.getrefcount(b)-1)

del a
print("tozsamosc b:",id(b))
print("typ b:",type(b))
print("wartosc b:",b)
print("licznik b:",sys.getrefcount(b)-1)
