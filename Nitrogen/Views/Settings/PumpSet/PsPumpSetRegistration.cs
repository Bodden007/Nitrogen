using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Navigation;

namespace Nitrogen.Views.Settings.PumpSet;

/// <summary>
/// Создание и подключение экрана настройки насоса ПС.
/// </summary>
internal sealed class PsPumpSetRegistration : IScreenRegistration
{
    public bool Open(ScreenContext context)
    {
        var screen = new PsPumpSetControl();

        screen.CloseRequested += (_, _) =>
            context.BackScreen();

        context.ShowScreen(screen);

        return true;
    }
}