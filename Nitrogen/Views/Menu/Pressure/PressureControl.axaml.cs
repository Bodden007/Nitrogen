using Avalonia.Controls;
using Avalonia.Input;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.Menu.Pressure;
using System.Threading.Tasks;

namespace Nitrogen.Views.Menu.Pressure;

public partial class PressureControl : UserControl, IHotKeyScreen, IScreenLoadable
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
        if (key == Key.Escape)
            _mainWindow?.CloseScreen();
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