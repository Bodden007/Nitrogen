using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Nitrogen.Views.MainView;
using ReactiveUI;

namespace Nitrogen.Views.Menu.Engine;

public sealed class EngineViewModel : ReactiveObject, IDisposable
{
    private readonly MainViewModel _mainVm;

    public EngineViewModel(MainViewModel mainVm)
    {
        _mainVm = mainVm;
        _mainVm.PropertyChanged += MainVmOnPropertyChanged;
    }

    public double Pump1Rpm => _mainVm.Pump1Rpm;

    private double _pump1Hours;

    public double Pump1Hours
    {
        get => _pump1Hours;
        private set => this.RaiseAndSetIfChanged(
            ref _pump1Hours, value);
    }

    public Task LoadAsync()
    {
        // TODO [CoreLink/Read]:
        // Чтение Pump1HoursLo/Hi.
        // Legacy: два регистра Input Registers,
        // декодирование Float32.

        return Task.CompletedTask;
    }

    public void Neutral()
    {
        SendTransmissionCommand(1);
    }

    public void Up()
    {
        SendTransmissionCommand(2);
    }

    public void Down()
    {
        SendTransmissionCommand(3);
    }

    private void SendTransmissionCommand(ushort command)
    {
        // TODO [CoreLink/Command]:
        // Запись TransmissionCommand.
        // 1 — нейтраль.
        // 2 — передача вверх.
        // 3 — передача вниз.
    }

    private void MainVmOnPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.Pump1Rpm))
            this.RaisePropertyChanged(nameof(Pump1Rpm));
    }

    public void Dispose()
    {
        _mainVm.PropertyChanged -= MainVmOnPropertyChanged;
    }
}
