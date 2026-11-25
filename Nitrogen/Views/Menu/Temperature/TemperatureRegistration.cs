using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Navigation;

namespace Nitrogen.Views.Menu.Temperature;

/// <summary>
/// Создание и подключение экрана температуры.
/// </summary>
internal sealed class TemperatureRegistration : IScreenRegistration
{
    public bool Open(ScreenContext context)
    {
        if (context.MainViewModel is not { } mainVm)
            return false;

        var screen = new TemperatureControl
        {
            DataContext = mainVm
        };

        screen.CloseRequested += (_, _) =>
            context.BackScreen();

        context.ShowScreen(screen);

        return true;
    }
}