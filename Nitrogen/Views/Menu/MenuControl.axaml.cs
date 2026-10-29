
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.Menu;

public partial class MenuControl : UserControl, IHotKeyScreen
{
    public event EventHandler? CloseRequested;
    public event EventHandler? PressureRequested;
    public event EventHandler? ScfVolumeRequested;
    public MenuControl()
    {
        InitializeComponent();
    }

    public void HandleKey(Key key)
    {
        switch (key)
        {
            case Key.Escape:
                CloseMenu();
                break;

            case Key.F1:
                ShowPressureScreen();
                break;

            case Key.F2:
                ShowTemperatureScreen();
                break;

            case Key.F3:
                ShowScfVolumeScreen();
                break;

            case Key.F4:
                ShowVolumeScreen();
                break;

            case Key.F5:
                ShowEngineScreen();
                break;

            case Key.F6:
                // TODO: Перенести StagecProfilesControl.
                break;

            case Key.F7:
                CloseApplication();
                break;

            case Key.F8:
                ShowSettingsScreen();
                break;
        }
    }

    private void Pressure_Click(object? sender, RoutedEventArgs e)
    {
        ShowPressureScreen();
    }

    private void Temperature_Click(object? sender, RoutedEventArgs e)
    {
        ShowTemperatureScreen();
    }

    private void ScfValue_Click(object? sender, RoutedEventArgs e)
    {
        ShowScfVolumeScreen();
    }

    private void Volume_Click(object? sender, RoutedEventArgs e)
    {
        ShowVolumeScreen();
    }

    private void Engine_Click(object? sender, RoutedEventArgs e)
    {
        ShowEngineScreen();
    }

    private void Profiles_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Перенести StagecProfilesControl.
    }

    private void CloseApp_Click(object? sender, RoutedEventArgs e)
    {
        CloseApplication();
    }

    private void Settings_Click(object? sender, RoutedEventArgs e)
    {
        ShowSettingsScreen();
    }

    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        CloseMenu();
    }

    private void ShowPressureScreen()
    {
        PressureRequested?.Invoke(this, EventArgs.Empty);
    }
    private void ShowTemperatureScreen()
    {
        // TODO: Открыть экран температуры.
    }

    private void ShowScfVolumeScreen()
    {
        // TODO: Открыть экран SCF.
    }

    private void ShowEngineScreen()
    {
        // TODO: Открыть экран двигателя.
    }

    private void ShowSettingsScreen()
    {
        // TODO: Открыть экран настроек.
    }

    private void ShowVolumeScreen()
    {
        // TODO: Подключить открытие экрана объёмов.
    }

    private void CloseApplication()
    {
        // TODO: Реализовать закрытие приложения
        // с учётом Desktop, Android и Browser.
        Environment.Exit(0);
    }

    private void CloseMenu()
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
