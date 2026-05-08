#include <cmath>
#include <fstream>
#include <iomanip>
#include <iostream>
#include <string>

using real = long double;

real RCAnalityczne(real t)
{
    return std::exp(-t);
}

real RCEulerStep(real q, real delt)
{
    return q * (1.0L - delt);
}

real RCMidpointStep(real q, real delt)
{
    real k1 = -delt * q;
    real k2 = -delt * (q + 0.5L * k1);
    return q + k2;
}

real RCRungeKuttaStep(real q, real delt)
{
    real k1 = -delt * q;
    real k2 = -delt * (q + 0.5L * k1);
    real k3 = -delt * (q + 0.5L * k2);
    real k4 = -delt * (q + k3);
    return q + (k1 + 2.0L * k2 + 2.0L * k3 + k4) / 6.0L;
}

int main()
{
    real delt = 0.0L;
    real tk = 0.0L;
    std::string fileName;

    std::cout << "skok czasu: ";
    std::cin >> delt;
    std::cout << "czas koncowy: ";
    std::cin >> tk;
    std::cout << "podaj nazwe pliku z wynikami: ";
    std::cin >> fileName;

    std::ofstream file(fileName.c_str());
    if (!file)
    {
        std::cerr << "Nie udalo sie otworzyc pliku wyjsciowego.\n";
        return 1;
    }

    real qEuler = 1.0L;
    real qMidpoint = 1.0L;
    real qRungeKutta = 1.0L;

    std::cout << "\nRozwiazanie modelu ukladu RC:\n\n";
    std::cout << std::left << std::setprecision(6)
              << std::setw(12) << "czas"
              << std::setw(16) << "analit"
              << std::setw(16) << "euler"
              << std::setw(16) << "midpoint"
              << std::setw(16) << "rungekutta"
              << '\n';

    file << "czas;analit;euler;midpoint;rungekutta\n";
    file << std::setprecision(10);

    for (real t = 0.0L; t <= tk + 1e-12L; t += delt)
    {
        real qAnalytical = RCAnalityczne(t);

        std::cout << std::setw(12) << static_cast<double>(t)
                  << std::setw(16) << static_cast<double>(qAnalytical)
                  << std::setw(16) << static_cast<double>(qEuler)
                  << std::setw(16) << static_cast<double>(qMidpoint)
                  << std::setw(16) << static_cast<double>(qRungeKutta)
                  << '\n';

        file << static_cast<double>(t) << ';'
             << static_cast<double>(qAnalytical) << ';'
             << static_cast<double>(qEuler) << ';'
             << static_cast<double>(qMidpoint) << ';'
             << static_cast<double>(qRungeKutta) << '\n';

        qEuler = RCEulerStep(qEuler, delt);
        qMidpoint = RCMidpointStep(qMidpoint, delt);
        qRungeKutta = RCRungeKuttaStep(qRungeKutta, delt);
    }

    std::cout << '\n';
    return 0;
}
