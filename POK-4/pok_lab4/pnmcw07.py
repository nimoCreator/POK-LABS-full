"""********************************************************************"""
"""* Zastosowanie modulu PyTorch do rozwiazywania rownania przez siec *"""
"""* neuronowa jako przyklad przetwarzania ogolnego problemu regresji *"""
"""********************************************************************"""

import torch
import torch.autograd as autograd
import torch.nn as nn
import torch.optim as optim
import matplotlib.pyplot as plt

#
# Wybor platformy sprzetowej do obliczen rownoleglych
#

device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')
print("device:",device)

#
# Definicja sieci neuronowej (PINN) sterowanej równaniem różniczkowym z konstrukcyjnym warunkiem początkowym
#

class Equation(nn.Module):
    def __init__(self, xo, yo, kc, depth):
        super(Equation, self).__init__()
        
        self.inix = xo
        self.iniy = yo
        self.conk = kc

        self.lossfun = nn.MSELoss(reduction ='mean')
        self.actfun = nn.Tanh()
        self.netdepth = depth
        self.indim = 1
        self.outdim = 1

        self.netchain = nn.Sequential(
            nn.Linear(self.indim, self.netdepth),
            self.actfun,
            nn.Linear(self.netdepth, self.netdepth),
            self.actfun,
            nn.Linear(self.netdepth, self.netdepth),
            self.actfun,
            nn.Linear(self.netdepth, self.netdepth),
            self.actfun,
            nn.Linear(self.netdepth, self.outdim)
        )

    def prediction(self, X): # Rozwiazanie liczbowe przewidywane przez siec neuronowa w konstrukcji warunku poczatkowego
        Y = (X - self.inix) * self.netchain(X) + self.iniy
        return Y
    
    def regression(self, aX): # Strata w regresji dla rownania na rozladowanie kondensatora w obwodzie elektrycznym RC
        X = aX.clone()
        Y = self.forward(X)
        dY_dX = autograd.grad(Y,X,grad_outputs=torch.ones([X.shape[0],1]).to(device), retain_graph=True, create_graph=True)[0]
        L = self.lossfun(dY_dX, -self.conk * Y) # funkcja straty z lewej i prawej strona rownania
        return L

    def solution(self, X): # Referencyjne rozwiazanie rownania otrzymane numerycznie lub metoda calkowania symbolicznego
        Y = self.iniy * torch.exp(-self.conk * (X - self.inix))
        return Y
    
    def forward(self, X): # Pokrycie definicji metody postepowej dla modulu sieciowego nn.Module
        return self.prediction(X)

#
# Inicjowanie srodowiska obliczeniowego
#

# Podane wartosci dla zakresu obliczen, warunku poczatkowego i stalej w rownaniu
x_beg = 0.0
x_end = 10.0
x_init = 0.0
y_init = 1.0
k_cnst = 2.0

# Wartosci pozostalych hiperparametrow w obliczeniach
numpoints = 100
netdepth = 128
learnrate = 0.01
numepochs = 1000

# Utworzenie danych punktow obliczeniowych do rozwiazywania rownania
X_data = torch.linspace(x_beg,x_end,numpoints).view(-1,1).requires_grad_(True)
x_data = X_data.detach().numpy()

# Utworzenie sieci neuronowej jako modelu solwera rownania
model = Equation(x_init, y_init, k_cnst, netdepth)

# Otrzymanie rozwiazania przewidywanego przez siec neuronowa
Y_pred = model.prediction(X_data)
y_pred = Y_pred.detach().numpy()

# Wyznaczenie rozwiazania referencyjnego (symnolicznego lub numerycznego)
Y_refs = model.solution(X_data)
y_refs = Y_refs.detach().numpy()

# Poczatkowy wykres wynikow obliczen
plt.figure(figsize=(8, 6))
plt.plot(x_data, y_refs, 'r-',  label='referencyjne')
plt.plot(x_data, y_pred, 'b--', label='przewidywane')
plt.xlabel('x')
plt.ylabel('y')
plt.legend()
plt.title('Rozwiazanie rownania przed uczeniem sieci')
plt.show()

#
# Uczenie sieci neronowej modelujacej rownanie w problemie regresji
#

print("model:",model)
model.to(device)
optimizer = optim.Adam(model.parameters(),lr=learnrate,amsgrad=False)
epochs = []
losses = []
for epoch in range(numepochs):
    L_regr = model.regression(X_data)
    optimizer.zero_grad()
    L_regr.backward()
    optimizer.step()
    if epoch%(numepochs/100)==0:
        loss = L_regr.item()
        epochs.append(epoch)
        losses.append(loss)
    if epoch%(numepochs/10)==0:
        print("epoch:",epoch,", loss:",loss)

plt.figure(figsize=(8, 6))
plt.plot(epochs, losses, 'g-')
plt.xlabel('i')
plt.ylabel('L')
plt.title('Strata w procesie uczenia sieci')
plt.show()

#
# Ewaluacja nauczonej sieci
#

# Otrzymanie rozwiązania przewidywanego przez nauczona siec neuronowa
Y_pred = model.prediction(X_data)
y_pred = Y_pred.detach().numpy()

# Koncowy wykres wynikow obliczen
plt.figure(figsize=(8, 6))
plt.plot(x_data, y_refs, 'r-',  label='referencyjne')
plt.plot(x_data, y_pred, 'b--', label='przewidywane')
plt.xlabel('x')
plt.ylabel('y')
plt.legend()
plt.title('Rozwiazanie rownania dla sieci nauczonej')
plt.show()

