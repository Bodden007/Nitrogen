using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.Common;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Settings.PressureSet;
using System.Threading.Tasks;

namespace Nitrogen.Views.Settings;

public partial class PressureSetControl : UserControl, IHotKeyScreen, IScreenLoadable
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    private readonly HotKeyFocusMap _focusMap = new();

    public PressureSetControl()
    {
        InitializeComponent();

        _focusMap.Add(Key.F3, SrcMinTextBox);
        _focusMap.Add(Key.F4, SrcMaxTextBox);
        _focusMap.Add(Key.F5, TargMinTextBox);
        _focusMap.Add(Key.F6, TargMaxTextBox);
    }

    public PressureSetControl(Nitrogen.MainWindow mainWindow)
        : this()
    {
        _mainWindow = mainWindow;
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

        if (_mainWindow is null)
            return;

        switch (key)
        {
            case Key.Escape:
                _mainWindow.BackScreen();
                break;

            case Key.F1:
                if (DataContext is PressureSetControlViewModel vm)
                   _= vm.SaveAsync();
                break;
        }
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.BackScreen();
    }

    private async void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is PressureSetControlViewModel vm)
            await vm.SaveAsync();
    }
}