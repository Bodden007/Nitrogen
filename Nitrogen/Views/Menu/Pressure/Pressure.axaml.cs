using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nitrogen;

public partial class Pressure : Window
{
    public Pressure()
    {
        InitializeComponent();

        SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;

        this.KeyDown += Pressure_KeyDown;

        Closed += Pressure_Closed;
    }

    private void Pressure_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Avalonia.Input.Key.Escape: Close(); e.Handled = true; break;
        }
    }
    private void SetShutdown_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private void Pressure_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= Pressure_KeyDown;
        Closed -= Pressure_Closed;
    }


}