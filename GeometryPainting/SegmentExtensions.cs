using System.Collections.Generic;
using Avalonia.Media;
using GeometryTasks;

namespace GeometryPainting;

public static class SegmentExtensions
{
    // Мне не очень нравится этот способ из-за расхода памяти,
    // но я не придумал как иначе, разве что наследоваться от Segment, но это мы еще не проходили)
    private static readonly Dictionary<Segment, Color> SegmentColors = new();

    public static void SetColor(this Segment segment, Color color)
    {
        SegmentColors[segment] = color;
    }

    public static Color GetColor(this Segment segment)
    {
        if (SegmentColors.TryGetValue(segment, out var color))
        {
            return color;
        }
        return Colors.Black;
    }
}