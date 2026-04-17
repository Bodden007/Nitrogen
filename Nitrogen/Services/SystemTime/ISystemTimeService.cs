using System;
using System.Collections.Generic;
using System.Text;

namespace Nitrogen.Services.SystemTime
{
    internal interface ISystemTimeService
    {
        IObservable<DateTime> Ticks { get; }
    }
}
