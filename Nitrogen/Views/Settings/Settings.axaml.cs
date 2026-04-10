using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace Nitrogen;

public partial class Settings : Window
{
    public Settings()
    {
        InitializeComponent();

        SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;

        this.KeyDown += Settings_KeyDown;

        Closed += Settings_Closed;
    }
    private void Settings_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape: Close(); break;

            case Key.F1: new Nitrogen.PressureSet().Show(); break;
        }
    }
    private void PressureCalli_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.PressureSet pressureSet = new Nitrogen.PressureSet();
        pressureSet.Show();
    }
    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
    private void Settings_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= Settings_KeyDown;
        Closed -= Settings_Closed;
    }
}