
namespace Nitrogen.Views.Menu.Pressure;

public sealed class PressureWriteRequest
{
    public PressureCommand Command { get; }
    public float? Value { get; }

    private PressureWriteRequest(PressureCommand command, float? value = null)
    {
        Command = command;
        Value = value;
    }

    public static PressureWriteRequest SetZero()
        => new(PressureCommand.SetZero);

    public static PressureWriteRequest ResetZero()
        => new(PressureCommand.ResetZero);

    public static PressureWriteRequest SetShutdown(float value)
        => new(PressureCommand.SetShutdown, value);
}