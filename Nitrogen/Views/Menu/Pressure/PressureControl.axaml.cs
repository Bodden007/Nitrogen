using Avalonia.Controls;
using Avalonia.Input;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.MainWindow;
using System.Threading.Tasks;

namespace Nitrogen.Views.Menu;

public partial class PressureControl : UserControl, IHotKeyScreen,
    IScreenLoadable
{
    private readonly Nitrogen.MainWindow? _mainWindow;
    public PressureControl()
    {
        InitializeComponent();
    }
    public PressureControl(Nitrogen.MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
    }
    public void HandleKey(Key key)
    {
        if (_mainWindow is null)
            return;
        if (key == Key.Escape)
            _mainWindow.CloseScreen();
    }
    public async Task LoadAsync()
    {
        LoadOpkoOnce();
        await Task.CompletedTask;
    }
    private void LoadOpkoOnce()
    {
        if (DataContext is not MainWindowViewModel vm)
            return;

        OpkoTextBox.Text = vm.Opko_1;
    }
    private void SetZero_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void ResetZero_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void SetShutdown_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _mainWindow?.CloseScreen();
    }
}