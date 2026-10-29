
using System;
using Avalonia.Media;
using ReactiveUI;

namespace Nitrogen.Views.MainView;

public class MainViewModel : ReactiveObject
{
    // TODO: Подключить обновление данных из CoreLink.

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

    private double _pump1Rpm = 1200;

    public double Pump1Rpm
    {
        get => _pump1Rpm;
        private set => this.RaiseAndSetIfChanged(
            ref _pump1Rpm, value);
    }

    private double _pump1ScfStage = 3500;

    public double Pump1ScfStage
    {
        get => _pump1ScfStage;
        private set => this.RaiseAndSetIfChanged(
            ref _pump1ScfStage, value);
    }

    private double _pump1TotalScfm = 1500;

    public double Pump1TotalScfm
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
