
using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CoreLink.Adapter.CSharp;
using CoreLink.Client;
using CoreLink.Client.Results;
using Nitrogen.CoreLink;
using Nitrogen.Views.MainWindow;
using Nitrogen.Views.MainView;

namespace Nitrogen;

public partial class App : Application
{
    private CoreLinkClient? _client;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _client = new CoreLinkClient(
            new CSharpCoreAdapter());

        //DEBUG 
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        ////

        var result = _client.Start(
            CoreLinkMapConfiguration.Create());

        //DEBUG 
        stopwatch.Stop();

        Console.WriteLine(
            $"CoreLink Start: {stopwatch.ElapsedMilliseconds} ms, Result: {result}");
        /////

        if (result != CoreLinkResult.Ok)
        {
            System.Diagnostics.Trace.TraceError(
                $"CoreLink Start failed: {result}. {_client.LastError}");
        }

        if (ApplicationLifetime is
            IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(_client)
            };

            desktop.Exit += (_, _) =>
            {
                (desktop.MainWindow?.DataContext
                    as MainViewModel)?.Dispose();

                _client.Stop();
            };
        }
        else if (ApplicationLifetime is
            IActivityApplicationLifetime activity)
        {
            activity.MainViewFactory = () =>
                new MainView
                {
                    DataContext = new MainViewModel(_client)
                };
        }
        else if (ApplicationLifetime is
            ISingleViewApplicationLifetime singleView)
        {
            singleView.MainView = new MainView
            {
                DataContext = new MainViewModel(_client)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
