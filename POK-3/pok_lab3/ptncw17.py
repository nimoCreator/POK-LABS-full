"""****************************************"""
"""* Definicje klas z metodami magicznymi *"""
"""****************************************"""

class Funktor:
    def __init__(self,a,b): # metoda-inicjalizator obiektu wlasnego
        print("metoda inicjujaca funktor")
        self.a = a
        self.b = b
    def __call__(self,x): # metoda-operator wolania obiektu wlasnego
        print("metoda wywolania funktora")
        return self.a*x+self.b

f = Funktor(1.1,2.2)
y = f(3.3) # wywolanie metody-operatora identyczne do wywolania zwyklej funkcji
print(y)

class Dziennik:
    def __init__(self, nazwa): # metoda-inicjalizator obiektu wlasnego
        print("metoda inicjujaca dziennik")
        self.nazwa = nazwa
    
    def __enter__(self): # metoda-operator wejscia do zasobow lub kontekstu
        print("metoda otwarcia dziennika")
        self.plik = open(self.nazwa, 'a')
        return self.plik

    def __exit__(self, *wyjatek): # metoda-operator wyjscia z zasobow lub kontekstu
        print("metoda zamkniecia dziennika")
        self.plik.close()
        return True

with open("dziennik.txt", 'a') as plik: # instrukcja do pozyskania i zwalniania zasobow np. pliku
    plik.write("poczatek obliczen:\n")

with Dziennik("dziennik.txt") as dziennik: # mozna ja wykorzystac dla wlasnej klasy manadzera zasobow/kontekstu
    dziennik.write("wykonywanie obliczen...\n")

plik = open("dziennik.txt", 'a') # kod rownowazny z tradycyjna obsluga wyjatkow
try:
    plik.write("zakonczenie obliczen.")
finally:
    plik.close()
