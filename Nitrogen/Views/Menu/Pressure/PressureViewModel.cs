using Avalonia.Media;
using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using Nitrogen.Services.Modbus.Connection;
using Nitrogen.Views.MainWindow;
using NModbus.Utility;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace Nitrogen.Views.Menu.Pressure;

internal sealed class PressureViewModel : ReactiveObject
{
    private readonly MainWindowViewModel _mainVm;
    private readonly IModbusWriter _writer;
    private readonly ModbusConnectionConfig _connectionConfig;
    private readonly IReadOnlyList<ModbusRegisterConfig> _holdingRegisters;

    public string Pressure_1 => _mainVm.Pressure_1;
    public IBrush PressureTileBackground => _mainVm.PressureTileBackground;

    private string _opkoEdit = "";

    public string OpkoEdit
    {
        get => _opkoEdit;
        set => this.RaiseAndSetIfChanged(ref _opkoEdit, value);
    }

    public ReactiveCommand<Unit, Unit> SetZeroCommand { get; }
    public ReactiveCommand<Unit, Unit> ResetZeroCommand { get; }
    public ReactiveCommand<Unit, Unit> SetShutdownCommand { get; }

    internal PressureViewModel(
        MainWindowViewModel mainVm,
        IModbusWriter writer,
        ModbusConnectionConfig connectionConfig,
        IReadOnlyList<ModbusRegisterConfig> holdingRegisters)
    {
        _mainVm = mainVm;
        _writer = writer;
        _connectionConfig = connectionConfig;
        _holdingRegisters = holdingRegisters;

        _mainVm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(MainWindowViewModel.Pressure_1))
                this.RaisePropertyChanged(nameof(Pressure_1));

            if (e.PropertyName == nameof(MainWindowViewModel.PressureTileBackground))
                this.RaisePropertyChanged(nameof(PressureTileBackground));
        };

        SetZeroCommand = ReactiveCommand.CreateFromTask(SetZeroAsync);
        ResetZeroCommand = ReactiveCommand.CreateFromTask(ResetZeroAsync);
        SetShutdownCommand = ReactiveCommand.CreateFromTask(SetShutdownAsync);
    }

    public Task LoadAsync()
    {
        OpkoEdit = _mainVm.Opko_1;
        return Task.CompletedTask;
    }

    private Task SetZeroAsync()
    {
        ushort address = GetHoldingAddress("Pressure_1SetZero");

        return _writer.WriteSingleRegisterAsync(
            _connectionConfig.SlaveId,
            address,
            1);
    }

    private Task ResetZeroAsync()
    {
        ushort address = GetHoldingAddress("Pressure_1SetZero");

        return _writer.WriteSingleRegisterAsync(
            _connectionConfig.SlaveId,
            address,
            0);
    }

    private async Task SetShutdownAsync()
    {
        if (!float.TryParse(
                OpkoEdit.Replace(',', '.'),
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var value))
            return;

        // 1. Сначала записываем новое значение OPKO
        ushort opkoAddress = GetHoldingAddress("OPKO_1Lo");

        await _writer.WriteMultipleRegistersAsync(
            _connectionConfig.SlaveId,
            opkoAddress,
            FloatToRegisters(value));

        // 2. Потом даем команду сброса/перечитать OPKO
        ushort resetAddress = GetHoldingAddress("SetOpko_1Lo");

        await _writer.WriteSingleRegisterAsync(
            _connectionConfig.SlaveId,
            resetAddress,
            1);
    }
    private static ushort[] FloatToRegisters(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);

        ushort lo = BitConverter.ToUInt16(bytes, 0);
        ushort hi = BitConverter.ToUInt16(bytes, 2);

        return [lo, hi];
    }

    private ushort GetHoldingAddress(string name)
    {
        return (ushort)_holdingRegisters
            .First(x => x.Name == name)
            .Address;
    }
}