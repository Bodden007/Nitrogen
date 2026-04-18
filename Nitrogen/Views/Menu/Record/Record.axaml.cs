using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace Nitrogen.Views.Menu.Record;

public partial class Record : Window
{
    public Record()
    {
        InitializeComponent();

        WindowState = WindowState.FullScreen;

        KeyDown += Record_KeyDown;

        Closed += Record_Closed;
    }
    private void Record_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
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
    private void Record_Closed(object? sender, System.EventArgs e)
    {
        KeyDown -= Record_KeyDown;
        Closed -= Record_Closed;
    }
}