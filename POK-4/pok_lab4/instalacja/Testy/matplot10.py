import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

def func(t, damp = 1., freq = 1., phase = 0.):
    """
    The solution of a second order linear ordinary differential equation:

    func(t) = exp(-t * damp) * cos(2 * pi * f * t + phase)

    Inputs:
    * damp: dampening coefficient.
    * freq: frequency
    * phase: the phase of the signal

    Ouput: data as a DataFrame for easier post processing.
    """
    return pd.DataFrame( {"a": np.exp(-t * damp) * np.cos(2. * np.pi * freq * t + phase),
                          "t": t})

t = np.linspace(0., 5., 1001)
data = func(t, damp = .1, freq = 1.)
data.head()
plt.figure()
for f in [1., .5, .25]:
    data = func(t, damp = .2, freq = f)
    plt.plot(data.t, data.a, label = "$f={0}$".format(f))
data = func(t, damp = .2, freq = 0)
plt.plot(data.t, data.a, "k--", label = "Enveloppe")
plt.plot(data.t, -data.a, "k--")
plt.grid()
plt.legend(loc = "best")
plt.xlabel("Time, $t$")
plt.ylabel("Amplitude, $a$")
plt.title(r"A damped harmonic oscillator: $a(t) = \exp(-t d) \cos (2 \pi f t + \phi)$")
plt.show()