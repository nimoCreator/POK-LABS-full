namespace POK_5.assets;

internal static class NumericAssert
{
    public static void AreAlmostEqual(
        double actual,
        double expected,
        double tolerance,
        string message)
    {
        double allowedError = tolerance * Math.Max(1.0, Math.Abs(expected));
        double error = Math.Abs(actual - expected);

        if (error > allowedError)
        {
            throw new InvalidOperationException(
                $"{message} Blad = {error:e6}, dopuszczalny blad = {allowedError:e6}.");
        }
    }
}
