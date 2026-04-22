"""***********************************"""
"""* Slowniki (mapy) i zbiory danych *"""
"""***********************************"""

mapa = { 'a': 123 }
mapa['a'] = 567
mapa['b'] = 234
print(mapa) 
print(len(mapa))
x = {}
print(x.get('a'))

zbior = { 12, 1.3, 'a', 'b' }
zbior.add("piec")
print(zbior) 
print(len(zbior))

A = {1,4}
B = {4,7}
print("A:",A, " , B:",B)
print("A | B =",A | B)
print("A - B =",A - B)
print("A ^ B =",A ^ B)
print("A & B =",A & B)
print("B - A =",B - A)
print("12 not in A =",12 not in A) 

