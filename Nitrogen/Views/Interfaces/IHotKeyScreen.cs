using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Input;

namespace Nitrogen.Views.Interfaces
{
    internal interface IHotKeyScreen
    {
        void HandleKey(Key key);
    }
}
