using System;
using Avalonia.Controls;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.MainView;
using CoreLink.Client;

namespace Nitrogen.Views.Navigation;

/// <summary>
/// Контекст создания и подключения экранов.
/// </summary>
internal sealed class ScreenContext
{
    private readonly Func<MainViewModel?> _getMainViewModel;

    private readonly Action<UserControl, IDisposable?> _showScreen;

    private readonly Action _backScreen;

    private Func<Type, bool>? _openScreen;

    public ScreenContext(
        Func<MainViewModel?> getMainViewModel,
        Action<UserControl, IDisposable?> showScreen,
        Action backScreen)
    {
        _getMainViewModel = getMainViewModel;
        _showScreen = showScreen;
        _backScreen = backScreen;
    }

    public MainViewModel? MainViewModel =>
        _getMainViewModel();

    public CoreLinkClient? Client =>
        MainViewModel?.Client;

    /// <summary>
    /// Подключение механизма межэкранной навигации.
    /// </summary>
    internal void SetScreenOpener(
        Func<Type, bool> openScreen)
    {
        _openScreen = openScreen;
    }

    /// <summary>
    /// Открытие зарегистрированного экрана.
    /// </summary>
    public bool Open<T>()
        where T : class, IScreenRegistration
    {
        return _openScreen?.Invoke(typeof(T)) ?? false;
    }

    /// <summary>
    /// Передача созданного экрана и его ресурсов навигатору.
    /// </summary>
    public void ShowScreen(
        UserControl screen,
        IDisposable? ownedResource = null)
    {
        _showScreen(screen, ownedResource);
    }

    public void BackScreen()
    {
        _backScreen();
    }
}