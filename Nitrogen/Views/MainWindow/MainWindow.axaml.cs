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

        WindowState = WindowState.FullScreen;

        this.KeyDown += MainWindow_KeyDown;
    }
    private void MainWindow_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.F1: new Nitrogen.Views.Menu.Pressure().Show(); break;

            case Key.F2: new Nitrogen.Views.Menu.Temperature().Show(); break;

            case Key.F3: new Nitrogen.Views.Menu.ScfVolume().Show(); break;

            case Key.F4: new Nitrogen.Views.Menu.Volume().Show(); break;

            case Key.F5: new Nitrogen.Views.Menu.Engine().Show(); break;

            case Key.F7: new Nitrogen.Views.Menu.Stages().Show(); break;

            case Key.F8: new Nitrogen.Views.Menu.Menu().Show(); break;

            case Key.D1: HandlerRateLineDisplay(); break;

            case Key.D2: HandlerTempLineDisplay(); break;

            case Key.D3: HandlerVolumeLineDisplay(); break;
        }
    }
    private void Pressure_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.Pressure pressure = new Nitrogen.Views.Menu.Pressure();
        pressure.Show();
    }
    private void Volume_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.Volume volume = new Nitrogen.Views.Menu.Volume();
        volume.Show();
    }
    private void Engine_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.Engine engine = new Nitrogen.Views.Menu.Engine();
        engine.Show();
    }
    private void Menu_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.Menu menu = new Nitrogen.Views.Menu.Menu();
        menu.Show();
    }
    private void Temp_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.Temperature temp = new Nitrogen.Views.Menu.Temperature();
        temp.Show();
    }
    private void Stages_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.Stages stages = new Nitrogen.Views.Menu.Stages();
        stages.Show();
    }
    private void Scf_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Nitrogen.Views.Menu.ScfVolume scfVolume = new Nitrogen.Views.Menu.ScfVolume();
        scfVolume.Show();
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

            ValueTempOut.Classes.Remove("menu_title");
            ValueTempOut.Classes.Add("menu_subtext");
        }
        else
        {
            HeaderTemp.Text = "Температура Выход";

            TempOut.IsVisible = false;
            TempExc.IsVisible = false;
            ValueVaporizer.IsVisible = false;

            ValueTempOut.Classes.Remove("menu_subtext");
            ValueTempOut.Classes.Add("menu_title");
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