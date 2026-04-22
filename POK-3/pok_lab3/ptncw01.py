"""*************************************"""
"""* Argumenty wejsciowe i kod wyjscia *"""
"""*************************************"""

import sys # konieczny import elementow bibliotecznych zwiazanych z systemem (maszyna+interpreter) Python

argc = len(sys.argv) # argumenty zapisane w linii polecen i dostepne w punkcie wejscia do skryptu

print("argumenty:",argc)

for arg in sys.argv[0:argc:1]: # instrukcja do wykonania przez petle for moze byc podana po dwukropku
	print("argv[",sys.argv.index(arg),"] =",arg) # lub mozna podac caly blok instrukcji za wcieciem klawiszami TAB lub 4 x SPACE

sys.exit(567) # przerwanie interpretacji skryptu z podanym kodem dostepnym w punkcie wyjscia ze skryptu i mozliwym do odczytania w linii polecen
print("-koniec-") 
