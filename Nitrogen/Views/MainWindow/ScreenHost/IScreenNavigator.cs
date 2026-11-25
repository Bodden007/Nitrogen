using System;
using Avalonia.Controls;

namespace Nitrogen.Views.MainWindow.ScreenHost;

internal interface IScreenNavigator : IDisposable
{
    void ShowScreen(
        UserControl screen,
        IDisposable? ownedResource = null);

    void BackScreen();

    void CloseScreen();
}