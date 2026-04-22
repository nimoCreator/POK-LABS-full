"""*****************************************"""
"""* Definicja klasy i tworzenie instancji *"""
"""*****************************************"""

class Osoba: # domyslnie dziedziczy po klasie object
    miasto = "Katowice" # pole statyczne
    def __init__(self,im,naz): # metoda-inicjalizator wlasnego obiektu
        self.imie = im # pola zwykle w obiekcie wlasnym
        self.nazwisko = naz
    def wyswietl(self): # metoda zwykla w obiekcie wlasnym
        print(Osoba.miasto,": ",\
        self.imie," ",self.nazwisko)

obiekt = Osoba("Jan", "Kowalski") # konstrukcja obiektu
obiekt.wyswietl() # wywolanie metody na rzecz obiektu