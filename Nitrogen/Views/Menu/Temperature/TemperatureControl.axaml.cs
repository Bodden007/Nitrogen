using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.MainWindow.Interfaces;

namespace Nitrogen.Views.Menu;

public partial class TemperatureControl : UserControl, IHotKeyScreen
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    public TemperatureControl()
    {
        InitializeComponent();
    }
    public TemperatureControl(Nitrogen.MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
    }

    public void HandleKey(Key key)
    {
        if (_mainWindow is null)
            return;
        if (key == Key.Escape)
            _mainWindow.CloseScreen();
    }
    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.CloseScreen();
    }
}