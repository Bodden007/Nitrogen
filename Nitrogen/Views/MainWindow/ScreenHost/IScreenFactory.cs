using Nitrogen.Views.Menu;
using Nitrogen.Views.Menu.Engine;
using Nitrogen.Views.Menu.Pressure;
using Nitrogen.Views.Settings;
using Nitrogen.Views.Settings.PressureSet;

namespace Nitrogen.Views.MainWindow.ScreenHost;

internal interface IScreenFactory
{
    PressureControl CreatePressureScreen();

    EngineControl CreateEngineScreen();

    PressureSetControl CreatePressureSetScreen();

    SCFSetControl CreateSCFSetScreen();
}