using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Navigation;

namespace Nitrogen.Views.Settings.SCFSet;

/// <summary>
/// Создание и подключение экрана калибровки расхода азота.
/// </summary>
internal sealed class ScfSetRegistration : IScreenRegistration
{
    public bool Open(ScreenContext context)
    {
        if (context.MainViewModel is not { } mainVm)
            return false;

        var vm = new SCFSetControlViewModel(mainVm);

        var screen = new SCFSetControl
        {
            DataContext = vm
        };

        screen.CloseRequested += (_, _) =>
            context.BackScreen();

        context.ShowScreen(screen, vm);

        return true;
    }
}