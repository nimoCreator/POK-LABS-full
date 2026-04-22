#!/usr/bin/env sage -python

import sys 
from sage.all import *

x = var('x')    # define a variable x
print( integral(x*sin(x**2), x) )

print( integral(x/(x**2+1), x, 0, 1) )

f = 1/((1+x)*(x-1))
print( f.partial_fraction(x) )

t = var('t')    # define a variable t
x = function('x')(t)   # define x to be a function of that variable
DE = diff(x, t) + x - 1
print( desolve(DE, [x,t]) )

s = var("s")
t = var("t")
f = t**2*exp(t) - sin(t)
print( f.laplace(t,s) )

t,x,y = PolynomialRing(RealField(10),3,"txy").gens()
f = y; g = -x - y * t
print( eulers_method_2x2(f,g, 0, 1, 0, 1/4, 1) )

x = polygen(QQ, 'x')
print( chebyshev_U(2,x) )

print( bessel_I(1,1).n(250) )

theta = var('theta')
print( solve(cos(theta)==sin(theta), theta) )

phi = var('phi')
print( find_root(cos(phi)==sin(phi),0,pi/2) )

var('x y p q')
eq1 = p+q==9
eq2 = q*y+p*x==-6
eq3 = q*y**2+p*x**2==24
print( solve([eq1,eq2,eq3,p==1],p,q,x,y) )

u = var('u')
print( diff(sin(u), u) )