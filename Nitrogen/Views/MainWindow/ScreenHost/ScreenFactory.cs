using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using Nitrogen.Services.Modbus.Connection;
using Nitrogen.Views.Menu;
using Nitrogen.Views.Menu.Engine;
using Nitrogen.Views.Menu.Pressure;
using Nitrogen.Views.Menu.SCFVolume;
using Nitrogen.Views.Settings;
using Nitrogen.Views.Settings.PressureSet;
using Nitrogen.Views.Settings.SCFSet;
using System.Collections.Generic;

namespace Nitrogen.Views.MainWindow.ScreenHost;

internal sealed class ScreenFactory : IScreenFactory
{
    private readonly Nitrogen.MainWindow _mainWindow;
    private readonly IModbusWriter? _writer;
    private readonly IModbusReader? _reader;
    private readonly ModbusConnectionConfig? _connectionConfig;
    private readonly IReadOnlyList<ModbusRegisterConfig>? _inputRegisters;
    private readonly IReadOnlyList<ModbusRegisterConfig>? _holdingRegisters;

    private MainWindowViewModel? MainVm =>
        _mainWindow.DataContext as MainWindowViewModel;

    public ScreenFactory(
        Nitrogen.MainWindow mainWindow,
        IModbusWriter? writer,
        IModbusReader? reader,
        ModbusConnectionConfig? connectionConfig,
        IReadOnlyList<ModbusRegisterConfig>? inputRegisters,
        IReadOnlyList<ModbusRegisterConfig>? holdingRegisters)
    {
        _mainWindow = mainWindow;
        _writer = writer;
        _reader = reader;
        _connectionConfig = connectionConfig;
        _inputRegisters = inputRegisters;
        _holdingRegisters = holdingRegisters;
    }

    public PressureControl CreatePressureScreen()
    {
        if (MainVm is null
            || _writer is null
            || _connectionConfig is null
            || _holdingRegisters is null)
            return new PressureControl(_mainWindow);

        return new PressureControl(_mainWindow)
        {
            DataContext = new PressureViewModel(
                MainVm,
                _writer,
                _connectionConfig,
                _holdingRegisters)
        };
    }

    public EngineControl CreateEngineScreen()
    {
        if (MainVm is null)
            return new EngineControl(_mainWindow);

        return new EngineControl(_mainWindow)
        {
            DataContext = new EngineViewModel(MainVm)
        };
    }

    public PressureSetControl CreatePressureSetScreen()
    {
        if (MainVm is null
            || _reader is null
            || _writer is null
            || _connectionConfig is null
            || _inputRegisters is null
            || _holdingRegisters is null)
            return new PressureSetControl(_mainWindow);

        return new PressureSetControl(_mainWindow)
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

    public SCFSetControl CreateSCFSetScreen()
    {
        if (MainVm is null
            || _reader is null
            || _writer is null
            || _connectionConfig is null
            || _inputRegisters is null
            || _holdingRegisters is null)
            return new SCFSetControl(_mainWindow);

        return new SCFSetControl(_mainWindow)
        {
            DataContext = new SCFSetControlViewModel(
                MainVm,
                _reader,
                _writer,
                _connectionConfig,
                _inputRegisters,
                _holdingRegisters)
        };
    }
    public ScfVolumeControl CreateScfVolumeScreen()
    {
        if (_reader is null
            || _connectionConfig is null
            || _inputRegisters is null)
            return new ScfVolumeControl(_mainWindow);

        return new ScfVolumeControl(_mainWindow)
        {
            DataContext = new ScfVolumeControlViewModel(
                _reader,
                _connectionConfig,
                _inputRegisters)
        };
    }
}