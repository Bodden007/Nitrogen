
using CoreLink.Contracts.Maps;
using CoreLink.Contracts.Registers;

namespace Nitrogen.CoreLink;

public static class CoreLinkMapConfiguration
{
    public static CoreLinkMap Create()
    {
        return new CoreLinkMap
        {
            IpAddress = "127.0.0.1",
            Port = 1502,
            UnitId = 255,

            PollFunctionCode = 4,
            PollIntervalMs = 500,
            RxIntervalMs = 500,

            ConnectTimeoutMs = 3000,
            RequestTimeoutMs = 1000,
            ReconnectDelayMs = 1000,

            RegisterOrder = RegisterOrder.ABCD,
            SupportedUnitSystemIds = [1, 2],

            Registers =
            [
                new("Measurme", 1, RegisterType.Word),

                new("Pressure_1", 8, RegisterType.Float,
                    new Units(
                        new UnitEntry(1, "psi"),
                        new UnitEntry(2, "bar"))),

                new("Opko_1", 12, RegisterType.Float,
                    new Units(
                        new UnitEntry(1, "psi"),
                        new UnitEntry(2, "bar"))),

                new("StatOpko_1", 14, RegisterType.Word),

                new("TemperatureOutlet", 20, RegisterType.Float,
                    new Units(
                        new UnitEntry(1, "°C"),
                        new UnitEntry(2, "°C"))),

                new("TemperatureVaporizer", 22, RegisterType.Float,
                    new Units(
                        new UnitEntry(1, "°C"),
                        new UnitEntry(2, "°C"))),

                new("TemperatureBath", 24, RegisterType.Float,
                    new Units(
                        new UnitEntry(1, "°C"),
                        new UnitEntry(2, "°C"))),

                new("PUMP_1RPM", 30, RegisterType.Float,
                    new Units(
                        new UnitEntry(1, "rpm"),
                        new UnitEntry(2, "об/мин"))),

                new("PUMP_1SCFStage", 34, RegisterType.Float,
                    new Units(
                        new UnitEntry(1, "scf"),
                        new UnitEntry(2, "нм³"))),

                new("PUMP_1TotalSCFM", 42, RegisterType.Float,
                    new Units(
                        new UnitEntry(1, "scfm"),
                        new UnitEntry(2, "нм³/мин"))),


            ]
        };
    }
}
