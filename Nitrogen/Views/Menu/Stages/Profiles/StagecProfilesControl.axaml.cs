using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.Interfaces;
using System;

namespace Nitrogen.Views.Menu;

public partial class StagecProfilesControl : UserControl, IHotKeyScreen
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    public StagecProfilesControl()
    {
        InitializeComponent();
    }
    public StagecProfilesControl(Nitrogen.MainWindow mainWindow)
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