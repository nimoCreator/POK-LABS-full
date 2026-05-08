namespace POK_5;

internal sealed class Gauss1D : Approximator1D
{
    public Gauss1D(int nodeCount)
        : base(nodeCount)
    {
    }

    public override double Kernel(double distance, double support)
    {
        return Math.Exp(-(distance * distance) / (support * support))
            / (support * Math.Sqrt(Math.PI));
    }

    public override double KernelDerivative(double distance, double support)
    {
        return -2.0 * Math.Exp(-(distance * distance) / (support * support))
            / (support * support * support * Math.Sqrt(Math.PI));
    }

    public override double KernelSecondDerivative(double distance, double support)
    {
        return -4.0 * (0.5 - (distance * distance) / (support * support))
            * Math.Exp(-(distance * distance) / (support * support))
            / (support * support * support * Math.Sqrt(Math.PI));
    }
}
