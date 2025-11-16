using ConsoleApp2;

long[] arr = [0, 1, 1, 1, 2, 4, 4, 5];
Console.WriteLine(FindLeftBorder(arr, 4));
Console.WriteLine(FindRightBorder(arr, 4));

static int FindLeftBorder(long[] arr, long value)
{
    return BinSearchLeftBorder(arr, value, -1, arr.Length);
}

static int BinSearchLeftBorder(long[] array, long value, int left, int right)
{
    if (right - left == 1)
    {
        return left;
    }

    var m = left + (right - left) / 2;
    if (array[m] < value)
    {
        return BinSearchLeftBorder(array, value, m, right);
    }

    return BinSearchLeftBorder(array, value, left, m);
}

static int FindRightBorder(long[] arr, long value)
{
    return BinSearchRightBorder(arr, value, -1, arr.Length);
}

static int BinSearchRightBorder(long[] array, long value, int left, int right)
{
    // if (right - left == 1)
    // {
    //     return right;
    // }
    //
    // var m = left + (right - left) / 2;
    // if (array[m] > value)
    // {
    //     return BinSearchRightBorder(array, value, left, m);
    // }
    //
    // return BinSearchRightBorder(array, value, m, right);
    while (right - left > 1)
    {
        var m = left + (right - left) / 2;
        if (array[m] > value)
        {
            right = m;
        }
        else
        {
            left = m;
        }
    }

    return right;
}

var result = GetRightBorderIndex(["a", "ab", "abc"], "", -1, 3);
Console.WriteLine(result);

static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
{
    while (right - left > 1)
    {
        var m = left + (right - left) / 2;
        if (string.Compare(prefix, phrases[m], StringComparison.InvariantCultureIgnoreCase) < 0 &&
            !phrases[m].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
        {
            right = m;
        }
        else
        {
            left = m;
        }
    }

    return right;
}