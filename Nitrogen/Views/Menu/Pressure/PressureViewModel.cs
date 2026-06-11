using Nitrogen.Services.Modbus.Connection;
using Nitrogen.Views.MainWindow;
using ReactiveUI;
using System.Globalization;
using System.Reactive;
using System.Threading.Tasks;

namespace Nitrogen.Views.Menu.Pressure;

internal sealed class PressureViewModel : ReactiveObject
{
    private readonly MainWindowViewModel _mainVm;
    private readonly IModbusWriter _writer;

    public string Pressure_1 => _mainVm.Pressure_1;

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
        IModbusWriter writer)
    {
        _mainVm = mainVm;
        _writer = writer;

        _mainVm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(MainWindowViewModel.Pressure_1))
                this.RaisePropertyChanged(nameof(Pressure_1));
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
        var request = PressureWriteRequest.SetZero();

        // TODO: отправить в PLC holding = 0

        return Task.CompletedTask;
    }

    private Task ResetZeroAsync()
    {
        var request = PressureWriteRequest.ResetZero();

        // TODO: отправить в PLC holding = 1

        return Task.CompletedTask;
    }

    private Task SetShutdownAsync()
    {
        if (!float.TryParse(
                OpkoEdit.Replace(',', '.'),
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var value))
            return Task.CompletedTask;

        var request = PressureWriteRequest.SetShutdown(value);

        // TODO: отправить в PLC holding = value
        // После записи не обновляем TextBox вручную.
        // Подтвержденное значение должно прийти обратно через Input Register.

        return Task.CompletedTask;
    }
}