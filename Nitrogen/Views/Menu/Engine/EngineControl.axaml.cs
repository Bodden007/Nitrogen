using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Menu.SCFVolume;
using System;
using System.Threading.Tasks;

namespace Nitrogen.Views.Menu.Engine;

public partial class EngineControl : UserControl, IHotKeyScreen, IScreenLoadable
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    public EngineControl()
    {
        InitializeComponent();
    }
    public EngineControl(Nitrogen.MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
    }

    public void HandleKey(Key key)
    {
        if (_mainWindow is null)
            return;

        if (DataContext is not EngineViewModel vm)
            return;

        switch (key)
        {
            case Key.Escape:
                _mainWindow.CloseScreen();
                break;

            case Key.F1:
                vm.NeutralCommand.Execute();
                break;

            case Key.F2:
                vm.UpCommand.Execute();
                break;

            case Key.F3:
                vm.DownCommand.Execute();
                break;
        }
    }

    public async Task LoadAsync()
    {
        if (DataContext is EngineViewModel vm)
            await vm.LoadAsync();
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.CloseScreen();
    }

}