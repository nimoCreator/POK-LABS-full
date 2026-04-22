"""************************************************************************"""
"""* Zastosowanie modulu PyTorch do lamania szyfrow tekstowych przez siec *"""
"""* neuronowa jako przyklad przetwarzania ogolnego problemu klasyfikacji *"""
"""************************************************************************"""

import io, random, string
from time import time
import torch
from torch import nn, optim
from torch.utils import data as utild
from torch.nn import functional as nfunc

#
# Funkcje do konwersji pomiedzy tekstem a danymi 
# liczbowymi bedacymi indeksami znakow w tekscie 
# nad zbiorem znakowym (alfabetem) lub tensorami
# rozkladu prawdopodobienstwa znakow nad zbiorem
#

def toData(text, chars):
    data = [chars.index(c) for c in text]
    return data

def fromData(data, chars):
    text = str().join(chars[i] for i in data)
    return text

def toProb(data, size):
    prob = nfunc.one_hot(torch.LongTensor(data),size).type(torch.FloatTensor)
    return prob

def fromProb(prob, size):
    data = prob.reshape([-1,size]).argmax(dim = 1).tolist()
    return data

#
# Zbior i alfabety znakow w przetwarzanych tekstach
# dozwolonych dla danych zrodlowych i docelowych
#

alphChars = list(string.ascii_uppercase)
alphCount = len(alphChars)

srcAlphabet = alphChars.copy()
srcAlphabet.append(" ")
srcSize = len(srcAlphabet)

dstAlphabet = alphChars.copy()
dstAlphabet.append("~")
dstSize = len(dstAlphabet)

print("\n"+"basic alphabet characters:", alphChars)
print("number of characters =", alphCount)

#
# Konwertery dla portow wejsciowego i wyjsciowego
# sieci do konwersji pomiędzy tekstem lub blokiem 
# tekstowym a sieciowym wsadem tensorow rozkladu 
#

class NetConverter:
    def __init__(self, chars, size):
        self.chars = chars
        self.size = size
    def __len__(self):
        return self.size
    def __getitem__(self, index):
        return self.chars[index]

class NetEncoder(NetConverter):
    def __init__(self, chars, size):
        super().__init__(chars, size)
    def fromText(self, text):
        data = toData(text, self.chars)
        prob = toProb(data, self.size)
        return prob
    def fromBlock(self, block):
        probs = []
        for text in block:
            data = toData(text, self.chars)
            prob = toProb(data, self.size)
            probs.append(prob)
        batch = torch.stack(probs)
        return batch
    def toBatch(self, argum):
        if isinstance(argum, list):
            batch = self.fromBlock(argum)
        else:
            prob = self.fromText(argum)
            batch = torch.stack([prob])
        return batch
    def __call__(self, argum):
        return self.toBatch(argum)

class NetDecoder(NetConverter):
    def __init__(self, chars, size):
        super().__init__(chars, size)
    def toText(self, prob):
        data = fromProb(prob, self.size)
        text = fromData(data, self.chars)
        return text
    def toBlock(self, batch):
        block = []
        for prob in batch:
            data = fromProb(prob, self.size)
            text = fromData(data, self.chars)
            block.append(text)
        return block
    def fromBatch(self, batch):
        if batch.size(dim = 0) == 1:
            prob = batch.flatten(0,1)
            result = self.toText(prob)
        else:
            result = self.toBlock(batch)
        return result
    def __call__(self, batch):
        return self.fromBatch(batch)

inEncoder = NetEncoder(srcAlphabet,srcSize)
outDecoder = NetDecoder(dstAlphabet,dstSize)

#
# Odczytanie pliku korpusu kryptograficznego zawierajacego 
# pary rozwojowych tekstow postaci (tekst-szyfr,tekst-jawny), 
# ktore moga byc przeznaczone do trenowania lub testowania
# zastosowanej sieci, a ktore zostaly utworzone z uzyciem 
# dowolnej metody szyfrowania danych, np. szyfrem Cezara
#

def readTexts(fname):
    texts = []
    with io.open(fname) as file:
        for line in file:
            sep = line.index("\t")
            end = line.index("\n")
            cph = line[:sep].strip()
            pln = line[sep+1:end].strip()
            if (all(c in srcAlphabet for c in cph) and all(c in dstAlphabet for c in pln)):
                text = cph, pln
                texts.append(text)
    return texts
    
