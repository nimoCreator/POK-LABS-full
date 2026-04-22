"""***************************************************"""
"""* Instrukcja iteracyjna, przerwania i kontynuacji *"""
"""***************************************************"""

i = 9
while i>0: 
	x = 10**i
	print(i,") x =",x)
	i = i-1 
print("i =",i)

i = 9
while i>0: 
	x = 10**i
	if (i==5): break
	print(i,") x =",x)
	i = i-1 
print("i =",i)

i = 9
while i>0: 
	x = 10**i
	i = i-1
	if (i==5): continue
	print(i+1,") x =",x) 
print("i =",i)
