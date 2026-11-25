
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nitrogen.Views.Common;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Settings.PressureSet;
using System;
using System.Threading.Tasks;

namespace Nitrogen.Views.Settings;

public partial class PressureSetControl : UserControl, IHotKeyScreen, IScreenLoadable
{
    private readonly HotKeyFocusMap _focusMap = new();

    public event EventHandler? CloseRequested;

    public PressureSetControl()
    {
        InitializeComponent();

        _focusMap.Add(Key.F3, SrcMinTextBox);
        _focusMap.Add(Key.F4, SrcMaxTextBox);
        _focusMap.Add(Key.F5, TargMinTextBox);
        _focusMap.Add(Key.F6, TargMaxTextBox);
    }

    public async Task LoadAsync()
    {
        if (DataContext is PressureSetControlViewModel vm)
            await vm.LoadAsync();
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
                if (DataContext is PressureSetControlViewModel vm)
                    _ = vm.SaveAsync();
                break;
        }
    }

    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is PressureSetControlViewModel vm)
            await vm.SaveAsync();
    }
}