trainFile = "train.txt"
trainTexts = readTexts(trainFile)
trainCount = len(trainTexts)

print("\n"+"training file name:", trainFile)
print("number of file records =", trainCount)
print("\n"+"train-texts, first:", trainTexts[0])
print("              last:", trainTexts[-1])
    
#
# Konwersja par tekstow zaszyfrowanego oraz jawnego
# na zrodlowe i docelowe dane przetwarzane w procesie 
# uczenia zastosowanej sieci o portach Input i Output
#

inSize = 0
outSize = 0
trainData = []
for text in trainTexts:
    cph, pln = text
    src = toData(cph,srcAlphabet)
    dst = toData(pln,dstAlphabet)
    inSize = max(inSize,len(src))
    outSize = max(outSize,len(dst))
    data = src, dst
    trainData.append(data)

print("\n"+"train-data, first:", trainData[0])
print("             last:", trainData[-1])

#
# Utworzenie koncowego zbioru danych do uczenia sieci
# zawierajacego dane przedstawione w formie tensorow
# podawanych na porty Origin i Target sieci uczącej
#

class TensorSet(utild.Dataset):
    def __init__(self, data, count, ssize = srcSize, dsize = dstSize):
        super().__init__()
        self.data = data
        self.count = count
        self.ssize = ssize
        self.dsize = dsize
    def __len__(self):
        return self.count
    def __getitem__(self, index):
        src, dst = self.data[index]
        return toProb(src,self.ssize), torch.LongTensor(dst)        
trainTensors = TensorSet(trainData,trainCount)

print("\n"+"first train-tensors:"+"\n", trainTensors[0])
print("\n"+"last train-tensors:"+"\n", trainTensors[-1])

#
# Podzial zbioru tensorowych danych na osobne wsady
# ladowane do sieci uczacej i przetwarzane w procesie 
# iteracyjnego uczenia z mozliwoscia zrownoleglania
#

batchSize = 32

trainBatches = utild.DataLoader(trainTensors, batch_size = batchSize, shuffle = True)
numBatches = len(trainBatches)
batchCount = numBatches*batchSize

print("\n"+"training batch size =", batchSize)
print("number of batches =", numBatches)
print("number of batch records =", batchCount)
print("\n"+"first train-batch:"+"\n", next(iter(trainBatches)))

#
# Definicja architektury sieci RNN w postaci lanucha warstw 
# z uzyciem warstw rekurencyjnych oraz warstw pomocniczych
#

class ReplicateLastRecTensor(nn.Module):
    """
    Warstwa pomocnicza do replikacji ostatniego tensora 
    (wektora) w kazdej sekwencji tensorow z przetwarzanego 
    wsadu otrzymanych z poprzedniej warstwy rekurencyjnej
    """
    def __init__(self, num, mult):
        super().__init__()
        self.num = num
        self.mult = mult
    def forward(self, x):
        y, _ = x
        n = min(max(self.num,1),y.size(1))
        m = max(self.mult,1)
        z = y[:, n-1, :]
        u = z.unsqueeze(dim = 1).repeat(1, m, 1)
        return u

class ReplicateRestRecTensor(nn.Module):
    """
    Warstwa pomocnicza do replikacji pozostalych tensorow 
    (wektorow) w kazdej sekwencji tensorow z przetwarzanego 
    wsadu otrzymanych z poprzedniej warstwy rekurencyjnej
    """
    def __init__(self, num, mult):
        super().__init__()
        self.num = num
        self.mult = mult
    def forward(self, x):
        y, _ = x
        n = min(max(self.num,1),y.size(1))
        m = max(self.mult,1)
        z = y[:, n-m:n, :] if n>m else y
        u = nfunc.pad(z, (0, 0, 0, m-n), "replicate") if m>n else z
        return u

class ExtractAllRecTensors(nn.Module):
    """
    Warstwa pomocnicza do wydobycia wszystkich tensorow
    (wektorow) w kazdej sekwencji tensorow z przetwarzanego 
    wsadu otrzymanych z poprzedniej warstwy rekurencyjnej
    """
    def __init__(self, num):
        super().__init__()
        self.num = num
    def forward(self, x):
        y, _ = x
        n = min(max(self.num,1),y.size(1))
        z = y[:, :n, :]
        return z

