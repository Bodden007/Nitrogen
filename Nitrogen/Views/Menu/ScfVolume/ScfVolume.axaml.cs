using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace Nitrogen.Views.Menu;

public partial class ScfVolume : Window
{
    public ScfVolume()
    {
        InitializeComponent();

        WindowState = WindowState.FullScreen;

        this.KeyDown += ScfVolume_KeyDown;
        Closed += ScfVolume_Closed;
    }
    private void ScfVolume_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
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
    private void ScfVolume_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= ScfVolume_KeyDown;
        Closed -= ScfVolume_Closed;
    }
}