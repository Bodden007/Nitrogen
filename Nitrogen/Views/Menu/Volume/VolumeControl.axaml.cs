
using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nitrogen.Views.Common;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.Menu;

public partial class VolumeControl : UserControl, IHotKeyScreen
{
    private readonly HotKeyFocusMap _focusMap = new();

    public event EventHandler? CloseRequested;

    public VolumeControl()
    {
        InitializeComponent();

        _focusMap.Add(Key.F3, TotalVolumeTextBox);
        _focusMap.Add(Key.F4, DsVolumeTextBox);
        _focusMap.Add(Key.F5, PsVolumeTextBox);
    }

    public void HandleKey(Key key)
    {
        if (_focusMap.Focus(key))
            return;

        switch (key)
        {
            case Key.Escape:
                CloseScreen();
                break;

            case Key.F1:
                // TODO [CoreLink]: Установка объёмов.
                break;

            case Key.F2:
                // TODO [CoreLink]: Сброс объёмов.
                break;
        }
    }

    private void SetVolume_Click(
        object? sender,
        RoutedEventArgs e)
    {
        // TODO [CoreLink]: Установка объёмов.
        // В Legacy обработчик не реализован.
    }

    private void ZeroVolume_Click(
        object? sender,
        RoutedEventArgs e)
    {
        // TODO [CoreLink]: Сброс объёмов.
        // В Legacy обработчик не реализован.
    }

    private void Exit_Click(
        object? sender,
        RoutedEventArgs e)
    {
        CloseScreen();
    }

    private void CloseScreen()
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
