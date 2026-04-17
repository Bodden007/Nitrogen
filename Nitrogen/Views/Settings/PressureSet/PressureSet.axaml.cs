using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace Nitrogen.Views.Settings;

public partial class PressureSet : Window
{
    public PressureSet()
    {
        InitializeComponent();

        //SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;

        this.KeyDown += PressureSet_KeyDown;

        Closed += PressureSet_Closed;
    }
    private void PressureSet_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
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
    private void PressureSet_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= PressureSet_KeyDown;
        Closed -= PressureSet_Closed;
    }
}