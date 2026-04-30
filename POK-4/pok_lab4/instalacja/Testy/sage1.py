import sys 
from sage.all import *

P = plot(sin, (0,10), plot_points=100)
save(P,'/tmp/sage0.png',axes=False,aspect_ratio=True)
os.system('powershell.exe -Command "& {start E:\Tmps\sage0.png}"')
