
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nitrogen.Views.Common;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Settings.SCFSet;
using System;
using System.Threading.Tasks;

namespace Nitrogen.Views.Settings;

public partial class SCFSetControl : UserControl, IHotKeyScreen, IScreenLoadable
{
    private readonly HotKeyFocusMap _focusMap = new();

    public event EventHandler? CloseRequested;

    public SCFSetControl()
    {
        InitializeComponent();

        _focusMap.Add(Key.F3, TargTeetnCountTextBox);
        _focusMap.Add(Key.F4, TargPumpFactorTextBox);
        _focusMap.Add(Key.F5, TargEfficiencyFactorTextBox);
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
                if (DataContext is SCFSetControlViewModel vm)
                    _ = vm.SaveAsync();
                break;
        }
    }

    public async Task LoadAsync()
    {
        if (DataContext is SCFSetControlViewModel vm)
            await vm.LoadAsync();
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is SCFSetControlViewModel vm)
            await vm.SaveAsync();
    }

    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
