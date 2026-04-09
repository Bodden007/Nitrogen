using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace Nitrogen;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;

        this.KeyDown += MainWindow_KeyDown;
    }

    private void MainWindow_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.F1: new Nitrogen.Pressure().Show(); break;

            case Key.F6: new Nitrogen.Menu().Show(); break;
        }
    }

    private void Pressure_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Pressure pressure = new Nitrogen.Pressure();
        pressure.Show();
    }

    private void Menu_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Menu menu = new Nitrogen.Menu();
        menu.Show();
    }
}