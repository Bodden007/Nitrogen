using Nitrogen.Services.Modbus.Configuration.Models.Connection;
using Nitrogen.Services.Modbus.Configuration.Models.Registers;
using Nitrogen.Services.Modbus.Connection;
using Nitrogen.Services.Modbus.Mapping.RegisterHelper;
using Nitrogen.Views.MainWindow;
using NModbus.Utility;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Nitrogen.Views.Menu.Engine;

internal sealed class EngineViewModel : ReactiveObject
{
    private readonly ObservableAsPropertyHelper<double> _pump1Rpm;
    private readonly IModbusReader _reader;
    private readonly IModbusWriter _writer;
    private readonly ModbusConnectionConfig _connectionConfig;
    private readonly IReadOnlyList<ModbusRegisterConfig> _inputRegisters;
    private readonly IReadOnlyList<ModbusRegisterConfig> _holdingRegisters;

    public ReactiveCommand<Unit, Unit> NeutralCommand { get; }
    public ReactiveCommand<Unit, Unit> UpCommand { get; }
    public ReactiveCommand<Unit, Unit> DownCommand { get; }

    public EngineViewModel(
        MainWindowViewModel mainWindowViewModel,
        IModbusReader reader,
        IModbusWriter writer,
        ModbusConnectionConfig connectionConfig,
        IReadOnlyList<ModbusRegisterConfig> inputRegisters,
        IReadOnlyList<ModbusRegisterConfig> holdingRegisters)
    {
        _reader = reader;
        _writer = writer;
        _connectionConfig = connectionConfig;
        _inputRegisters = inputRegisters;
        _holdingRegisters = holdingRegisters;

        _pump1Rpm = mainWindowViewModel
            .WhenAnyValue(x => x.Pump1Rpm)
            .ToProperty(this, x => x.Pump1Rpm);

        NeutralCommand = ReactiveCommand.CreateFromTask(NeutralAsync);
        UpCommand = ReactiveCommand.CreateFromTask(UpAsync);
        DownCommand = ReactiveCommand.CreateFromTask(DownAsync);
    }

    public double Pump1Rpm => _pump1Rpm.Value;

    private double _pump1Hours;
    public double Pump1Hours
    {
        get => _pump1Hours;
        private set => this.RaiseAndSetIfChanged(ref _pump1Hours, value);
    }

    public async Task LoadAsync()
    {
        var startAddress = (ushort)ModbusRegisterHelper
            .GetRegister(_inputRegisters, "Pump1HoursLo")
            .Address;

        var registers = await _reader.ReadInputRegistersAsync(
            _connectionConfig.SlaveId,
            startAddress,
            2);

        Pump1Hours = ModbusUtility.GetSingle(registers[1], registers[0]);
    }
    private async Task NeutralAsync()
    {
        await SendTransmissionCommandAsync(1);
    }

    private async Task UpAsync()
    {
        await SendTransmissionCommandAsync(2);
    }

    private async Task DownAsync()
    {
        await SendTransmissionCommandAsync(3);
    }

    private async Task SendTransmissionCommandAsync(ushort command)
    {
        await _writer.WriteSingleRegisterAsync(
            _connectionConfig.SlaveId,
            GetHoldingAddress("TransmissionCommand"),
            command);
    }

    private ushort GetHoldingAddress(string name)
    {
        return (ushort)_holdingRegisters
            .First(x => x.Name == name)
            .Address;
    }
}