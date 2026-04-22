"""********************************************************"""
"""* Zakresy i wycinki danych oraz wyrazenie kompletnosci *"""
"""********************************************************"""

zakres = range(3, 10, 2) # zakres liczb od 3 do mniej niz 10 co 2
print("zakres:",zakres)
print("dlugosc =",len(zakres))
print("pierwszy =",zakres[0])
print("ostatni =",zakres[-1])

lista = [ 2*a for a in range(10) if (a%2==0) ] # lista liczb od 0 do mniej niz 10 co 4
print("lista:",lista)
print("dlugosc =",len(lista))
print("pierwszy =",lista[0])
print("ostatni =",lista[-1])

zbior = { 2*a for a in range(10) if (a%2==0) } # zbior liczb od 0 do mniej niz 10 co 4
print("zbior:",zbior)
print("dlugosc =",len(zbior))
print("0 in zbior =",0 in zbior)
print("17 not in zbior =",17 not in zbior)

wycinek = slice(4, 16, 2) # wycinek pozycji od 4 do mniej niz 16 co 2
print("wycinek:",wycinek)
dane = list(range(20)) # lista danych liczbowych od 0 do 19 co 1
print("dane:",dane)
print("wyciete =",dane[wycinek]) # liczby wyciete z listy danych 
print("wyciete =",dane[4:16:2]) # zapis rownowazny przy uzyciu konstruktu wycinka jedynie w operacji indeksowania
print("dane[:5] =",dane[:5]) # brak (None) startu to domyslnie 0  
print("dane[5:] =",dane[5:]) # brak (None) stopu to domyslnie len(dane)
print("dane[:] =",dane[:]) # domyslnie 0:len:1 czyli cala lista danych

mapa = {} # konstrukcja macierzy n x m w postaci mapy (slownika) indeksowanej krotka indeksow (i,j) = (1..n,1..m)
n = m = 3
for i in range(1,n+1):
	for j in range(1,m+1): mapa[i,j] = i*10+j
for j in range(1,m+1): mapa[...,j] = [ mapa[i,j] for i in range(1,n+1) ]
for i in range(1,n+1): mapa[i,...] = [ mapa[i,j] for j in range(1,m+1) ]
# uwaga: jako kluczy w mapie mozna zamiast literalow uzyc wielokropka jako operatora pominiecia ale nie mozna uzyc wycinkow

print("mapa:",mapa) # wydruk calej mapy
print("dlugosc:",len(mapa))

print("mapa[1,2] =",mapa[1,2]) # wydruk elementu macierzy
print("mapa[1,...] =",mapa[1,...]) # wydruk wiersza macierzy
print("mapa[...,2] =",mapa[...,2]) # wydruk kolumny macierzy

print("macierz: [") # wydruk calej macierzy
for i in range(1,n+1): print(mapa[i,...])
print("]")



