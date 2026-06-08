using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Nitrogen.Services.Modbus.Polling;
using Nitrogen.Services.Modbus.Rx;
using Nitrogen.Services.SystemTime;
using Nitrogen.Views.MainWindow;
using System;

namespace Nitrogen;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            //desktop.MainWindow = new MainWindow();
            var timeService = new SystemTimeService(TimeSpan.FromSeconds(1));

            var poller = new ModbusPoller();
            var modbusRxService = new ModbusRxService(poller);

            var viewModel = new MainWindowViewModel(
               timeService,
               modbusRxService);

            desktop.MainWindow = new MainWindow { DataContext = viewModel };
        }

        base.OnFrameworkInitializationCompleted();
    }
}