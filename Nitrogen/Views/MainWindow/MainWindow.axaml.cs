
using Avalonia.Controls;
using Avalonia.Input;

namespace Nitrogen.Views.MainWindow;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        KeyDown += MainWindow_KeyDown;
    }

    private void MainWindow_KeyDown(object? sender, KeyEventArgs e)
    {
        if (Content is Nitrogen.Views.MainView.MainView mainView)
        {
            mainView.HandleKeyDown(e);
        }
    }
}
