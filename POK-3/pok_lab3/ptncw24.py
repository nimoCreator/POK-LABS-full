"""************************************"""
"""* Dodatkowe pakiety oprogramowania *"""
"""************************************"""

import numpy as np # pakiet obliczen numerycznych

tab = np.array([ [11,12,13], 
                 [21,22,23], 
                 [31,32,33] ]) # mutacyjna tablica numeryczna zadana lista (tensorem) liczb calkowitych lub rzeczywistych
print("id:", id(tab) )
print("type:", type(tab) )
print("tab =", tab )

print("tab[1] =", tab[1] )
print("tab[1,:] =", tab[1,:] )
print("tab[1,...] =", tab[1,...] )
print("tab[1,2] =", tab[1,2] )
print("tab[:,2] =", tab[:,2] )
print("tab[...,2] =", tab[...,2] )
print("tab[...] =", tab[...] ) # operator wielokropka uzywany we tablicy numerycznej podobnie jak we zwyklym slowniku






