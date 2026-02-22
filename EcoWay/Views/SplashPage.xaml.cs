namespace EcoWay.Views;

public partial class SplashPage : ContentPage
{
    private bool _hasNavigated;

    public SplashPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_hasNavigated)
        {
            return;
        }

        _hasNavigated = true;

        try
        {
            LogoOrb.Scale = 0.86;
            await LogoOrb.ScaleToAsync(1.0, 420, Easing.CubicOut);

            await Task.Delay(950);

            if (Shell.Current is null)
            {
                return;
            }

            await Shell.Current.GoToAsync("//MainMenuPage", false);
        }
        catch
        {
            // Avoid blocking startup if animation/navigation gets interrupted.
        }
    }
}
