using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using System;

namespace Nitrogen;

public partial class Menu : Window
{
    public Menu()
    {
        InitializeComponent();

        SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;

        this.KeyDown += Menu_KeyDown;

        Closed += Menu_Closed;
    }
    private void Menu_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape: Close(); break;

            case Key.F1: new Nitrogen.Pressure().Show(); break;

            case Key.F5: new Nitrogen.Volume().Show(); break;

            case Key.F7: Environment.Exit(0); break;

            case Key.F8: new Nitrogen.Settings().Show(); break;
        }
    }
    private void Pressure_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Pressure pressure = new Nitrogen.Pressure();
        pressure.Show();
    }

    private void Volume_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Volume volume = new Nitrogen.Volume();
        volume.Show();
    }

    private void CloseApp_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //TODO Разобраться с корректным закрытием приложения
        //Avalonia.Application.Current?.Shutdown();
        Environment.Exit(0);
    }
    private void Settings_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Settings settings = new Nitrogen.Settings();
        settings.Show();
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private void Menu_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= Menu_KeyDown;
        Closed -= Menu_Closed;
    }
}