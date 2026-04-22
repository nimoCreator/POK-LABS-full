"""*********************************"""
"""* Zakresy skryptu i zasieg nazw *"""
"""*********************************"""

a = 10
def funkcja1():
	b = 20 # element lokalny
	print("lokalny: b =",b)
	a = 21 # przysloniecie elementu globalnego przez lokalny
	print("lokalny: a =",a)
print("1 -->")
print("globalny: a =",a)
funkcja1()
print("globalny: a =",a)

a = 10
def funkcja2():
	global b # wprowadzenie elementu do zakresu globalnego
	b = 20
	print("lokalny: b =",b)
	global a # uzycie elementu z zakresu globalnego
	print("lokalny: a =",a)
	a = 21
	print("lokalny: a =",a)
print("2 -->")
print("globalny: a =",a)
funkcja2()
print("globalny: a =",a)
print("globalny: b =",b)

