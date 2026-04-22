"""************************************************"""
"""* Zakresy skryptu i zasieg nazw - plik skryptu *"""
"""************************************************"""

import ptncw20a # import modulu do skryptu i wykonanie jego globalnego kodu

from ptncw20a import * # lub jedynie import nazw publicznych modulu do skryptu 

a = zmienna # bezposredni dostep w skrypcie do zaimportowanych nazw publicznych
b = funkcja(5)

# c = _prywatny # w przypadku zaimportowanej nazwy prywatnej mozliwy...
c = ptncw20a._prywatny # ...jest jedynie dostep posredni w skrypcie 

print("w skrypcie o nazwie",__name__)
print(a,b,c)
