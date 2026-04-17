using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace Nitrogen.Views.Menu;

public partial class Volume : Window
{
    public Volume()
    {
        InitializeComponent();

        //SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;

        this.KeyDown += Volume_KeyDown;

        Closed += Volume_Closed;
    }
    private void Volume_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape: Close(); break;
        }
    }

    private void SetVolume_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
    private void ZeroVolume_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
    private void Volume_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= Volume_KeyDown;
        Closed -= Volume_Closed;
    }
}