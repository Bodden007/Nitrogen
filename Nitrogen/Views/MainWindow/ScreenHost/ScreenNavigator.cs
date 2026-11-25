using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.MainWindow.ScreenHost;

internal sealed class ScreenNavigator : IScreenNavigator
{
    private readonly ContentControl _screenHost;
    private readonly Stack<ScreenEntry> _screenStack = new();

    private ScreenEntry? _currentScreen;
    private bool _disposed;

    public ScreenNavigator(ContentControl screenHost)
    {
        _screenHost = screenHost;
    }

    public void ShowScreen(
        UserControl screen,
        IDisposable? ownedResource = null)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_currentScreen is not null)
            _screenStack.Push(_currentScreen);

        _currentScreen = new ScreenEntry(
            screen,
            ownedResource);

        _screenHost.Content = screen;

        // TODO: Обработать ошибки асинхронной загрузки.
        _ = LoadScreenAsync(screen);
    }

    public void BackScreen()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        ReleaseCurrentScreen();

        if (_screenStack.TryPop(out var previousScreen))
        {
            _currentScreen = previousScreen;
            _screenHost.Content = previousScreen.Screen;
        }
    }

    public void CloseScreen()
    {
        ReleaseCurrentScreen();

        while (_screenStack.TryPop(out var entry))
            entry.Dispose();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        CloseScreen();
    }

    private void ReleaseCurrentScreen()
    {
        var entry = _currentScreen;

        _currentScreen = null;
        _screenHost.Content = null;

        entry?.Dispose();
    }

    private static async Task LoadScreenAsync(
        UserControl screen)
    {
        if (screen is IScreenLoadable loadable)
            await loadable.LoadAsync();
    }

    private sealed class ScreenEntry : IDisposable
    {
        private readonly IDisposable? _ownedResource;
        private bool _disposed;

        public UserControl Screen { get; }

        public ScreenEntry(
            UserControl screen,
            IDisposable? ownedResource)
        {
            Screen = screen;
            _ownedResource = ownedResource;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            Screen.DataContext = null;

            _ownedResource?.Dispose();
        }
    }
}