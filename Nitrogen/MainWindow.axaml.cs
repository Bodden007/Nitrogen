using Avalonia.Controls;
using Avalonia.Media;

namespace Nitrogen;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;
    }

    private void Pressure_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Pressure.Background = Brushes.Red;
    }
}