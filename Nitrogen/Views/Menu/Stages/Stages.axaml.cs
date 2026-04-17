using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace Nitrogen.Views.Menu;

public partial class Stages : Window
{
    public Stages()
    {
        InitializeComponent();

        WindowState = WindowState.FullScreen;

        this.KeyDown += Stages_KeyDown;

        Closed += Stages_Closed;
    }
    private void Stages_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
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
    private void Stages_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= Stages_KeyDown;
        Closed -= Stages_Closed;
    }
}