
using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.Menu;

public partial class VolumeControl : UserControl, IHotKeyScreen
{
    public event EventHandler? CloseRequested;

    public VolumeControl()
    {
        InitializeComponent();
    }

    public void HandleKey(Key key)
    {
        if (key == Key.Escape)
            CloseScreen();
    }

    private void SetVolume_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Реализовать установку объёма.
        // В Legacy обработчик не реализован.
    }

    private void ZeroVolume_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Реализовать сброс объёма.
        // В Legacy обработчик не реализован.
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
