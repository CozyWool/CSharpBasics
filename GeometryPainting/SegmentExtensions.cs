using System.Runtime.CompilerServices;
using Avalonia.Media;
using GeometryTasks;

namespace GeometryPainting;

public static class SegmentExtensions
{
    private static readonly ConditionalWeakTable<Segment, ColorWrapper> SegmentColors = new();

    public static void SetColor(this Segment segment, Color color)
    {
        SegmentColors.AddOrUpdate(segment, new ColorWrapper(color));
    }

    public static Color GetColor(this Segment segment)
    {
        return SegmentColors.TryGetValue(segment, out var colorWrapper)
                   ? colorWrapper.Color
                   : Colors.Black;
    }

    private class ColorWrapper(Color color)
    {
        public Color Color = color;
    }
}