using Nitrogen.Services.SystemTime;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace Nitrogen.Views.MainWindow
{
    internal class MainWindowViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; } = new();

        private readonly ObservableAsPropertyHelper<DateTime> _currentTime;

        public MainWindowViewModel(ISystemTimeService systemTimeService)
        {
            _currentTime = systemTimeService.Ticks
                .ToProperty(this, vm => vm.CurrentTime, initialValue: DateTime.MinValue);

        }
        public DateTime CurrentTime => _currentTime.Value;
    }
}
