"""*****************************************"""
"""* Petla zakresowa i wyrazenie generacji *"""
"""*****************************************"""

for n in [1,2,3]: print(n)

for x in (123,"abc"): print(x)

for i in range(10,100,5):
	x = i**2
	print(i,") x =",x)
print("i =",i)

gen = ( 2*a for a in [1,2,3,4,5] if a%2!=0 )
for i in gen:
	x = i**2
	print(i,") x =",x)
print(gen)