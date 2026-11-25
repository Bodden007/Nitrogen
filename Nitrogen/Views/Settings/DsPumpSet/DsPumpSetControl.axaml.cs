
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nitrogen.Views.Interfaces;
using System;

namespace Nitrogen.Views.Settings;

public partial class DsPumpSetControl : UserControl, IHotKeyScreen
{
    public event EventHandler? CloseRequested;

    public DsPumpSetControl()
    {
        InitializeComponent();
    }

    public void HandleKey(Key key)
    {
        switch (key)
        {
            case Key.Escape:
                CloseRequested?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
