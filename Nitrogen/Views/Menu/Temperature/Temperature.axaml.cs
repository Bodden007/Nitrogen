using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace Nitrogen;

public partial class Temperature : Window
{
    public Temperature()
    {
        InitializeComponent();

        WindowState = WindowState.FullScreen;

        this.KeyDown += Temperature_KeyDown;

        Closed += Temperature_Closed;
    }
    private void Temperature_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape: Close(); break;
        }
    }
    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
    private void Temperature_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= Temperature_KeyDown;
        Closed -= Temperature_Closed;
    }
}