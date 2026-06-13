using Avalonia.Controls;
using Avalonia.Input;
using Nitrogen.Views.Common;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Menu.Pressure;
using System;
using System.Threading.Tasks;

namespace Nitrogen.Views.Menu.Pressure;

public partial class PressureControl : UserControl, IHotKeyScreen, IScreenLoadable
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    private readonly HotKeyFocusMap _focusMap = new();

    public PressureControl()
    {
        InitializeComponent();
        _focusMap.Add(Key.F4, OpkoTextBox);
    }

    public PressureControl(Nitrogen.MainWindow mainWindow)
        : this()
    {
        _mainWindow = mainWindow;
    }

    public void HandleKey(Key key)
    {
        if (_focusMap.Focus(key))
            return;

        if (DataContext is not PressureViewModel vm)
            return;

        switch (key)
        {
            case Key.Escape:
                _mainWindow?.CloseScreen(); break;

            case Key.Enter:
                vm.SetShutdownCommand.Execute().Subscribe();
                break;

            case Key.F1:
                vm.SetZeroCommand.Execute().Subscribe();
                break;

            case Key.F2:
                vm.ResetZeroCommand.Execute().Subscribe();
                break;

            case Key.F3:
                vm.SetShutdownCommand.Execute().Subscribe();
                break;
        }
    }

    public async Task LoadAsync()
    {
        if (DataContext is PressureViewModel vm)
            await vm.LoadAsync();
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.CloseScreen();
    }
}