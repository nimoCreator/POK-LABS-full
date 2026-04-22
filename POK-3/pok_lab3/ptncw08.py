"""************************************"""
"""* Instrukcja i wyrazenie warunkowe *"""
"""************************************"""

a = input("podaj x: ")
print("typ a:",type(a))
print("wartosc a:",a)

x = int(a)
print("typ x:",type(x))
print("wartosc x:",x)

if x>0: 
	print("plus") 
elif x<0: 
	print("minus") 
else: print("zero") 

y = 111 if x>=1 else "000"
print(y)



