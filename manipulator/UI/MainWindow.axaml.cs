using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;

namespace Manipulation.UI;

public partial class MainWindow : Window
{
    private readonly Frame frame;
    private double currentStep;
    private double maxStep = Math.PI / 2;

    public void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public MainWindow()
    {
        InitializeComponent();
        frame = this.FindNameScope()?.Find<Frame>("Frame") ??
                throw new InvalidOperationException("Frame wasn't created!");
        frame.GetSize = () => ClientSize;

        Background = new SolidColorBrush(new Color(255, 255, 230, 230));
        var timer = new DispatcherTimer();

        timer.Tick += (_, ev) =>
                      {
                          if (VisualizerTask.KeyDown(frame, timer.Tag as KeyEventArgs ?? null))
                          {
                              timer.Stop();
                          }
                      };
        KeyDown += (_, ev) =>
                   {
                       if (timer.IsEnabled)
                       {
                           return;
                       }
                       timer.Tag = ev;
                       VisualizerTask.CurrentStep = 0;
                       timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                       timer.Start();
                   };
        PointerMoved += (_, ev) => VisualizerTask.MouseMove(frame, ev);
        PointerWheelChanged += (_, ev) => VisualizerTask.MouseWheel(frame, ev);
    }
}