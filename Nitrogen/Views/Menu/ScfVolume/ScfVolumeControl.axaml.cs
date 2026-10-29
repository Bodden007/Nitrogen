
using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nitrogen.Views.Common;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.Menu.SCFVolume;

public partial class ScfVolumeControl :
    UserControl, IHotKeyScreen, IScreenLoadable
{
    private readonly HotKeyFocusMap _focusMap = new();

    public event EventHandler? CloseRequested;

    public ScfVolumeControl()
    {
        InitializeComponent();

        _focusMap.Add(Key.F4, TotalScfStageEditTextBox);
        _focusMap.Add(Key.F5, Pump1ScfStageTextBox);
        _focusMap.Add(Key.F7, TotalScfJobTextBox);
        _focusMap.Add(Key.F8, Pump1ScfJobTextBox);
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
                // TODO [CoreLink]: Установка значений.
                break;

            case Key.F2:
                // TODO [CoreLink]: Сброс стадии.
                break;

            case Key.F3:
                // TODO [CoreLink]: Сброс работы.
                break;
        }
    }

    public Task LoadAsync()
    {
        return DataContext is ScfVolumeControlViewModel vm
            ? vm.LoadAsync()
            : Task.CompletedTask;
    }

    private void Set_Click(
        object? sender,
        RoutedEventArgs e)
    {
        // TODO [CoreLink]: Установка значений.
    }

    private void ResetStage_Click(
        object? sender,
        RoutedEventArgs e)
    {
        // TODO [CoreLink]: Сброс стадии.
    }

    private void ResetJob_Click(
        object? sender,
        RoutedEventArgs e)
    {
        // TODO [CoreLink]: Сброс работы.
    }

    private void Exit_Click(
        object? sender,
        RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
