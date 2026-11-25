using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Navigation;
using Nitrogen.Views.Menu.Pressure;
using Nitrogen.Views.Menu.Engine;
using Nitrogen.Views.Menu.SCFVolume;
using Nitrogen.Views.Menu.Temperature;
using Nitrogen.Views.Menu.Volume;
using Nitrogen.Views.Settings;

namespace Nitrogen.Views.Menu;

/// <summary>
/// Создание и подключение основного меню.
/// </summary>
internal sealed class MenuRegistration : IScreenRegistration
{
    public bool Open(ScreenContext context)
    {
        var screen = new MenuControl();

        screen.PressureRequested += (_, _) =>
            context.Open<PressureRegistration>();

        screen.TemperatureRequested += (_, _) =>
            context.Open<TemperatureRegistration>();

        screen.ScfVolumeRequested += (_, _) =>
            context.Open<ScfVolumeRegistration>();

        screen.VolumeRequested += (_, _) =>
            context.Open<VolumeRegistration>();

        screen.EngineRequested += (_, _) =>
            context.Open<EngineRegistration>();

        screen.SettingsRequested += (_, _) =>
            context.Open<SettingsRegistration>();

        screen.CloseRequested += (_, _) =>
            context.BackScreen();

        context.ShowScreen(screen);

        return true;
    }
}