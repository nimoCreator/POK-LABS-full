"""********************************************************************"""
"""* Zastosowanie modulu PyTorch do rozwiazywania rownania przez siec *"""
"""* neuronowa jako przyklad przetwarzania ogolnego problemu regresji *"""
"""********************************************************************"""

import torch
import torch.autograd as autograd
import torch.nn as nn
import torch.optim as optim
import matplotlib.pyplot as plt
from time import time
import math
import numpy as np

# Narzedzia programu
def toStr(frm, lst):
    return "["+(", ".join(frm % arg for arg in lst))+"]"
compType = torch.float32 if torch.cuda.is_available() else torch.float64
compDevice = torch.device('cuda' if torch.cuda.is_available() else 'cpu')
print("\nComputation type:", compType)
print("\nComputation device:", compDevice)

# Podane wartosci dla zakresu obliczen, warunku poczatkowego i wspolczynnika w rownaniu
xBegin = 0.0
xEnd = 20.0
xInit = 0.0
yInit = 50.0
kConst = 0.25

# Wartosci pozostalych hiperparametrow programu
numArgs = 1  # liczba argumentow w rownaniu
numVals = 1  # liczba wartosci rozwiazania 
numPoints = 100  # liczba punktow dziedziny
netDepths = [128, 128]  # liczby neuronow w warstwach ukrytych
learnRate = 0.01  # tempo uczenia sieci
learnSpan = 1000  # zasieg petli uczenia

#
# Procedura obliczania pochodnej z podanego wyjscia sieci po podanym wejsciu
#

def differentiation(X, inp, Y, out):
    Y_out = torch.zeros_like(Y).to(compDevice)
    Y_out[:,out] = 1
    (grad_Y_X,) = autograd.grad(Y, (X,), grad_outputs=Y_out, create_graph=True)
    dY_dX = grad_Y_X[:,inp:inp+1]
    return dY_dX

#
# Definicja sieci neuronowej PINN sterowanej przykladowym 
# rownaniem rozniczkowym I rzedu rozladowania kondensatora
#

class Equation(nn.Module):
    def __init__(self, xo, yo, kc, d, n, m):
        super(Equation, self).__init__()
        
        self.inix = xo  # poczatkowa wartosc argumentu rozwiazania - czas poczatkowy to
        self.iniy = yo  # poczatkowa wartosc szukanej funkcji - poczatkowe natezenie pradu Io
        self.conk = kc  # staly wspolczynnik w rownaniu - odwrotnosc stalej czasowej tau = RC

        self.losf = nn.MSELoss(reduction ='mean')  # funkcja straty "mean squared error" w problemie regersji
        self.hids = d  # rozmiary warstw ukrytych (glebie sieci)
        self.inps = n  # rozmiar portu wejsciowego sieci
        self.outs = m  # rozmiar portu wyjsciowego sieci

        self.chain = nn.Sequential(  # lancuchowa siec z warstwami nueronu liniowego i nieliniowa funkcja aktywacji - zainicjowana niejawnie zerami lub losami
            nn.Linear(self.inps, self.hids[0], dtype=compType),
            nn.Tanh(),
            nn.Linear(self.hids[0], self.hids[1], dtype=compType),
            nn.Tanh(),
            nn.Linear(self.hids[1], self.hids[1], dtype=compType),
            nn.Tanh(),
            nn.Linear(self.hids[1], self.hids[0], dtype=compType),
            nn.Tanh(),
            nn.Linear(self.hids[0], self.outs, dtype=compType)
        ).to(compDevice)

    def prediction(self, X):  # wartosci rozwiazania rownania przewidywanego przez siec dla podanego argumentu
        Y = (X - self.inix) * self.chain(X) + self.iniy  # konstrukcyjne przylozenie warunku poczatkowego
        return Y
    
    def regression(self, aX):  # strata w regresji dla rownania na rozladowanie kondensatora w obwodzie elektrycznym RC
        X = aX.clone()  # postepowa propagacja wejscia na wyjscie w sieci
        Y = self.forward(X)

        dY_dX = differentiation(X, 0, Y, 0)  # wstecznie liczona pierwsza i druga pochodna z j-tego wyjscia sieci po i-tym wejscie sieci
        
        left = dY_dX  # lewa strona rownania
        right = -self.conk * Y  # prawa strona rownania
        L = self.losf(left, right)  # funkcja straty sterowana rownaniem w uczeniu nienadzorowanym

        return L

    def solution(self, X):  # referencyjne rozwiazanie rownania otrzymane numerycznie lub metoda calkowania symbolicznego
        Y = self.iniy * torch.exp(-self.conk * (X - self.inix))
        return Y
    
    def forward(self, X):  # pokrycie metody propagacji postepowej z modulu sieciowego nn.Module do wywolania na zewnatrz poprzez operator()
        return self.prediction(X)

