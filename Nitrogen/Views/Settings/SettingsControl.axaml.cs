using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.MainWindow.Interfaces;
using Nitrogen.Views.Menu;
using System;

namespace Nitrogen.Views.Settings;

public partial class SettingsControl : UserControl, IHotKeyScreen
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    public SettingsControl()
    {
        InitializeComponent();
    }
    public SettingsControl(Nitrogen.MainWindow mainWindow)
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

            case Key.F1:
                _mainWindow.ShowScreen(new Nitrogen.Views.Settings.PressureSetControl(_mainWindow));
                break;

            case Key.F5:
                _mainWindow.ShowScreen(new Nitrogen.Views.Settings.DsPumpSetControl(_mainWindow));
                break;

            case Key.F6:
                _mainWindow.ShowScreen(new Nitrogen.Views.Settings.PsPumpSetControl(_mainWindow));
                break;
        }
    }

    private void PressureCalli_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.ShowScreen(new Nitrogen.Views.Settings.PressureSetControl(_mainWindow));
    }

    private void DsPumpCalli_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.ShowScreen(new Nitrogen.Views.Settings.DsPumpSetControl(_mainWindow));
    }

    private void PsPumpCalli_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.ShowScreen(new Nitrogen.Views.Settings.PsPumpSetControl(_mainWindow));
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.CloseScreen();
    }
}