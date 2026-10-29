
using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.Menu;

public partial class TemperatureControl : UserControl, IHotKeyScreen
{
    public event EventHandler? CloseRequested;

    public TemperatureControl()
    {
        InitializeComponent();
    }

    public void HandleKey(Key key)
    {
        if (key == Key.Escape)
            CloseScreen();
    }

    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        CloseScreen();
    }

    private void CloseScreen()
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