#
# Inicjowanie srodowiska obliczeniowego
#

# Utworzenie danych punktow obliczeniowych do rozwiazywania rownania
xDatTensor = torch.linspace(xBegin, xEnd, numPoints*numArgs, dtype=compType, requires_grad=True).reshape(numPoints,numArgs).to(compDevice)
xDatTable = xDatTensor.detach().cpu().numpy()
print("\nData arguments =", xDatTable[:min(10,numPoints)])

# Utworzenie modelu solwera rownania i jego optymalizatora
model = Equation(xInit, yInit, kConst, netDepths, numArgs, numVals)
model.to(compDevice)
optimizer = optim.Adam(model.parameters(), lr=learnRate, amsgrad=False)
print("\nSolver model:", model)
print("\nModel optimizer:", optimizer)

# Wyznaczenie rozwiazania symbolicznego lub numerycznego (referencyjnego)
ySolTensor = model.solution(xDatTensor)
ySolTable = ySolTensor.detach().cpu().numpy()
print("\nSolution values =", ySolTable[:min(10,numPoints)])

# Przewidywanie rozwiazania przez model przed uczeniem
yPredTensor = model.prediction(xDatTensor)
yPredTable = yPredTensor.detach().cpu().numpy()
print("\nSolution predictions =", yPredTable[:min(10,numPoints)])

# Poczatkowy wykres wynikow obliczen
plt.figure(figsize=(8, 6))
plt.plot(xDatTable, ySolTable, 'r-',  label="solution")
plt.plot(xDatTable, yPredTable, 'b--', label="prediction")
plt.xlabel("x")
plt.ylabel("y")
plt.legend()
plt.title("Before training")
plt.show()

#
# Uczenie sieci neuronowej modelujacej rownanie w problemie regresji
#

print("\nTraining epochs:", learnSpan);
epochs = []
losses = []
model.train()
trainStart = time()
for epoch in range(learnSpan+1):
    loss = model.regression(xDatTensor)
    optimizer.zero_grad()
    loss.backward()
    optimizer.step()
    aloss = loss.item()
    if epoch%(learnSpan/100)==0:
        epochs.append(epoch)
        losses.append(aloss)
    if epoch%(learnSpan/10)==0:
        print("epoch:", epoch, "/", learnSpan, ", loss: %.8f" % aloss)
trainStop = time()
model.eval()
trainTime = (trainStop-trainStart)/60.0
print("\nTraining time: %.2f minutes" % trainTime)

plt.figure(figsize=(8, 6))
plt.plot(epochs, losses, 'g-')
plt.xlabel("epochs")
plt.ylabel("loss")
plt.title("Training loss")
plt.show()

#
# Ewaluacja nauczonej sieci
#

# Przewidywanie rozwiazania przez model po uczeniu
yPredTensor = model.prediction(xDatTensor)
yPredTable = yPredTensor.detach().cpu().numpy()
print("\nSolution predictions =", yPredTable[:min(10,numPoints)])

# Obliczenie miar bledu/dokladnosci dopasowania przewidywan do rozwiazan
yMSEValue = 0.5*np.mean((yPredTable - ySolTable)**2)  # blad sredniokwadratowy (mean squared error)
print("\nPrediction MSE value: %.10f" % yMSEValue)
yRMSEValue = math.sqrt(yMSEValue)  # pierwiastek z bledu sredniokwadratowego (root-mean-square error)
print("\nPrediction RMSE value: %.7f" % yRMSEValue)
yMEValue = np.max(np.abs(yPredTable - ySolTable))  # maksymalny blad bezwgledny (maximal error)
print("\nPrediction ME value: %.5f" % yMEValue)
yMAEValue = np.mean(np.abs(yPredTable - ySolTable))  # sredni blad bezwgledny (mean absolute error)
print("\nPrediction MAE value: %.5f" % yMAEValue)
yMREValue = np.mean(np.abs(yPredTable - ySolTable)/np.abs(ySolTable))  # sredni blad wzgledny (mean relative error) 
print("\nPrediction MRE value: %.5f" % yMREValue)
yAccuracy = np.clip(1.0 - yMREValue, 0.0, 1.0)*100.0  # procentowa miara dokladnosci
print("\nPrediction accuracy: %.5f" % yAccuracy, "%")

# Koncowy wykres wynikow obliczen
plt.figure(figsize=(8, 6))
plt.plot(xDatTable, ySolTable, 'r-',  label="solution")
plt.plot(xDatTable, yPredTable, 'b--', label="prediction")
plt.xlabel("x")
plt.ylabel("y")
plt.legend()
plt.title("After training")
plt.show()

