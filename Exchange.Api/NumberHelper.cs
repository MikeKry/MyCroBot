using System.Globalization;

public static class NumberHelper
{
    public static decimal ParseDecimal(string input, decimal defaultValue = 0)
    {
        return decimal.TryParse(input, CultureInfo.InvariantCulture, out decimal val) ? val : 0;
    }

    public static decimal GwtPercentDifference(decimal prev, decimal current)
    {
        return Math.Round(100 / prev * current - 100, 3);
    }
}