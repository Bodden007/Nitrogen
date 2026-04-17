using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace Nitrogen.Views.Settings;

public partial class Settings : Window
{
    public Settings()
    {
        InitializeComponent();

        //SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;

        this.KeyDown += Settings_KeyDown;

        Closed += Settings_Closed;
    }
    private void Settings_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape: Close(); break;

            case Key.F1: new Nitrogen.Views.Settings.PressureSet().Show(); break;

            case Key.F5: new Nitrogen.Views.Settings.DsPumpSet().Show(); break;

            case Key.F6: new Nitrogen.Views.Settings.PsPumpSet().Show(); break;
        }
    }
    private void PressureCalli_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Settings.PressureSet pressureSet = new Nitrogen.Views.Settings.PressureSet();
        pressureSet.Show();
    }
    private void DsPumpCalli_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Settings.DsPumpSet dsPumpSet = new Nitrogen.Views.Settings.DsPumpSet();
        dsPumpSet.Show();
    }
    private void PsPumpCalli_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Settings.PsPumpSet psPumpSet = new Nitrogen.Views.Settings.PsPumpSet();
        psPumpSet.Show();
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