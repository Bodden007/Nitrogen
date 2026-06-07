using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.MainWindow.Interfaces;

namespace Nitrogen.Views.Menu;

public partial class VolumeControl : UserControl, IHotKeyScreen
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    public VolumeControl()
    {
        InitializeComponent();
    }
    public VolumeControl(Nitrogen.MainWindow mainWindow)
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

    private void SetVolume_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
    private void ZeroVolume_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.CloseScreen();
    }
}