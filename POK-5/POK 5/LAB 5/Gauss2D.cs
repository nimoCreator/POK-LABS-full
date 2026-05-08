namespace POK_5;

internal sealed class Gauss2D : Approximator2D
{
    public Gauss2D(int nodeCountX, int nodeCountY)
        : base(nodeCountX, nodeCountY)
    {
    }

    public override double Kernel(double radius, double support)
    {
        return Math.Exp(-(radius * radius) / (support * support))
            / (Math.PI * support * support);
    }
}
