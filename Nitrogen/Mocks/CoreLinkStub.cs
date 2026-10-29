using System;
using System.Reactive.Linq;

namespace Nitrogen.Mocks;

// Временный источник готовых значений для HMI.
public sealed class CoreLinkStub : ICoreLinkSource
{
    private readonly IObservable<long> _ticks = Observable
        .Interval(TimeSpan.FromMilliseconds(200))
        .Publish()
        .RefCount();

    public IObservable<RxValue> Value(ValueId id) =>
        _ticks
            .Select(tick => CreateValue(id, tick))
            .DistinctUntilChanged();

    private static RxValue CreateValue(ValueId id, long tick)
    {
        // Имитация переключения системы измерений каждые 10 секунд.
        bool metric = (tick / 50) % 2 == 1;

        float variation =
            (float)Math.Sin(tick * 0.1) * 3f;

        return id switch
        {
            ValueId.Pressure_1 => new(
                metric ? 103.4f + variation : 1500f + variation,
                "Pressure",
                metric ? "bar" : "psi"),

            ValueId.Opko_1 => new(
                metric ? 124.1f : 1800f,
                "Cutoff",
                metric ? "bar" : "psi"),

            ValueId.TemperatureOutlet => new(
                metric ? 36f + variation : 96f + variation,
                "Outlet temperature",
                metric ? "°C" : "°F"),

            ValueId.TemperatureVaporizer => new(
                metric ? 29f + variation : 85f + variation,
                "Vaporizer temperature",
                metric ? "°C" : "°F"),

            ValueId.TemperatureBath => new(
                metric ? 24f + variation : 75f + variation,
                "Bath temperature",
                metric ? "°C" : "°F"),

            ValueId.Pump1Rpm => new(
                1200f + variation,
                "Pump 1 speed",
                "rpm"),

            ValueId.Pump1ScfStage => new(
                3500f + tick,
                "Pump 1 stage volume",
                metric ? "m³" : "scf"),

            ValueId.Pump1TotalScfm => new(
                metric ? 42f + variation : 1500f + variation,
                "Pump 1 flow",
                metric ? "m³/min" : "scfm"),

            ValueId.CementVolumeTotal => new(
                metric ? 17.5f : 110f,
                "Total cement volume",
                metric ? "m³" : "bbl"),

            ValueId.CementVolumeDs => new(
                metric ? 9.5f : 60f,
                "DS cement volume",
                metric ? "m³" : "bbl"),

            ValueId.CementVolumePs => new(
                metric ? 8f : 50f,
                "PS cement volume",
                metric ? "m³" : "bbl"),

            ValueId.TotalScfStage => new(
                7000f + tick,
                "Total stage volume",
                metric ? "m³" : "scf"),

            ValueId.Pump1ScfJob => new(
                12000f + tick,
                "Pump 1 job volume",
                metric ? "m³" : "scf"),

            ValueId.TotalScfJob => new(
                24000f + tick,
                "Total job volume",
                metric ? "m³" : "scf"),

            _ => throw new ArgumentOutOfRangeException(
                nameof(id), id, null)
        };
    }
}