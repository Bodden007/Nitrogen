using System;
using System.Collections.Generic;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.Navigation;

/// <summary>
/// Реестр механизмов подключения экранов.
/// Не хранит экземпляры экранов и ViewModel.
/// </summary>
internal sealed class ScreenRegistry
{
    private readonly Dictionary<Type, IScreenRegistration>
        _registrations = new();

    private readonly ScreenContext _context;

    public ScreenRegistry(ScreenContext context)
    {
        _context = context;
    }

    public bool Register<T>(T registration)
        where T : class, IScreenRegistration
    {
        return _registrations.TryAdd(
            typeof(T),
            registration);
    }

    public bool Open<T>()
        where T : class, IScreenRegistration
    {
        return Open(typeof(T));
    }

    internal bool Open(Type registrationType)
    {
        if (!_registrations.TryGetValue(
                registrationType,
                out var registration))
        {
            return false;
        }

        return registration.Open(_context);
    }
}