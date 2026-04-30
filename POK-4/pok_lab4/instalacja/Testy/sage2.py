import sys 
from sage.all import *

x=var('x')
Pa=plot(lambda x:fibonacci(round(x)), (x,1,10))
save(Pa,'/tmp/sage1a.png',axes=False,aspect_ratio=True)
os.system('powershell.exe -Command "& {start E:\Tmps\sage1a.png}"')

g1 = plot(x**2*sin(1/x), (x, -2, 2), axes_labels=['$x$', '$y$'])
g2 = plot(x**2*sin(1/x), (x, -0.3, 0.3), axes_labels=['$x$', '$y$'],frame=True)
Pb=g1.inset(g2, pos=(0.15, 0.7, 0.25, 0.25))
save(Pb,'/tmp/sage1b.png',axes=False,aspect_ratio=True)
os.system('powershell.exe -Command "& {start E:\Tmps\sage1b.png}"')





