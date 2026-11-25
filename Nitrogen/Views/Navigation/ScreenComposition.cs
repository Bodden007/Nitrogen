using System;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Menu;
using Nitrogen.Views.Menu.Pressure;
using Nitrogen.Views.Menu.Engine;
using Nitrogen.Views.Menu.SCFVolume;
using Nitrogen.Views.Menu.Temperature;
using Nitrogen.Views.Menu.Volume;
using Nitrogen.Views.Settings;
using Nitrogen.Views.Settings.PressureSet;
using Nitrogen.Views.Settings.SCFSet;
using Nitrogen.Views.Settings.DsPumpSet;
using Nitrogen.Views.Settings.PumpSet;

namespace Nitrogen.Views.Navigation;

/// <summary>
/// Единая точка регистрации прикладных экранов HMI.
/// </summary>
internal static class ScreenComposition
{
    public static ScreenRegistry Create(ScreenContext context)
    {
        var registry = new ScreenRegistry(context);

        context.SetScreenOpener(registry.Open);

        // NOTE: Основное меню и прикладные экраны.
        Register(registry, new MenuRegistration());
        Register(registry, new PressureRegistration());
        Register(registry, new TemperatureRegistration());
        Register(registry, new ScfVolumeRegistration());
        Register(registry, new VolumeRegistration());
        Register(registry, new EngineRegistration());

        // NOTE: Меню настроек и экраны калибровки.
        Register(registry, new SettingsRegistration());
        Register(registry, new PressureSetRegistration());
        Register(registry, new ScfSetRegistration());
        Register(registry, new DsPumpSetRegistration());
        Register(registry, new PsPumpSetRegistration());

        return registry;
    }

    /// <summary>
    /// Регистрация экрана с проверкой дублирования.
    /// </summary>
    private static void Register<T>(
        ScreenRegistry registry,
        T registration)
        where T : class, IScreenRegistration
    {
        if (!registry.Register(registration))
        {
            throw new InvalidOperationException(
                $"{typeof(T).Name} is already registered.");
        }
    }
}