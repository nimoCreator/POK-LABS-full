"""*******************************************"""
"""* Definicja funkcji i wyrazenie funkcyjne *"""
"""*******************************************"""

def funkcja0(): # definicja (deklaracja) funkcji bez kodu
    pass # zamiast instrukcji przejscia mozna tu uzyc specyfikator pominiecia (wielokropek) 
y = funkcja0() 
print(y)  

def funkcja0(): # pelna definicja funkcji ze kodem
    print("kod funkcji")
    return "abc"
y = funkcja0() 
print(y)  

def funkcja0(x): # nowa definicja funkcji ze parametrem pozycyjnym
    print("parametr funkcji:",x)
    return x
# y = funkcja0() # uwaga: nie mozna przeciazac nazwy funkcji 
y = funkcja0("abc") 
print(y)  

def funkcja1(a,b,c=33): # definicja funkcji ze paramterami o argumentach domyslnych na koncu listy parametrow
    print(a,b,c)
    return a+b+c
funkcja1(11,22)   
funkcja1(11,22,33)
y = funkcja1(11,c=33,b=22) # dla parametrow pozycyjnych na koncu listy argumentow mozna podac ich nazwy w dowolnej kolejnosci
print(y)  

def funkcja2(a,*b,**c): # definicja funkcji ze paramterami o argumentach wariadycznych, nienazwanych (*krotka) i nazwanych (**slownik)
    print(a,b,c)
y = funkcja2(1, 2,3,4, u=5, v=6)
print(y)   

funkcja3 = lambda a,b: a*b # wyrazenie funkcji lambda
y = funkcja3(2,3)
print(y)  

def funkcja4(x: str = "xxx") -> str: # adnotacje dla statycznej kontroli typow argumentu i rezultatu od wersji Python 3.5
    print("parametr funkcji:",x)
    return x
y = funkcja4("abc") 
print(y)  
y = funkcja4(3.14) # analizator poprawnosci (np. mypy) skryptu zglosi tu blad zgodnosci typow
print(y)  
