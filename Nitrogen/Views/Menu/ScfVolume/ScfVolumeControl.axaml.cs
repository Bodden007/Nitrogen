using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.MainWindow.Interfaces;
using System;

namespace Nitrogen.Views.Menu;

public partial class ScfVolumeControl : UserControl, IHotKeyScreen
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    public ScfVolumeControl()
    {
        InitializeComponent();
    }
    public ScfVolumeControl(Nitrogen.MainWindow mainWindow)
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