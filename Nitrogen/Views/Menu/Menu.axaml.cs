using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using System;

namespace Nitrogen.Views.Menu;

public partial class Menu : Window
{
    public Menu()
    {
        InitializeComponent();

        //SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;

        this.KeyDown += Menu_KeyDown;

        Closed += Menu_Closed;
    }
    private void Menu_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape: Close(); break;

            case Key.F1: new Nitrogen.Views.Menu.Pressure().Show(); break;

            case Key.F2: new Nitrogen.Views.Menu.Temperature().Show(); break;

            case Key.F3: new Nitrogen.Views.Menu.ScfVolume().Show(); break;

            case Key.F4: new Nitrogen.Views.Menu.Volume().Show(); break;

            case Key.F5: new Nitrogen.Views.Menu.Engine().Show(); break;

            case Key.F6: new Nitrogen.Views.Menu.Stage.Profiles.StageProfiles(); break;

            case Key.F7: Environment.Exit(0); break;

            case Key.F8: new Nitrogen.Views.Settings.Settings().Show(); break;
        }
    }
    private void Pressure_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.Pressure pressure = new Nitrogen.Views.Menu.Pressure();
        pressure.Show();
    }
    private void Temperature_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.Temperature temperature = new Nitrogen.Views.Menu.Temperature();
        temperature.Show();
    }
    private void ScfValue_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.ScfVolume scfVolume = new Nitrogen.Views.Menu.ScfVolume();
        scfVolume.Show();
    }
    private void Volume_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.Volume volume = new Nitrogen.Views.Menu.Volume();
        volume.Show();
    }
    private void Engine_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.Engine engine = new Nitrogen.Views.Menu.Engine();
        engine.Show();
    }
    private void Profiles_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.Stage.Profiles.StageProfiles stageProfiles = new Nitrogen.Views.Menu.Stage.Profiles.StageProfiles();
        stageProfiles.Show();
    }
    private void CloseApp_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //TODO Разобраться с корректным закрытием приложения
        //Avalonia.Application.Current?.Shutdown();
        Environment.Exit(0);
    }
    private void Settings_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Settings.Settings settings = new Nitrogen.Views.Settings.Settings();
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