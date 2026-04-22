"""************************************************************"""
"""* Obiekty danych prostych i ich przetwarzanie niemutacyjne *"""
"""************************************************************"""

import sys

a = 1
print("tozsamosc a:",id(a))
print("typ a:",type(a))
print("wartosc a:",a)
print("licznik a:",sys.getrefcount(a)-1)

a = 2
print("tozsamosc a:",id(a))
print("typ a:",type(a))
print("wartosc a:",a)
print("licznik a:",sys.getrefcount(a)-1)

b = 3.4
print("tozsamosc b:",id(b))
print("typ b:",type(b))
print("wartosc b:",b)
print("licznik b:",sys.getrefcount(b)-1)

a = 5
b = 5
print("tozsamosc a,b:",id(a),id(b))
print("tozsame a i b:")
print("  id==id:",id(a)==id(b))
print("  is:",a is b)
print("typ a,b:",type(a),type(b))
print("wartosc a,b:",a,b)
print("licznik a:",sys.getrefcount(a)-1)
print("licznik b:",sys.getrefcount(b)-1)
print("licznik 5:",sys.getrefcount(5)-1)

a = 6
b = a
print("tozsame a i b:",a is b)
print("licznik a:",sys.getrefcount(a)-1)
print("licznik b:",sys.getrefcount(b)-1)
a+=1
print("nietozsame a i b:",a is not b)
print("licznik a:",sys.getrefcount(a)-1)
print("licznik b:",sys.getrefcount(b)-1)
