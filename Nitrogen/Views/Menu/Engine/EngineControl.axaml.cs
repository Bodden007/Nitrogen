using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.MainWindow.Interfaces;
using System;

namespace Nitrogen.Views.Menu;

public partial class EngineControl : UserControl, IHotKeyScreen
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    public EngineControl()
    {
        InitializeComponent();
    }
    public EngineControl(Nitrogen.MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
    }

    public void HandleKey(Key key)
    {
        if (_mainWindow is null)
            return;

        switch (key)
        {
            case Key.Escape:
                _mainWindow.CloseScreen();
                break;
        }
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.CloseScreen();
    }
}