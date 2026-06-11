using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Settings;
using System;

namespace Nitrogen.Views.Menu;

public partial class MenuControl : UserControl, IHotKeyScreen
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    public MenuControl()
    {
        InitializeComponent();
    }
    public MenuControl(Nitrogen.MainWindow mainWindow)
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
                _mainWindow.ShowScreen(new PressureControl(_mainWindow));
                break;

            case Key.F2:
                _mainWindow.ShowScreen(new TemperatureControl(_mainWindow));
                break;

            case Key.F3:
                _mainWindow.ShowScreen(new ScfVolumeControl(_mainWindow));
                break;

            case Key.F4:
                _mainWindow.ShowScreen(new VolumeControl(_mainWindow));
                break;

            case Key.F5:
                _mainWindow.ShowScreen(new EngineControl(_mainWindow));
                break;

            case Key.F6:
                _mainWindow.ShowScreen(new StagecProfilesControl(_mainWindow));
                break;

            case Key.F7:
                Environment.Exit(0);
                break;

            case Key.F8:
                _mainWindow.ShowScreen(new SettingsControl(_mainWindow));
                break;

        }
    }

    private void Pressure_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.ShowScreen(new PressureControl(_mainWindow));
    }

    private void Temperature_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.ShowScreen(new TemperatureControl(_mainWindow));
    }

    private void ScfValue_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.ShowScreen(new ScfVolumeControl(_mainWindow));
    }

    private void Volume_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.ShowScreen(new VolumeControl(_mainWindow));
    }

    private void Engine_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.ShowScreen(new EngineControl(_mainWindow));
    }

    private void Profiles_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.ShowScreen(new StagecProfilesControl(_mainWindow));
    }

    private void CloseApp_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    private void Settings_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.ShowScreen(new SettingsControl(_mainWindow));
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.CloseScreen();
    }
}