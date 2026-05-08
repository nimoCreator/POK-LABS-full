#include <cmath>
#include <fstream>
#include <iomanip>
#include <iostream>
#include <string>

using real = long double;

constexpr real Pi = 3.14159265358979323846L;
constexpr real Degrees = 180.0L / Pi;

struct PendulumState
{
    real fi;
    real om;
};

PendulumState Derivatives(const PendulumState& state, real g_div_l)
{
    return { state.om, -g_div_l * std::sin(state.fi) };
}

PendulumState EulerStep(const PendulumState& state, real delt, real g_div_l)
{
    PendulumState d = Derivatives(state, g_div_l);
    return { state.fi + delt * d.fi, state.om + delt * d.om };
}

PendulumState MidpointStep(const PendulumState& state, real delt, real g_div_l)
{
    PendulumState k1 = Derivatives(state, g_div_l);
    PendulumState middle
    {
        state.fi + 0.5L * delt * k1.fi,
        state.om + 0.5L * delt * k1.om
    };

    PendulumState k2 = Derivatives(middle, g_div_l);
    return { state.fi + delt * k2.fi, state.om + delt * k2.om };
}

PendulumState RungeKuttaStep(const PendulumState& state, real delt, real g_div_l)
{
    PendulumState k1 = Derivatives(state, g_div_l);

    PendulumState s2
    {
        state.fi + 0.5L * delt * k1.fi,
        state.om + 0.5L * delt * k1.om
    };
    PendulumState k2 = Derivatives(s2, g_div_l);

    PendulumState s3
    {
        state.fi + 0.5L * delt * k2.fi,
        state.om + 0.5L * delt * k2.om
    };
    PendulumState k3 = Derivatives(s3, g_div_l);

    PendulumState s4
    {
        state.fi + delt * k3.fi,
        state.om + delt * k3.om
    };
    PendulumState k4 = Derivatives(s4, g_div_l);

    return
    {
        state.fi + delt * (k1.fi + 2.0L * k2.fi + 2.0L * k3.fi + k4.fi) / 6.0L,
        state.om + delt * (k1.om + 2.0L * k2.om + 2.0L * k3.om + k4.om) / 6.0L
    };
}

real PendulumAnalytical(real t, real fi0, real sqrt_g_div_l)
{
    return fi0 * std::cos(t * sqrt_g_div_l);
}

int main()
{
    constexpr real g = 9.81L;

    real l = 0.0L;
    real deltPeriods = 0.0L;
    real tkPeriods = 0.0L;
    real fi0Degrees = 0.0L;
    std::string fileName;

    std::cout << "dlugosc wahadla w metrach: ";
    std::cin >> l;
    real g_div_l = g / l;
    real sqrt_g_div_l = std::sqrt(g_div_l);
    real T = 2.0L * Pi / sqrt_g_div_l;
    std::cout << "okres wahan dla malych wychylen wynosi: " << static_cast<double>(T) << " sekund\n";

    std::cout << "kat poczatkowy w stopniach: ";
    std::cin >> fi0Degrees;
    real fi0 = fi0Degrees / Degrees;

    std::cout << "skok czasu w okresach: ";
    std::cin >> deltPeriods;
    real delt = deltPeriods * T;

    std::cout << "czas koncowy w okresach: ";
    std::cin >> tkPeriods;
    real tk = tkPeriods * T;

    std::cout << "podaj nazwe pliku z wynikami: ";
    std::cin >> fileName;

    std::ofstream file(fileName.c_str());
    if (!file)
    {
        std::cerr << "Nie udalo sie otworzyc pliku wyjsciowego.\n";
        return 1;
    }

    PendulumState euler { fi0, 0.0L };
    PendulumState midpoint { fi0, 0.0L };
    PendulumState rungeKutta { fi0, 0.0L };

    std::cout << "\nRozwiazanie modelu wahadla w sekundach i stopniach:\n\n";
    std::cout << std::left << std::setprecision(6)
              << std::setw(12) << "czas"
              << std::setw(16) << "analit"
              << std::setw(16) << "euler"
              << std::setw(16) << "midpoint"
              << std::setw(16) << "rungekutta"
              << '\n';

    file << "czas;analit_deg;euler_deg;midpoint_deg;rungekutta_deg\n";
    file << std::setprecision(10);

    for (real t = 0.0L; t <= tk + 1e-12L; t += delt)
    {
        real analytical = PendulumAnalytical(t, fi0, sqrt_g_div_l);

        std::cout << std::setw(12) << static_cast<double>(t)
                  << std::setw(16) << static_cast<double>(analytical * Degrees)
                  << std::setw(16) << static_cast<double>(euler.fi * Degrees)
                  << std::setw(16) << static_cast<double>(midpoint.fi * Degrees)
                  << std::setw(16) << static_cast<double>(rungeKutta.fi * Degrees)
                  << '\n';

        file << static_cast<double>(t) << ';'
             << static_cast<double>(analytical * Degrees) << ';'
             << static_cast<double>(euler.fi * Degrees) << ';'
             << static_cast<double>(midpoint.fi * Degrees) << ';'
             << static_cast<double>(rungeKutta.fi * Degrees) << '\n';

        euler = EulerStep(euler, delt, g_div_l);
        midpoint = MidpointStep(midpoint, delt, g_div_l);
        rungeKutta = RungeKuttaStep(rungeKutta, delt, g_div_l);
    }

    std::cout << '\n';
    return 0;
}
