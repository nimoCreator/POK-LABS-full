"""*************************************"""
"""* Funkcja rekurencyjna i generujaca *"""
"""*************************************"""

def fact(x): # zwraca silnie z liczby
    if x == 1:
        return 1
    else:
        return (x * fact(x-1))  # zwraca rezultat z wywolaniem rekurencyjnym

print("silnia ze 5 =",fact(5))

def fibo(x): # generuje ciag fibonacciego
    a, b = 0, 1
    while a < x:
        yield a  # dostarcza rezultat z wywolaniem przerywanym 
        a, b = b, a+b
		
print("fibonacci do 100:")
for i in fibo(100):
    print(i)