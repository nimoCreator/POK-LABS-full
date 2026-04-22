"""***********************************"""
"""* Importowanie modulow i pakietow *"""
"""***********************************"""

import builtins  # domyslny import i uzycie przestrzeni zawierajacej kilka elementow bibliotecznych wbudowanych do jezyka, np funkcje print, type 

def print(x): # redefinicja funkcji wbudowanej wymaga poznizszej kwalifikacji
	builtins.print(x) # uzycie nazwy kwalifikowanej przez nazwe przestrzeni wbudowanej
	return type(x) # jezeli nazwa nie jest przeciazona to kwalifikacja jest zbedna
y = print(123)
print(y)

import math  # importowanie calego modulu
a = math.fabs(-1.2)
print(a)

from math import *  # importowanie wszystkich elementow publicznych modulu
b = sin(3.14)
print(b)

import xml.dom  # importowanie calego modulu w pakiecie
print( xml.dom.getDOMImplementation() )
  
import xml.dom as xd  # importowanie modulu z nazwa aliasowa 
print( xd.getDOMImplementation() )

from xml import dom  # importowanie calego modulu z pakietu
print( dom.getDOMImplementation() )

from xml.dom import getDOMImplementation  # importowanie funkcji z modulu w pakiecie
print( getDOMImplementation() )

from xml.dom import getDOMImplementation as xdimp  # importowanie funkcji z nazwa aliasowa 
print( xdimp() )
