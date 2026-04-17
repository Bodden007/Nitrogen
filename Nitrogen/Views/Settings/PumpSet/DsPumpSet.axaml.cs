using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace Nitrogen.Views.Settings;

public partial class DsPumpSet : Window
{
    public DsPumpSet()
    {
        InitializeComponent();

        //SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;

        this.KeyDown += DsPumpSet_KeyDown;

        Closed += DsPumpSet_Closed;
    }
    private void DsPumpSet_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
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

    private void DsPumpSet_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= DsPumpSet_KeyDown;
        Closed -= DsPumpSet_Closed;
    }
}