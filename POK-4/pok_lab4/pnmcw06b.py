"""******************************************************************"""
"""* Generowanie tekstu zaszyfrowanego do lamania z uzyciem PyTorch *"""
"""******************************************************************"""

import io, string
import numpy as np

chars = list(string.ascii_uppercase) # alfabet znakow w tekstach
chcnt = len(chars) # liczba znakow w alfabecie
isize = 10 # dlugosc tekstu jawnego
trcnt = 900 # liczba rekordow w zbiorze treningowym
tscnt = 100 # liczba rekordow w zbiorze testowym
shift = 10 # przesuniecie znakow alfabetu w szyfrze Cezara

def genLine():
    n = shift # biezace przesuniecie w szyfrze Cezara
    dat = np.random.randint(0, chcnt-1, isize)
    cph = str().join(chars[(i+n) % chcnt] for i in dat)
    pln = str().join(chars[i] for i in dat)
    return cph+"\t"+pln+"\n"

with io.open("train.txt", 'w') as file: # generuje plik treningowy
    for _ in range(trcnt):
        file.write(genLine())
        
with io.open("test.txt", 'w') as file: # generuje plik testowy
    for _ in range(tscnt):
        file.write(genLine())