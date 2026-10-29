
using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Media;
using Nitrogen.Views.MainView;
using ReactiveUI;

namespace Nitrogen.Views.Menu.Pressure;

public sealed class PressureViewModel : ReactiveObject, IDisposable
{
    private readonly MainViewModel _mainVm;

    public string Pressure_1 => _mainVm.Pressure_1;

    public IBrush PressureTileBackground =>
        _mainVm.PressureTileBackground;

    private string _opkoEdit = string.Empty;

    public string OpkoEdit
    {
        get => _opkoEdit;
        set => this.RaiseAndSetIfChanged(ref _opkoEdit, value);
    }

    public PressureViewModel(MainViewModel mainVm)
    {
        _mainVm = mainVm;
        _mainVm.PropertyChanged += MainVmOnPropertyChanged;
    }

    public Task LoadAsync()
    {
        OpkoEdit = _mainVm.Opko_1;
        return Task.CompletedTask;
    }

    // TODO [CoreLink/Command]:
    // Установка нуля датчика давления.
    // Legacy: Pressure_1SetZero = 1.
    public void SetZero()
    {
    }

    // TODO [CoreLink/Command]:
    // Сброс установки нуля.
    // Legacy: Pressure_1SetZero = 0.
    public void ResetZero()
    {
    }

    // TODO [CoreLink/Command]:
    // Установка порога отсечки.
    // Legacy:
    // 1. Запись float в OPKO_1Lo/Hi.
    // 2. Команда SetOpko_1Lo = 1.
    public void SetShutdown()
    {
        if (!float.TryParse(
                OpkoEdit.Replace(',', '.'),
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out float value))
        {
            return;
        }

        // TODO: Передать value через будущий API CoreLink.
    }

    private void MainVmOnPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.Pressure_1))
        {
            this.RaisePropertyChanged(nameof(Pressure_1));
        }

        if (e.PropertyName ==
            nameof(MainViewModel.PressureTileBackground))
        {
            this.RaisePropertyChanged(
                nameof(PressureTileBackground));
        }
    }

    public void Dispose()
    {
        _mainVm.PropertyChanged -= MainVmOnPropertyChanged;
    }
}
