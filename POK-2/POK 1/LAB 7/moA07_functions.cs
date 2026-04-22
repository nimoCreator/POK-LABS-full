namespace POK_1;

internal static class MoA07Functions
{
    public static double Zad1(double x)
    {
        double tan = Math.Tan(x);
        return tan * tan * tan - 4.0 * tan;
    }

    public static double Zad2(double x)
    {
        return Math.Exp(x - 1.0) + x * x - 2.0;
    }

    public static double Zad3(double x)
    {
        double log2 = Math.Log(2.0);
        double p = x * x - 5.0 * x + 10.0;
        return Math.Pow(2.0, p) * log2 * (2.0 * x - 5.0);
    }

    public static double Zad3Derivative(double x)
    {
        double log2 = Math.Log(2.0);
        double p = x * x - 5.0 * x + 10.0;
        double linear = 2.0 * x - 5.0;

        return Math.Pow(2.0, p) * log2 * (2.0 + log2 * linear * linear);
    }
}
