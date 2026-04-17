using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Nitrogen;

public partial class Engine : Window
{
    public Engine()
    {
        InitializeComponent();

        WindowState = WindowState.FullScreen;

        this.KeyDown += Engine_KeyDown;

        Closed += Engine_Closed;
    }
    private void Engine_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
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
    private void Engine_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= Engine_KeyDown;
        Closed -= Engine_Closed;
    }
}