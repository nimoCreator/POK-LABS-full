from pylab import show
from pylab import pause
from matplotlib.pyplot import plot
from math import sin
from math import pi

wykres=[]
N = 200
for i in range(N): wykres.append(sin(i*2*pi/(N-1)))

%matplotlib inline

show(plot(wykres)); pause(5)