class ApplyLinearList(nn.Module):
    """
    Warstwa pomocnicza do zastosowania listy liniowych
    neuronow dla kolejnych tensorow (wektorow) w kazdej 
    sekwencji tensorow z przetwarzanego wsadu
    """
    def __init__(self, count, inum, onum, bias=True, device=None, dtype=None):
        super().__init__()
        self.count = count
        self.lays = nn.ModuleList(nn.Linear(inum, onum, bias, device, dtype) for i in range(count if count>0 else 1))
    def forward(self, x):
        u = []
        m = max(self.count,0)
        n = min(m,x.size(1))
        for i, lay in enumerate(self.lays):
            if i>=n: break
            y = x[:, i, :]
            z = lay(y)
            u.append(z)
        n = x.size(1)
        lay = self.lays[-1]
        for j in range(n-m):
            y = x[:, j, :]
            z = lay(y)
            u.append(z)
        v = torch.stack(u, dim = 1)
        return v
    
hidSize = 64

rnnChain = nn.Sequential(  # uwaga: siec na wejsciu oczekuje p-wsadu n-sekwencji zawierajacych tensory w postaci r-wektorow one-hot, ktore zamienia na wewnetrzne h-wektory, gdzie na przyklad p = 32, n = 8, r = 27, h = 64
    nn.LSTM(srcSize, hidSize, num_layers = 1, batch_first = True),
    ReplicateRestRecTensor(inSize, outSize),
    nn.LSTM(hidSize, hidSize, num_layers = 1, batch_first = True),
    ExtractAllRecTensors(outSize),
    # nn.Linear(hidSize, dstSize)  # zamiast pojedynczego neuronu stosujemy m-liste neuronow, co zwieksza liczbe parametrow do nauczenia, ale jednoczesnie zwieksza zbieznosc procesu uczenia
    ApplyLinearList(outSize, hidSize, dstSize)
    #, nn.Softmax(dim = 2)  # jawne obliczanie prawdopodobiedstw, gdy warstwa straty takich obliczen nie wykonuje, uwaga: warstwa CrossEntropyLoss wykonuje obliczanie prawdopodobienstw
)  # uwaga: siec na wyjsciu zwraca p-wsad m-sekwencji zawierajacych s-rozklady prawdopodobienstwa, gdzie na przyklad p = 32, m = 8, s = 27 

print("\n"+"recurrent network chain module:", rnnChain)
print("input port size =", inSize)
print("source alphabet size =", srcSize)
print("hidden recurrent size =", hidSize)
print("destination alphabet size =", dstSize)
print("output port size =", outSize)

#
# Proces uczenia sieci na danych treningowych 
# w problemie klasyfikacji z uzyciem warstwy 
# liczenia straty uczenia z entropii krosowej 
# oraz z uzyciem algorytmu optymalizacji ADAM
#

trainSpan = 999
learnRate = 0.01
learnCost = 0.001

learnSize = 0
learnParams = rnnChain.parameters()
for param in learnParams:
    learnSize += param.numel()
learnParams = rnnChain.parameters()

learnOptimizer = optim.Adam(learnParams, learnRate, betas = (0.9, 0.999), eps = 0.00001, weight_decay = 0, amsgrad = False)
#learnCriterion = nn.CrossEntropyLoss(reduction = 'mean')  # przecietna redukcja strat z entropii w calym p-wsadzie
learnCriterion = nn.CrossEntropyLoss(reduction = 'sum')  # ...lub redukcja laczna osobna dla m-sekwencji w p-wsadzie

print("\n"+"number of training epochs =", trainSpan)
print("number of learning parameters =", learnSize)
print("initial learning rate =", learnRate)
print("maximal learning cost =", learnCost)

print("\n"+"training iteration..."+"\n")
rnnChain.train()

