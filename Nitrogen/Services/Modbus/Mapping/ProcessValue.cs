namespace Nitrogen.Services.Modbus.Mapping;

internal sealed class ProcessValue
{
    public string Name { get; init; } = string.Empty;

    public double Value { get; init; }

    public bool HasError { get; init; }

    public string? ErrorText { get; init; }
}