"""*************************"""
"""* Listy i krotki danych *"""
"""*************************"""

lista = [ [1, 2.0], "trzy", ['a', 'b'], True ]
print(lista)
print(len(lista)) 
print(lista[0])
print(lista[1:-1]) 

a = [1,2,3]
b = [5,6]
c = a+2*b
print(c==[1,2,3,5,6,5,6]) 

a = [1,2,1,3,5]
a.append(6)
print(a.count(1))
a.clear(); b = []
print(a==b)

krotka = '1', 2.0, 3, "cztery"
print(krotka)
print(len(krotka))
print(krotka.index("cztery"))
print( (), 1, (2,3), 4 )
