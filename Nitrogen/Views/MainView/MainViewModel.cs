using System;
using Avalonia.Media;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI.Primitives.Signals;
using CoreLink.Client;
using CoreLink.Rx.Runtime;
using Nitrogen.CoreLink;

namespace Nitrogen.Views.MainView;

public class MainViewModel : ReactiveObject
{
    // TODO: Подключить обновление данных из CoreLink.

    private readonly CompositeDisposable _subscriptions = new();

    public MainViewModel(CoreLinkClient client)
    {
        // NOTE: Единица измерения определяется CoreLink.
        // Отображение текущего давления.
        Subscribe(client, ValueId.Pressure_1,
            value => Pressure_1 = string.IsNullOrEmpty(value.Unit)
                ? value.Value.ToString("0.##")
                : $"{value.Value:0.##} {value.Unit}");

        // Отображение порога отсечки.
        Subscribe(client, ValueId.Opko_1,
            value => Opko_1 = string.IsNullOrEmpty(value.Unit)
                ? value.Value.ToString("0.##")
                : $"{value.Value:0.##} {value.Unit}");

        // Аварийная индикация по статусу ПЛК.
        Subscribe(client, ValueId.StatOpko_1,
            value => UpdateOpkoAlarm(value.Value));

        // Отображение температуры выхода.
        Subscribe(client, ValueId.TemperatureOutlet,
           value => TemperatureOutlet = string.IsNullOrEmpty(value.Unit)
               ? value.Value.ToString("0.##")
               : $"{value.Value:0.##} {value.Unit}");

        // Отображение температуры вапорайзера.
        Subscribe(client, ValueId.TemperatureVaporizer,
            value => TemperatureVaporizer = string.IsNullOrEmpty(value.Unit)
                ? value.Value.ToString("0.##")
                : $"{value.Value:0.##} {value.Unit}");

        // Отображение температуры бани.
        Subscribe(client, ValueId.TemperatureBath,
            value => TemperatureBath = string.IsNullOrEmpty(value.Unit)
                ? value.Value.ToString("0.##")
                : $"{value.Value:0.##} {value.Unit}");

        // NOTE: Обороты насоса.
        // Числовое значение используется тахометром,
        // строковое — для отображения с единицей измерения.
        Subscribe(client, ValueId.PUMP_1RPM, value =>
        {
            Pump1Rpm = value.Value;

            Pump1RpmText = string.IsNullOrEmpty(value.Unit)
                ? value.Value.ToString("0")
                : $"{value.Value:0} {value.Unit}";
        });

        // NOTE: Объём азота за текущую стадию.
        // Единица измерения определяется CoreLink.
        Subscribe(client, ValueId.PUMP_1SCFStage,
            value => Pump1ScfStage = string.IsNullOrEmpty(value.Unit)
                ? value.Value.ToString("0.###")
                : $"{value.Value:0.###} {value.Unit}");

        // NOTE: Текущий расход азота.
        // Единица измерения определяется CoreLink
        // в соответствии с системой измерения ПЛК.
        Subscribe(client, ValueId.PUMP_1TotalSCFM,
            value => Pump1TotalScfm = string.IsNullOrEmpty(value.Unit)
                ? value.Value.ToString("0.###")
                : $"{value.Value:0.###} {value.Unit}");
    }

    private void Subscribe(
        CoreLinkClient client,
        ValueId id,
        Action<RxValue> update)
    {
        _subscriptions.Add(
            client.ObserveValue((int)id)
    .WitnessOn(RxSchedulers.MainThreadScheduler)
    .Subscribe(update));
    }

    public void Dispose()
    {
        _subscriptions.Dispose();
    }

    private DateTime _currentTime = DateTime.Now;

    public DateTime CurrentTime
    {
        get => _currentTime;
        private set => this.RaiseAndSetIfChanged(
            ref _currentTime, value);
    }

    private ushort[] _registers = new ushort[50];

    public ushort[] Registers
    {
        get => _registers;
        private set => this.RaiseAndSetIfChanged(
            ref _registers, value);
    }

    private string _pressure1 = "1500";

    public string Pressure_1
    {
        get => _pressure1;
        private set => this.RaiseAndSetIfChanged(
            ref _pressure1, value);
    }

    private string _opko1 = "1800";

    public string Opko_1
    {
        get => _opko1;
        private set => this.RaiseAndSetIfChanged(
            ref _opko1, value);
    }

