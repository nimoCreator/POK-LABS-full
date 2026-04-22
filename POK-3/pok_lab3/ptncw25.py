"""************************************"""
"""* Dodatkowe pakiety oprogramowania *"""
"""************************************"""

from pylab import show # pakiety do obliczen i wizualizacji matematycznej 
from matplotlib.pyplot import plot

from math import sin # modul matematyczny z biblioteki standardowej
from math import pi

wykres=[]
N = 200
for i in range(N): wykres.append(sin(i*2*pi/(N-1)))

# polecenie IPython do wybrania trybu wizualizacji wykresu
#%matplotlib qt
#%matplotlib inline
#%matplotlib notebook
"""
Lista trybow wizualizacji graficznej:

none - brak wizualizacji
inline - wizualizacja liniowa w srodowisku IDE
notebook - wizualizacja w interfejsie notatnikowym
glut, wx, osx, qt4, gtk3, pyglet, qt, gtk, tk - wizualizacja w osobnym oknie GUI

uwaga: polecenia interpretera IPython wykonywane sa jedynie w trybie powloki
"""

plot(wykres)
show()
