using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Navigation;

namespace Nitrogen.Views.Settings.DsPumpSet;

/// <summary>
/// Создание и подключение экрана настройки насоса ВС.
/// </summary>
internal sealed class DsPumpSetRegistration : IScreenRegistration
{
    public bool Open(ScreenContext context)
    {
        var screen = new DsPumpSetControl();

        screen.CloseRequested += (_, _) =>
            context.BackScreen();

        context.ShowScreen(screen);

        return true;
    }
}