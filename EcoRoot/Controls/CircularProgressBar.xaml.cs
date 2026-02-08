using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace EcoRoot.Controls;

public partial class CircularProgressBar : ContentView
{
    private const double RingDiameter = 100;
    private const double RingRadius = RingDiameter / 2;

    public static readonly BindableProperty ProgressRatioProperty = BindableProperty.Create(
        nameof(ProgressRatio),
        typeof(double),
        typeof(CircularProgressBar),
        0.0,
        propertyChanged: OnProgressRatioChanged);

    public double ProgressRatio
    {
        get => (double)GetValue(ProgressRatioProperty);
        set => SetValue(ProgressRatioProperty, value);
    }

    public CircularProgressBar()
    {
        InitializeComponent();
        BindingContext = this;
        UpdateProgressRing();
    }

    private static void OnProgressRatioChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CircularProgressBar control)
        {
            control.UpdateProgressRing();
        }
    }

    private void UpdateProgressRing()
    {
        if (ProgressRing is null)
        {
            return;
        }

        var progress = Math.Clamp(ProgressRatio, 0.0, 1.0);
        var circumference = 2 * Math.PI * RingRadius;
        var dashLength = Math.Max(0.01, progress * circumference);
        var gapLength = Math.Max(0.01, circumference - dashLength);

        ProgressRing.StrokeDashArray = new DoubleCollection { dashLength, gapLength };
    }
}
