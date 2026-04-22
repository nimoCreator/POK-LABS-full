"""*********************************"""
"""* Zakresy skryptu i zasieg nazw *"""
"""*********************************"""

def funkcja1():
	x = 10
	print("funkcja1: x =",x)
	def funkcja2():
		global y # wprowadzenie elementu do zakresu skryptu
		y = 21
		print("funkcja2: y =",y)
		nonlocal x # uzycie elementu z zakresu nadrzednej funkcji
		print("funkcja2: x =",x)
		x = 20
		print("funkcja2: x =",x)
	funkcja2()
	print("funkcja1: x =",x)
print("lokalne>")
funkcja1()
print("globalny> y =",y)
