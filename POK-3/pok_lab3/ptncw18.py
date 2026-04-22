"""***********************************************"""
"""* Dziedziczenie klas i polimorfizm dynamiczny *"""
"""***********************************************"""

class Figura:
    def __init__(self,r):
        self.obwod = r
    def powieksz(self,dr): # definicja metody wirtualnej nieabstrakcyjnej
        self.obwod += dr

def Obrazek(fig):
    fig.rysuj() # wywolanie metody abstrakcyjnej poprzez polimorfizm dynamiczny
    fig.powieksz(0.1) # wywolanie metody wirtualnej z klasy pochodnej
    fig.rysuj()

class Kwadrat(Figura): # klasa pochodna i dziedziczenie klas bazowej
    def __init__(self,a):
        Figura.__init__(self,4*a)
        self.bok = a
    def powieksz(self,da): # pokrycie metody wirtualnej z klasy bazowej
        Figura.powieksz(self,4*da)
        self.bok += da
    def rysuj(self): # definicja metody abstrakcyjnej
        print("< kwadrat o boku",self.bok,"i obwodzie",self.obwod,">")

f = Kwadrat(1.2)
Obrazek(f)


