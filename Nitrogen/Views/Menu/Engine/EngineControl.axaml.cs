using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.Menu.Engine;

public partial class EngineControl :
    UserControl, IHotKeyScreen, IScreenLoadable
{
    public event EventHandler? CloseRequested;

    public EngineControl()
    {
        InitializeComponent();
    }

    public void HandleKey(Key key)
    {
        if (key == Key.Escape)
        {
            CloseScreen();
            return;
        }

        if (DataContext is not EngineViewModel vm)
            return;

        switch (key)
        {
            case Key.F1:
                vm.Neutral();
                break;

            case Key.F2:
                vm.Up();
                break;

            case Key.F3:
                vm.Down();
                break;
        }
    }

    public Task LoadAsync()
    {
        return DataContext is EngineViewModel vm
            ? vm.LoadAsync()
            : Task.CompletedTask;
    }

    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        CloseScreen();
    }

    private void Neutral_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is EngineViewModel vm)
            vm.Neutral();
    }

    private void Up_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is EngineViewModel vm)
            vm.Up();
    }

    private void Down_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is EngineViewModel vm)
            vm.Down();
    }
    private void CloseScreen()
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
