using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;

namespace Nitrogen.Services.SystemTime
{
    internal class SystemTimeService : ISystemTimeService
    {
        private readonly IObservable<DateTime> _ticks;

        public SystemTimeService(TimeSpan? interval = null)
        {
            var tickInterval = interval ?? TimeSpan.FromSeconds(1);

            _ticks = Observable
                .Interval(tickInterval)
                .Select(_ => DateTime.Now)
                .ObserveOn(RxSchedulers.MainThreadScheduler) // UI-поток, без этого нельзя
                .Publish()
                .RefCount(); // Один таймер на всех подписчиков
        }

        public IObservable<DateTime> Ticks => _ticks;

    }
}
