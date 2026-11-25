
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nitrogen.Views.Interfaces;
using System;

namespace Nitrogen.Views.Settings;

public partial class SettingsControl : UserControl, IHotKeyScreen
{
    public event EventHandler? CloseRequested;
    public event EventHandler? PressureSetRequested;
    public event EventHandler? ScfSetRequested;
    public event EventHandler? DsPumpSetRequested;
    public event EventHandler? PsPumpSetRequested;

    public SettingsControl()
    {
        InitializeComponent();
    }

    public void HandleKey(Key key)
    {
        switch (key)
        {
            case Key.Escape:
                CloseMenu();
                break;

            case Key.F1:
                ShowPressureSetScreen();
                break;

            case Key.F2:
                ShowScfSetScreen();
                break;

            case Key.F5:
                ShowDsPumpSetScreen();
                break;

            case Key.F6:
                ShowPsPumpSetScreen();
                break;
        }
    }

    private void PressureCalli_Click(object? sender, RoutedEventArgs e)
    {
        ShowPressureSetScreen();
    }

    private void SCFCalli_Click(object? sender, RoutedEventArgs e)
    {
        ShowScfSetScreen();
    }

    private void DsPumpCalli_Click(object? sender, RoutedEventArgs e)
    {
        ShowDsPumpSetScreen();
    }

    private void PsPumpCalli_Click(object? sender, RoutedEventArgs e)
    {
        ShowPsPumpSetScreen();
    }

    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        CloseMenu();
    }

    private void ShowPressureSetScreen()
    {
        PressureSetRequested?.Invoke(this, EventArgs.Empty);
    }

    private void ShowScfSetScreen()
    {
        ScfSetRequested?.Invoke(this, EventArgs.Empty);
    }

    private void ShowDsPumpSetScreen()
    {
        DsPumpSetRequested?.Invoke(this, EventArgs.Empty);
    }

    private void ShowPsPumpSetScreen()
    {
        PsPumpSetRequested?.Invoke(this, EventArgs.Empty);
    }

    private void CloseMenu()
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
