using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.Settings;

public partial class DsPumpSetControl : UserControl, IHotKeyScreen
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    public DsPumpSetControl()
    {
        InitializeComponent();
    }
    public DsPumpSetControl(Nitrogen.MainWindow mainWindow)
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