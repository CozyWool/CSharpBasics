namespace Recognizer;

public static class GrayscaleTask
{
    public static double[,] ToGrayscale(Pixel[,] original)
    {
        var row = original.GetLength(0);
        var col = original.GetLength(1);
        var grayscale = new double[row, col];

        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                grayscale[i, j] = (0.299 * original[i, j].R
                                   + 0.587 * original[i, j].G
                                   + 0.114 * original[i, j].B) / 255;
            }
        }

        return grayscale;
    }
}