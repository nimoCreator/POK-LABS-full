import sys 
from sage.all import *

sigma = 10
z = var('z')
m0 = 128 
P = exp(-(1/2)*z**2/sigma**2)/(sqrt(2*pi)*sigma)
m = m0*exp(-(1/2)*z**2) 
mb = m0/sqrt(1+sigma**2)

f = lambda u: 1-2*(numerical_integral(u*P*m**2/(mb*(u*mb+(1-u)*m)), 0, Infinity))[0] 
print( find_root(f, 0.1, 1, rtol=0.01) )