using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using Nitrogen.Services.Modbus.Connection;
using Nitrogen.Views.Interfaces;
using Nitrogen.Views.MainWindow;
using Nitrogen.Views.MainWindow.ScreenHost;
using Nitrogen.Views.Menu;
using Nitrogen.Views.Menu.Engine;
using Nitrogen.Views.Menu.Pressure;
using Nitrogen.Views.Settings;
using Nitrogen.Views.Settings.PressureSet;
using System.Collections.Generic;

namespace Nitrogen;

public partial class MainWindow : Window
{
    private readonly Stack<UserControl> _screenStack = new();
    private readonly IModbusWriter? _writer;
    private readonly IModbusReader? _reader;

    private readonly ModbusConnectionConfig? _connectionConfig;
    private readonly IReadOnlyList<ModbusRegisterConfig>? _holdingRegisters;
    private readonly IReadOnlyList<ModbusRegisterConfig>? _inputRegisters;

    private readonly ScreenNavigator _screenNavigator;

    private UserControl? _currentScreen;

    private MainWindowViewModel? MainVm => DataContext as MainWindowViewModel;
    public MainWindow()
    {
        InitializeComponent();

        WindowState = WindowState.FullScreen;

        _screenNavigator = new ScreenNavigator(ScreenHost);

        this.KeyDown += MainWindow_KeyDown;
    }
    internal MainWindow(
        IModbusWriter writer,
        ModbusConnectionConfig connectionConfig,
        IReadOnlyList<ModbusRegisterConfig> inputRegisters,
        IReadOnlyList<ModbusRegisterConfig> holdingRegisters) : this()
    {
        _writer = writer;
        _reader = writer as IModbusReader;
        _connectionConfig = connectionConfig;
        _inputRegisters = inputRegisters;
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
                    ShowScreen(CreateEngineScreen());
                    e.Handled = true;
                    break;
                }

            case Key.F6:
                {
                    ShowScreen(new Nitrogen.Views.Menu.RecordControl(this));
                    e.Handled = true;
                    break;
                }

                //TODO доделать и раскоментировать
            //case Key.F7:
            //    {
            //        ShowScreen(new Nitrogen.Views.Menu.StagesControl(this));
            //        e.Handled = true;
            //        break;
            //    }

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
        ShowScreen(CreateEngineScreen());
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
        //TODO доделать и раскоментировать
        //ShowScreen(new Nitrogen.Views.Menu.StagesControl(this));
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

    public void ShowPressureScreen()
    {
        ShowScreen(CreatePressureScreen());
    }

    private EngineControl CreateEngineScreen()
    {
        if (MainVm is null)
            return new Nitrogen.Views.Menu.EngineControl(this);

        return new Nitrogen.Views.Menu.EngineControl(this)
        {
            DataContext = new EngineViewModel(MainVm)
        };
    }
    public void ShowEngineScreen()
    {
        ShowScreen(CreateEngineScreen());
    }

    internal PressureSetControl CreatePressureSetScreen()
    {
        if (MainVm is null
            || _reader is null
            || _writer is null
            || _connectionConfig is null
            || _inputRegisters is null
            || _holdingRegisters is null)
            return new PressureSetControl(this);

        return new PressureSetControl(this)
        {
            DataContext = new PressureSetControlViewModel(
                MainVm,
                _reader,
                _writer,
                _connectionConfig,
                _inputRegisters,
                _holdingRegisters)
        };
    }

    public async void ShowScreen(UserControl screen)
    {
        _screenNavigator.ShowScreen(screen);
        _currentScreen = screen;
        ScreenHost.IsVisible = true;
        Focus();
    }
    public void BackScreen()
    {
        _screenNavigator.BackScreen();
        _currentScreen = ScreenHost.Content as UserControl;
        ScreenHost.IsVisible = ScreenHost.Content is not null;
        WindowState = WindowState.FullScreen;
        Focus();
    }
    public void CloseScreen()
    {
        _screenNavigator.CloseScreen();
        _currentScreen = null;
        ScreenHost.IsVisible = false;
        WindowState = WindowState.FullScreen;
        Focus();
    }
}