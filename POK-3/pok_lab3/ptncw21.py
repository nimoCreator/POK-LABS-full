"""********************************************"""
"""* Kod glowny skryptu a importowanie modulu *"""
"""********************************************"""

zmienna = 123 # nazwy w zakresie globalnym pliku o publicznym lub prywatnym zasiego przenoszonym na inny skrypt importujacy ten modul

def funkcja(x): return 2*x+1

_prywatny = -1

def main(): # kod glowny jedynie dla pliku traktowango jako skrypt 
	print("kod glowny skryptu")
	global _prywatny
	_prywatny = +1 if _prywatny<0 else (_prywatny+1)
	print("zostal wywolany",_prywatny,"raz/razy")

if __name__ == "__main__": # kod globalny pliku wykonywany gdy plik jest wykonywanylnym skryptem lub importowalnym modulem
	main()