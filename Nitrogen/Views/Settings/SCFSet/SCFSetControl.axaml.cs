using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.Common;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.Settings;

public partial class SCFSetControl : UserControl, IHotKeyScreen
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    private readonly HotKeyFocusMap _focusMap = new();
    public SCFSetControl()
    {
        InitializeComponent();
        _focusMap.Add(Key.F3, TargTeetnCountTextBox);
        _focusMap.Add(Key.F4, TargPumpFactorTextBox);
        _focusMap.Add(Key.F5, TargEfficiencyFactorTextBox);        
    }

    public SCFSetControl(Nitrogen.MainWindow mainWindow)
         : this()
    {
        _mainWindow = mainWindow;
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
                _mainWindow?.BackScreen();
                break;

        }
    }

    private void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.BackScreen();
    }
}