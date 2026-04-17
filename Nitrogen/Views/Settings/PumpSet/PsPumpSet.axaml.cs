using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace Nitrogen.Views.Settings;

public partial class PsPumpSet : Window
{
    public PsPumpSet()
    {
        InitializeComponent();

        //SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;

        this.KeyDown += PsPumpSet_KeyDown;

        Closed += PsPumpSet_Closed;
    }
    private void PsPumpSet_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
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
    private void PsPumpSet_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= PsPumpSet_KeyDown;
        Closed -= PsPumpSet_Closed;
    }
}