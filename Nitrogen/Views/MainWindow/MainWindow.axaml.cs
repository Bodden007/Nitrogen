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

            case Key.F2: new Nitrogen.Temperature().Show(); break;

            case Key.F4: new Nitrogen.Volume().Show(); break;

            case Key.F8: new Nitrogen.Menu().Show(); break;

            case Key.D1: HandlerRateLineDisplay(); break;

            case Key.D2: HandlerTempLineDisplay(); break;

            case Key.D3: HandlerVolumeLineDisplay(); break;
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

    private void Temp_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Temperature temp = new Nitrogen.Temperature();
        temp.Show();
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

    private void HandlerTempLineDisplay()
    {
        if (!TempOut.IsVisible)
        {
            HeaderTemp.Text = "Температура";

            TempOut.IsVisible = true;
            TempExc.IsVisible = true;
            ValueVaporizer.IsVisible = true;

            ValueTempOut.Classes.Remove("StyleTextBlock_2");
            ValueTempOut.Classes.Add("StyleTextBlock_1");
        }
        else
        {
            HeaderTemp.Text = "Температура Выход";

            TempOut.IsVisible = false;
            TempExc.IsVisible = false;
            ValueVaporizer.IsVisible = false;

            ValueTempOut.Classes.Remove("StyleTextBlock_1");
            ValueTempOut.Classes.Add("StyleTextBlock_2");
        }
    }

    private void HandlerRateLineDisplay()
    {
        if (!HeaderRate_A.IsVisible)
        {
            HeaderRate_A.IsVisible = true;
            ValueRate_A.IsVisible = true;
            ValueRate.IsVisible = false;
            HeaderRate_B.IsVisible = true;
            ValueRate_B.IsVisible = true;

            HeaderRate.Text = "Расход";
        }
        else
        {
            HeaderRate_A.IsVisible = false;
            ValueRate_A.IsVisible = false;
            ValueRate.IsVisible = true;
            HeaderRate_B.IsVisible = false;
            ValueRate_B.IsVisible = false;

            HeaderRate.Text = "Σ Расход";
        }
    }


}