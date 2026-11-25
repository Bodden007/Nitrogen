using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Navigation;

namespace Nitrogen.Views.Menu.Engine;

/// <summary>
/// Создание и подключение экрана двигателя.
/// </summary>
internal sealed class EngineRegistration : IScreenRegistration
{
    public bool Open(ScreenContext context)
    {
        if (context.MainViewModel is not { } mainVm)
            return false;

        var vm = new EngineViewModel(mainVm);

        var screen = new EngineControl
        {
            DataContext = vm
        };

        screen.CloseRequested += (_, _) =>
            context.BackScreen();

        context.ShowScreen(screen, vm);

        return true;
    }
}