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

    public async void HandleKey(Key key)
    {
        if (_focusMap.Focus(key))
            return;

        if (key == Key.Escape)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (DataContext is not PressureViewModel vm)
            return;

        switch (key)
        {
            case Key.F1:
                await vm.SetZeroAsync();
                break;

            case Key.F2:
                await vm.ResetZeroAsync();
                break;

            case Key.F3:
            case Key.Enter:
                await vm.SetShutdownAsync();
                break;
        }
    }

    public Task LoadAsync()
    {
        return DataContext is PressureViewModel vm
            ? vm.LoadAsync()
            : Task.CompletedTask;
    }

    private async void SetZero_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (DataContext is PressureViewModel vm)
            await vm.SetZeroAsync();
    }

    private async void ResetZero_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (DataContext is PressureViewModel vm)
            await vm.ResetZeroAsync();
    }

    private async void SetShutdown_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (DataContext is PressureViewModel vm)
            await vm.SetShutdownAsync();
    }

    private void Exit_Click(
        object? sender,
        RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}