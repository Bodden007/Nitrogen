using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Navigation;

namespace Nitrogen.Views.Menu.SCFVolume;

/// <summary>
/// Создание и подключение экрана объема азота.
/// </summary>
internal sealed class ScfVolumeRegistration : IScreenRegistration
{
    public bool Open(ScreenContext context)
    {
        if (context.MainViewModel is not { } mainVm)
            return false;

        var vm = new ScfVolumeControlViewModel(mainVm);

        var screen = new ScfVolumeControl
        {
            DataContext = vm
        };

        screen.CloseRequested += (_, _) =>
            context.BackScreen();

        context.ShowScreen(screen);

        return true;
    }
}