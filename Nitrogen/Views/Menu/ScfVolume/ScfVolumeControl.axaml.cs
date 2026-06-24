using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.Common;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Menu.Pressure;
using System;
using System.Threading.Tasks;

namespace Nitrogen.Views.Menu.SCFVolume;

public partial class ScfVolumeControl : UserControl, IHotKeyScreen, IScreenLoadable
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    private readonly HotKeyFocusMap _focusMap = new();
    public ScfVolumeControl()
    {
        InitializeComponent();
        _focusMap.Add(Key.F4, TotalScfStageEditTextBox);
        _focusMap.Add(Key.F5, Pump1ScfStageTextBox);
        _focusMap.Add(Key.F7, TotalScfJobTextBox);
        _focusMap.Add(Key.F8, Pump1ScfJobTextBox);
    }
    public ScfVolumeControl(Nitrogen.MainWindow mainWindow)
        : this()
    {
        _mainWindow = mainWindow;
    }

    public void HandleKey(Key key)
    {
        if (_focusMap.Focus(key))
            return;
     

        if (DataContext is not ScfVolumeControlViewModel vm)
            return;

        switch (key)
        {
            case Key.Escape:
                _mainWindow?.CloseScreen();
                break;

            case Key.F1:
                vm.SetCommand.Execute().Subscribe();
                break;

            case Key.F2:
                vm.ResetStageCommand.Execute().Subscribe();
                break;

            case Key.F3:
                vm.ResetJobCommand.Execute().Subscribe();
                break;
        }
    }

    public async Task LoadAsync()
    {
        if (DataContext is ScfVolumeControlViewModel vm)
            await vm.LoadAsync();
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.CloseScreen();
    }
}