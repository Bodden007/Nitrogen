using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Navigation;

namespace Nitrogen.Views.Settings.PressureSet;

/// <summary>
/// Создание и подключение экрана калибровки давления.
/// </summary>
internal sealed class PressureSetRegistration : IScreenRegistration
{
    public bool Open(ScreenContext context)
    {
        if (context.MainViewModel is not { } mainVm)
            return false;

        var vm = new PressureSetControlViewModel(mainVm);

        var screen = new PressureSetControl
        {
            DataContext = vm
        };

        screen.CloseRequested += (_, _) =>
            context.BackScreen();

        context.ShowScreen(screen, vm);

        return true;
    }
}