trainStart = time()
for epoch in range(trainSpan):  # glowna petla iteracyjna procesu uczenia sieci na danych treningowych
    cost = 0.0
    for batch in trainBatches:  # petla po wsadach z tensorami danych, sekwencyjna lub zrownoleglona
        origin, target = batch
        learnOptimizer.zero_grad()
        logits = rnnChain(origin)
        #loss = learnCriterion(logits.flatten(0,1), target.flatten(0,1))  # przecietna redukcja strat z entropii w calym p-wsadzie
        losses = []  # ...lub redukcja laczna osobna dla m-sekwencji w p-wsadzie, po ktorej zachodzi redukcja przecietna w p-wsadzie
        for lgt, trg in zip(logits,target):
            lss = learnCriterion(lgt,trg)
            losses.append(lss)
        loss = torch.stack(losses).mean()
        loss.backward()
        learnOptimizer.step()
        cost += loss.item()
    else:
        cost = cost/numBatches  # koncowy koszt optymalizacji w procesie uczenia jako przecietna strata entropii w zbiorze wszystkich wsadow
        print("... epoch {0:3d}, learning cost = {1:.5f}".format(epoch, cost))
    if cost<learnCost:
        break
trainStop = time()

trainTime = (trainStop-trainStart)/60.0
print("\n"+"training time: {0:.2f} minutes".format(trainTime))

print("\n"+"network evaluation...")
rnnChain.eval()

#
# Utworzenie z nauczonej sieci modelu sieciowego 
# do przewidwania tekstu jawnego na wyjsciu sieci
# dla podanego na wejsciu tekstu zaszyfrowanego
#

class NetWithProb(nn.Module):
    """
    Siec pomocnicza dolaczajaca do nauczonej sieci 
    jawna warstwe liczaca rozklad prawdopodobienstwa
    na porcie wyjsciowym dla danych docelowych, gdy
    wybrana warstwa straty wykonuje takie obliczenia
    """
    def __init__(self, core):
        super().__init__()
        self.core = core
        self.prob = nn.Softmax(dim = 2)
    def forward(self, x):
        y = self.core(x)
        z = self.prob(y)
        return z
netModel = NetWithProb(rnnChain)
    
def predictPlains(ciphers):
    inbatch = inEncoder(ciphers)
    outbatch = netModel(inbatch)
    plains = outDecoder(outbatch)
    return plains

#
# Walidacja otrzymanego modelu na podstawie przykladow 
# tekstowych z pliku treningowego oraz weryfikacja modelu
# dla innych (losowych) przykladow podanych w skrypcie 
# lub odczytanych z pliku testowego ale takich ktore 
# nie byly uzywane jako treningowe w uczeniu sieci
#

numTrains = min(10,trainCount)
trainSamples = [trainTexts[i] for i in random.sample(range(trainCount), numTrains)]

trainCiphers = []
trainPlains = []
for sample in trainSamples:
    cph, pln = sample
    trainCiphers.append(cph)
    trainPlains.append(pln)
trainPredicts = predictPlains(trainCiphers)

print("\n"+"number of training samples =", numTrains)
print("\n"+"train-ciphers:", trainCiphers)
print("   and plains:", trainPlains)
print("  predictions:", trainPredicts)

if alphCount>=inSize:
    anyCipher = str().join(alphChars[i] for i in random.sample(range(alphCount), inSize))
    anyPlain = str().join("?" for i in random.sample(range(alphCount), outSize))
    anyPredict = predictPlains(anyCipher)
    print("\n"+"any cipher:", anyCipher)
    print(" and plain:", anyPlain)
    print("prediction:", anyPredict)

testFile = "test.txt"
testTexts = readTexts(testFile)
testCount = len(testTexts)

print("\n"+"testing file name:", testFile)
print("number of file records =", testCount)
print("\n"+"test-texts, first:", testTexts[0])
print("             last:", testTexts[-1])
 
numTests = min(10000,testCount)
testSamples = [testTexts[i] for i in random.sample(range(testCount), numTests)]

testCiphers = []
testPlains = []
for sample in testSamples:
    cph, pln = sample
    testCiphers.append(cph)
    testPlains.append(pln)
testPredicts = predictPlains(testCiphers)

errCount = 0
for predc, plain in zip(testPredicts,testPlains):
    if predc!=plain:
        errCount = errCount+1
testError = errCount*100.0/numTests

print("\n"+"number of testing samples =", numTests)
print("false prediction count =", errCount)
print("relative testing error =", testError, "%")
