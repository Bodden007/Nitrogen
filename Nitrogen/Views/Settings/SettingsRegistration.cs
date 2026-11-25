using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Navigation;
using Nitrogen.Views.Settings.PressureSet;
using Nitrogen.Views.Settings.SCFSet;
using Nitrogen.Views.Settings.DsPumpSet;
using Nitrogen.Views.Settings.PumpSet;

namespace Nitrogen.Views.Settings;

/// <summary>
/// Создание и подключение меню настроек.
/// </summary>
internal sealed class SettingsRegistration : IScreenRegistration
{
    public bool Open(ScreenContext context)
    {
        var screen = new SettingsControl();

        screen.PressureSetRequested += (_, _) =>
            context.Open<PressureSetRegistration>();

        screen.ScfSetRequested += (_, _) =>
            context.Open<ScfSetRegistration>();

        screen.DsPumpSetRequested += (_, _) =>
            context.Open<DsPumpSetRegistration>();

        screen.PsPumpSetRequested += (_, _) =>
            context.Open<PsPumpSetRegistration>();

        screen.CloseRequested += (_, _) =>
            context.BackScreen();

        context.ShowScreen(screen);

        return true;
    }
}