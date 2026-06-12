using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Nitrogen.Services.Modbus.Connection;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.MainWindow;
using Nitrogen.Views.Menu.Pressure;
using System.Collections.Generic;
using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using Nitrogen.Services.Modbus.Configuration.Models.Registers;

namespace Nitrogen;

public partial class MainWindow : Window
{
    private readonly Stack<UserControl> _screenStack = new();
    private readonly IModbusWriter? _writer;

    private readonly ModbusConnectionConfig? _connectionConfig;
    private readonly IReadOnlyList<ModbusRegisterConfig>? _holdingRegisters;

    private UserControl? _currentScreen;

    private MainWindowViewModel? MainVm => DataContext as MainWindowViewModel;
    public MainWindow()
    {
        InitializeComponent();

        WindowState = WindowState.FullScreen;

        this.KeyDown += MainWindow_KeyDown;
    }
    internal MainWindow(
      IModbusWriter writer,
      ModbusConnectionConfig connectionConfig,
      IReadOnlyList<ModbusRegisterConfig> holdingRegisters) : this()
    {
        _writer = writer;
        _connectionConfig = connectionConfig;
        _holdingRegisters = holdingRegisters;
    }
    private void MainWindow_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Source is TextBox && !IsCommandKey(e.Key))
            return;

        if (_currentScreen is IHotKeyScreen hotKeyScreen)
        {
            hotKeyScreen.HandleKey(e.Key);
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Escape)
        {
            e.Handled = true;
            WindowState = WindowState.FullScreen;
            return;
        }

        switch (e.Key)
        {
            case Key.F1:
                {
                    ShowScreen(CreatePressureScreen());
                    e.Handled = true;
                    break;
                }

            case Key.F2:
                {
                    ShowScreen(new Nitrogen.Views.Menu.TemperatureControl(this));
                    e.Handled = true;
                    break;
                }

            case Key.F3:
                {
                    ShowScreen(new Nitrogen.Views.Menu.ScfVolumeControl(this));
                    e.Handled = true;
                    break;
                }

            case Key.F4:
                {
                    ShowScreen(new Nitrogen.Views.Menu.VolumeControl(this));
                    e.Handled = true;
                    break;
                }

            case Key.F5:
                {
                    ShowScreen(new Nitrogen.Views.Menu.EngineControl(this));
                    e.Handled = true;
                    break;
                }

            case Key.F6:
                {
                    ShowScreen(new Nitrogen.Views.Menu.RecordControl(this));
                    e.Handled = true;
                    break;
                }

            case Key.F7:
                {
                    ShowScreen(new Nitrogen.Views.Menu.StagesControl(this));
                    e.Handled = true;
                    break;
                }

            case Key.F8:
                {
                    ShowScreen(new Nitrogen.Views.Menu.MenuControl(this));
                    e.Handled = true;
                    break;
                }

            case Key.D1: HandlerRateLineDisplay(); break;

            case Key.D2: HandlerTempLineDisplay(); break;

            case Key.D3: HandlerVolumeLineDisplay(); break;
        }
    }
    private void Pressure_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowScreen(CreatePressureScreen());
    }
    private void Volume_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowScreen(new Nitrogen.Views.Menu.VolumeControl(this));
    }
    private void Engine_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowScreen(new Nitrogen.Views.Menu.EngineControl(this));
    }
    private void Menu_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowScreen(new Nitrogen.Views.Menu.MenuControl(this));
    }
    private void Temp_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowScreen(new Nitrogen.Views.Menu.TemperatureControl(this));
    }
    private void Stages_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowScreen(new Nitrogen.Views.Menu.StagesControl(this));
    }
    private void Scf_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowScreen(new Nitrogen.Views.Menu.ScfVolumeControl(this));
    }
    private void Record_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowScreen(new Nitrogen.Views.Menu.RecordControl(this));
    }
    private void HandlerVolumeLineDisplay()
    {
        if (CombiVolume.IsVisible)
        {
            DsStage.IsVisible = true;
            PsStage.IsVisible = true;
            PsVolume.IsVisible = true;
            DsVolume.IsVisible = true;
            CombiVolume.IsVisible = false;
            CombiHeader.Text = "Объем Цемента";
        }
        else
        {
            DsStage.IsVisible = false;
            PsStage.IsVisible = false;
            PsVolume.IsVisible = false;
            DsVolume.IsVisible = false;
            CombiVolume.IsVisible = true;
            CombiHeader.Text = "Σ Объем Цемента";
        }
    }
    private void HandlerTempLineDisplay()
    {
        if (!TempOut.IsVisible)
        {
            HeaderTemp.Text = "Температура";

            TempOut.IsVisible = true;
            TempExc.IsVisible = true;
            ValueVaporizer.IsVisible = true;

            ValueTempOut.Classes.Remove("menu_title");
            ValueTempOut.Classes.Add("menu_subtext");
        }
        else
        {
            HeaderTemp.Text = "Температура Выход";

            TempOut.IsVisible = false;
            TempExc.IsVisible = false;
            ValueVaporizer.IsVisible = false;

            ValueTempOut.Classes.Remove("menu_subtext");
            ValueTempOut.Classes.Add("menu_title");
        }
    }
    private void HandlerRateLineDisplay()
    {
        if (!HeaderRate_A.IsVisible)
        {
            HeaderRate_A.IsVisible = true;
            ValueRate_A.IsVisible = true;
            ValueRate.IsVisible = false;
            HeaderRate_B.IsVisible = true;
            ValueRate_B.IsVisible = true;

            HeaderRate.Text = "Расход";
        }
        else
        {
            HeaderRate_A.IsVisible = false;
            ValueRate_A.IsVisible = false;
            ValueRate.IsVisible = true;
            HeaderRate_B.IsVisible = false;
            ValueRate_B.IsVisible = false;

            HeaderRate.Text = "Σ Расход";
        }
    }
    private static bool IsCommandKey(Key key)
    {
        return key == Key.Escape
            || key == Key.Enter
            || key is >= Key.F1 and <= Key.F12;
    }
    public async void ShowScreen(UserControl screen)
    {
        if (_currentScreen != null)
            _screenStack.Push(_currentScreen);

        if (screen.DataContext is null)
            screen.DataContext = DataContext;

        _currentScreen = screen;
        ScreenHost.Content = screen;
        ScreenHost.IsVisible = true;

        if (screen is IScreenLoadable loadable)
            await loadable.LoadAsync();

        Focus();
    }
    private PressureControl CreatePressureScreen()
    {
        if (MainVm is null
     || _writer is null
     || _connectionConfig is null
     || _holdingRegisters is null)
            return new PressureControl(this);

        var screen = new PressureControl(this)
        {
            DataContext = new PressureViewModel(
                MainVm,
                _writer,
                _connectionConfig,
                _holdingRegisters)
        };

        return screen;
    }

    public void BackScreen()
    {
        if (_screenStack.Count > 0)
        {
            _currentScreen = _screenStack.Pop();
            ScreenHost.Content = _currentScreen;
            ScreenHost.IsVisible = true;
        }
        else
        {
            CloseScreen();
        }

        WindowState = WindowState.FullScreen;
        Focus();
    }
    public void CloseScreen()
    {
        _screenStack.Clear();

        ScreenHost.Content = null;
        ScreenHost.IsVisible = false;
        _currentScreen = null;

        WindowState = WindowState.FullScreen;
        Focus();
    }
}