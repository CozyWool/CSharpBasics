namespace Pluralize;

public static class PluralizeTask
{
    public static string PluralizeRubles(int count)
    {
        if (count % 100 is >= 5 and <= 20 || count % 10 >= 5 || count % 10 == 0)
        {
            return "рублей";
        }

        return count % 10 == 1 ? "рубль" : "рубля";
    }
}