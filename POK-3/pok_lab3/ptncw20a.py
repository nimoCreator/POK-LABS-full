"""***********************************************"""
"""* Zakresy skryptu i zasieg nazw - plik modulu *"""
"""***********************************************"""

zmienna = 123 # globalne nazwy publicznie dostepne dla skryptu importujacego modul

def funkcja(x): return 2*x+1

_prywatny = -1 # globalna nazwa o dostepie prywatnym w importowaniu modulu

print("import modulu o nazwie",__name__) # kod globalny wykonywany przy importowaniu modulu 
