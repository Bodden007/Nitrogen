using System;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Menu;
using Nitrogen.Views.Menu.Pressure;
using Nitrogen.Views.Menu.Engine;
using Nitrogen.Views.Menu.SCFVolume;
using Nitrogen.Views.Menu.Temperature;
using Nitrogen.Views.Menu.Volume;

namespace Nitrogen.Views.Navigation;

/// <summary>
/// Единая точка регистрации прикладных экранов HMI.
/// </summary>
internal static class ScreenComposition
{
    public static ScreenRegistry Create(
        ScreenContext context,
        MenuRegistration menuRegistration)
    {
        var registry = new ScreenRegistry(context);

        context.SetScreenOpener(registry.Open);

        Register(registry, menuRegistration);
        Register(registry, new PressureRegistration());
        Register(registry, new TemperatureRegistration());
        Register(registry, new ScfVolumeRegistration());
        Register(registry, new VolumeRegistration());
        Register(registry, new EngineRegistration());

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