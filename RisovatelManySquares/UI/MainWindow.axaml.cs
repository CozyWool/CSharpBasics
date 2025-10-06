using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using RefactorMe.Common;

namespace RefactorMe.UI;

public partial class MainWindow : Window
{
    private int time;
    private Canvas canvas;
    private readonly DispatcherTimer timer;
    private readonly CanvasGraphics canvasGraphics;

    public MainWindow()
    {
        InitializeComponent();

        canvasGraphics = new CanvasGraphics(canvas);
        Width = WindowSettings.Width;
        Height = WindowSettings.Height;

        timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(250)
                };
        timer.Tick += (_, _) => TimerTick();

        canvas.Tapped += (_, _) =>
                         {
                             if (timer.IsEnabled)
                                 timer.Stop();
                             else
                                 timer.Start();
                         };

        Opened += (_, __) => Draw();

        CanResize = false;
    }

    private void TimerTick()
    {
        time++;
        Draw();
    }

    private void Draw()
    {
        var random = new Random();
        var angularVelocity =  Math.PI / random.Next(2, 8);
        var angle = angularVelocity * (time * timer.Interval.Milliseconds / 1000d);
        var innerLineMultiplier = 0.375f;
        var outerLineMultiplier = 0.04f;
        ImpossibleSquare.Draw((int) Width / 2,
                              (int) Height / 2,
                              angle,
                              canvasGraphics,
                              innerLineMultiplier,
                              outerLineMultiplier);
        InvalidateVisual();
    }

    public void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        canvas = this.FindControl<Canvas>("Canvas");
    }
}