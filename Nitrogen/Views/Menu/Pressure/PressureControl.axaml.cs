
using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nitrogen.Views.Common;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.Menu.Pressure;

public partial class PressureControl :
    UserControl, IHotKeyScreen, IScreenLoadable
{
    private readonly HotKeyFocusMap _focusMap = new();

    public event EventHandler? CloseRequested;

    public PressureControl()
    {
        InitializeComponent();

        _focusMap.Add(Key.F4, OpkoTextBox);
    }

    public void HandleKey(Key key)
    {
        if (_focusMap.Focus(key))
            return;

        switch (key)
        {
            case Key.Escape:
                CloseRequested?.Invoke(this, EventArgs.Empty);
                break;

            case Key.F1:
                // TODO [CoreLink]: SetZero.
                break;

            case Key.F2:
                // TODO [CoreLink]: ResetZero.
                break;

            case Key.F3:
            case Key.Enter:
                // TODO [CoreLink]: SetShutdown.
                break;
        }
    }

    public Task LoadAsync()
    {
        return DataContext is PressureViewModel vm
            ? vm.LoadAsync()
            : Task.CompletedTask;
    }

    private void SetZero_Click(
        object? sender,
        RoutedEventArgs e)
    {
        // TODO [CoreLink]: SetZero.
    }

    private void ResetZero_Click(
        object? sender,
        RoutedEventArgs e)
    {
        // TODO [CoreLink]: ResetZero.
    }

    private void SetShutdown_Click(
        object? sender,
        RoutedEventArgs e)
    {
        // TODO [CoreLink]: SetShutdown.
    }

    private void Exit_Click(
        object? sender,
        RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
