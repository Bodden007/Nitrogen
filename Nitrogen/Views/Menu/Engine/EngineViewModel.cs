using Nitrogen.Views.MainWindow;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace Nitrogen.Views.Menu.Engine;

internal sealed class EngineViewModel : ReactiveObject
{
    private readonly ObservableAsPropertyHelper<double> _pump1Rpm;

    public EngineViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _pump1Rpm = mainWindowViewModel
            .WhenAnyValue(x => x.Pump1Rpm)
            .ToProperty(this, x => x.Pump1Rpm);
    }

    public double Pump1Rpm => _pump1Rpm.Value;
}