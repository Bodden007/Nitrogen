using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Media;
using Nitrogen.Views.MainView;
using ReactiveUI;
using CoreLink.Client;
using CoreLink.Contracts.Core;

namespace Nitrogen.Views.Menu.Pressure;

public sealed class PressureViewModel : ReactiveObject, IDisposable
{
    private readonly MainViewModel _mainVm;

    public string Pressure_1 => _mainVm.Pressure_1;

    private readonly CoreLinkClient _client;

    public IBrush PressureTileBackground =>
        _mainVm.PressureTileBackground;

    private string _opkoEdit = string.Empty;

    public string OpkoEdit
    {
        get => _opkoEdit;
        set => this.RaiseAndSetIfChanged(ref _opkoEdit, value);
    }

    public PressureViewModel(
        MainViewModel mainVm,
        CoreLinkClient client)
    {
        _mainVm = mainVm;
        _client = client;

        _mainVm.PropertyChanged += MainVmOnPropertyChanged;
    }

    public Task LoadAsync()
    {
        OpkoEdit = _mainVm.Opko_1Value is float value
            ? value.ToString("0.##", CultureInfo.InvariantCulture)
            : string.Empty;

        return Task.CompletedTask;
    }

    // TODO [CoreLink/Command]:
    // Установка нуля датчика давления.
    // Legacy: Pressure_1SetZero = 1.
    public void SetZero()
    {
    }

    // FIXME: Согласовать контракт адресации CoreLink.
    public Task SetZeroAsync()
    {
        return _client.WriteWordAsync(16, 1);
    }

    // FIXME: Согласовать контракт адресации CoreLink.
    public Task ResetZeroAsync()
    {
        return _client.WriteWordAsync(16, 0);
    }
    public async Task SetShutdownAsync()
    {
        if (!float.TryParse(
                OpkoEdit.Replace(',', '.'),
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out float value))
        {
            return;
        }

        // Запись уставки ОПКО.
        await _client.WriteFloatAsync(8, value);

        // NOTE: Не отправляем команду применения,
        // если запись уставки завершилась ошибкой.
        if (_client.LastWriteStatus != CoreWriteStatus.Ok)
            return;

        // Применение уставки.
        await _client.WriteWordAsync(10, 1);
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