    private string _temperatureOutlet = "96";

    public string TemperatureOutlet
    {
        get => _temperatureOutlet;
        private set => this.RaiseAndSetIfChanged(
            ref _temperatureOutlet, value);
    }

    private string _temperatureVaporizer = "85";

    public string TemperatureVaporizer
    {
        get => _temperatureVaporizer;
        private set => this.RaiseAndSetIfChanged(
            ref _temperatureVaporizer, value);
    }

    private string _temperatureBath = "75";

    public string TemperatureBath
    {
        get => _temperatureBath;
        private set => this.RaiseAndSetIfChanged(
            ref _temperatureBath, value);
    }

    // NOTE: Отображение оборотов с единицей измерения.
    private string _pump1RpmText = string.Empty;

    public string Pump1RpmText
    {
        get => _pump1RpmText;
        private set => this.RaiseAndSetIfChanged(
            ref _pump1RpmText, value);
    }

    // Заменить фактическими данными двигателя из CoreLink.
    private double _pump1Rpm;

    public double Pump1Rpm
    {
        get => _pump1Rpm;
        private set => this.RaiseAndSetIfChanged(
            ref _pump1Rpm, value);
    }

    // NOTE: Форматированное значение объёма азота за стадию.
    private string _pump1ScfStage = string.Empty;

    public string Pump1ScfStage
    {
        get => _pump1ScfStage;
        private set => this.RaiseAndSetIfChanged(
            ref _pump1ScfStage, value);
    }

    // NOTE: Форматированное значение текущего расхода азота.
    // Используется исключительно для отображения в HMI.
    private string _pump1TotalScfm = string.Empty;

    public string Pump1TotalScfm
    {
        get => _pump1TotalScfm;
        private set => this.RaiseAndSetIfChanged(
            ref _pump1TotalScfm, value);
    }

    private IBrush _pressureTileBackground =
        Brushes.Transparent;

    public IBrush PressureTileBackground
    {
        get => _pressureTileBackground;
        private set => this.RaiseAndSetIfChanged(
            ref _pressureTileBackground, value);
    }

    private IBrush _pressureHeaderBackground =
        Brushes.Transparent;

    public IBrush PressureHeaderBackground
    {
        get => _pressureHeaderBackground;
        private set => this.RaiseAndSetIfChanged(
            ref _pressureHeaderBackground, value);
    }

    private IBrush _pressureOpkoBackground =
        new SolidColorBrush(Color.Parse("#FBA9A3"));

    public IBrush PressureOpkoBackground
    {
        get => _pressureOpkoBackground;
        private set => this.RaiseAndSetIfChanged(
            ref _pressureOpkoBackground, value);
    }

    // Логика тревоги сохранена из Legacy.
    // TODO: Передавать StatOpko_1 из CoreLink.
    public void UpdateOpkoAlarm(double statOpko)
    {
        PressureTileBackground = statOpko == 1
            ? Brushes.Red
            : Brushes.Transparent;
    }


    // TODO: Получать фактические объёмы цемента из CoreLink.
    // Пока используются демонстрационные значения из Legacy.

    private string _cementVolumeTotal = "110";

    public string CementVolumeTotal
    {
        get => _cementVolumeTotal;
        set => this.RaiseAndSetIfChanged(
            ref _cementVolumeTotal, value);
    }

    private string _cementVolumeDs = "60";

    public string CementVolumeDs
    {
        get => _cementVolumeDs;
        set => this.RaiseAndSetIfChanged(
            ref _cementVolumeDs, value);
    }

    private string _cementVolumePs = "50";

    public string CementVolumePs
    {
        get => _cementVolumePs;
        set => this.RaiseAndSetIfChanged(
            ref _cementVolumePs, value);
    }

    private double _totalScfStage = 7000;

    public double TotalScfStage
    {
        get => _totalScfStage;
        private set => this.RaiseAndSetIfChanged(
            ref _totalScfStage, value);
    }

    private double _pump1ScfJob = 12000;

    public double Pump1ScfJob
    {
        get => _pump1ScfJob;
        private set => this.RaiseAndSetIfChanged(
            ref _pump1ScfJob, value);
    }

    private double _totalScfJob = 24000;

    public double TotalScfJob
    {
        get => _totalScfJob;
        private set => this.RaiseAndSetIfChanged(
            ref _totalScfJob, value);
    }

}
