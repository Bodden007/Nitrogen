using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.MainWindow.Interfaces;

namespace Nitrogen.Views.Settings;

public partial class PressureSetControl : UserControl, IHotKeyScreen
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    public PressureSetControl()
    {
        InitializeComponent();
    }
    public PressureSetControl(Nitrogen.MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
    }

    public void HandleKey(Key key)
    {
        if (_mainWindow is null)
            return;

        switch (key)
        {
            case Key.Escape:
                _mainWindow.BackScreen();
                break;
        }
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.BackScreen();
    }
}