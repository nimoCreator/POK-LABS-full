namespace POK_5;

internal static class Monaghan1D
{
    public static double Value(double distance, double support)
    {
        double q = Math.Abs(distance) / support;
        if (q >= 2.0)
        {
            return 0.0;
        }

        if (q < 1.0)
        {
            return (2.0 / 3.0 - q * q + 0.5 * q * q * q) / support;
        }

        double tail = 2.0 - q;
        return tail * tail * tail / (6.0 * support);
    }

    public static double FirstDerivative(double distance, double support)
    {
        double r = Math.Abs(distance);
        double q = r / support;
        if (q >= 2.0)
        {
            return 0.0;
        }

        double derivativeShape = q < 1.0
            ? -2.0 * q + 1.5 * q * q
            : -0.5 * Math.Pow(2.0 - q, 2.0);

        return derivativeShape / (support * support);
    }

    public static double DerivativeOverRadius(double distance, double support)
    {
        double r = Math.Abs(distance);
        if (r <= 1.0e-12)
        {
            return 0.0;
        }

        return FirstDerivative(r, support) / r;
    }

    public static double Laplacian(double distance, double support)
    {
        double q = Math.Abs(distance) / support;
        if (q >= 2.0)
        {
            return 0.0;
        }

        double secondShape = q < 1.0
            ? -2.0 + 3.0 * q
            : 2.0 - q;

        return secondShape / (support * support * support);
    }
}
