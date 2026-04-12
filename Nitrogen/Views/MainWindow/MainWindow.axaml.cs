using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;

namespace Nitrogen;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        //SystemDecorations = SystemDecorations.None;
        WindowState = WindowState.FullScreen;

        this.KeyDown += MainWindow_KeyDown;
    }

    private void MainWindow_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.F1: new Nitrogen.Pressure().Show(); break;

            case Key.F5:new Nitrogen.Volume().Show(); break;

            case Key.F6: new Nitrogen.Menu().Show(); break;

            case Key.D2: HandlerVolumeLineDisplay(); break;
        }
    }

    private void Pressure_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Pressure pressure = new Nitrogen.Pressure();
        pressure.Show();
    }
    private void Volume_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Volume volume = new Nitrogen.Volume();
        volume.Show();
    }

    private void Menu_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Menu menu = new Nitrogen.Menu();
        menu.Show();
    }

    private void HandlerVolumeLineDisplay()
    {
        if (CombiVolume.IsVisible)
        {
            DsStage.IsVisible = true;
            PsStage.IsVisible = true;
            PsVolume.IsVisible = true;
            DsVolume.IsVisible = true;
            CombiVolume.IsVisible = false;
            CombiHeader.Text = "Объем Цемента";
        }
        else
        {
            DsStage.IsVisible = false;
            PsStage.IsVisible = false;
            PsVolume.IsVisible = false;
            DsVolume.IsVisible = false;
            CombiVolume.IsVisible = true;
            CombiHeader.Text = "Σ Объем Цемента";
        }
    }
}