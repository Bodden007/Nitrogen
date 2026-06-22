using Avalonia.Controls;

namespace Nitrogen.Views.MainWindow.ScreenHost;

internal interface IScreenNavigator
{
    void ShowScreen(UserControl screen);

    void BackScreen();

    void CloseScreen();
}