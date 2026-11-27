using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Navigation;

namespace Nitrogen.Views.Menu.Pressure;

/// <summary>
/// Создание и подключение экрана давления.
/// </summary>
internal sealed class PressureRegistration : IScreenRegistration
{
    public bool Open(ScreenContext context)
    {
        if (context.MainViewModel is not { } mainVm)
            return false;

        if (context.Client is not { } client)
            return false;

        var vm = new PressureViewModel(mainVm, client);

        var screen = new PressureControl
        {
            DataContext = vm
        };

        screen.CloseRequested += (_, _) =>
            context.BackScreen();

        context.ShowScreen(screen, vm);

        return true;
    }
}