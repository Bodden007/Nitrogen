using Avalonia.Controls;
using Nitrogen.Views.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nitrogen.Views.MainWindow.ScreenHost;

internal sealed class ScreenNavigator : IScreenNavigator
{
    private readonly ContentControl _screenHost;
    private readonly Stack<UserControl> _screenStack = new();

    public ScreenNavigator(ContentControl screenHost)
    {
        _screenHost = screenHost;
    }

    public void ShowScreen(UserControl screen)
    {
        if (_screenHost.Content is UserControl currentScreen)
            _screenStack.Push(currentScreen);

        _screenHost.Content = screen;

        _ = LoadScreenAsync(screen);
    }

    public void BackScreen()
    {
        if (_screenStack.Count == 0)
            return;

        _screenHost.Content = _screenStack.Pop();
    }

    public void CloseScreen()
    {
        _screenStack.Clear();
        _screenHost.Content = null;
    }

    private static async Task LoadScreenAsync(UserControl screen)
    {
        if (screen is IScreenLoadable loadable)
            await loadable.LoadAsync();
    }
}