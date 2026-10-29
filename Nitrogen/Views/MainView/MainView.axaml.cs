
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Nitrogen.Views.Menu;
using Nitrogen.Views.Interfaces;

namespace Nitrogen.Views.MainView;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();


    }

    public void HandleKeyDown(KeyEventArgs e)
    {
        if (e.Source is TextBox && !IsCommandKey(e.Key))
            return;

      if (ScreenHost.Content is IHotKeyScreen hotKeyScreen)
{
    hotKeyScreen.HandleKey(e.Key);
    e.Handled = true;
    return;
}

        if (e.Key == Key.Escape)
        {
            e.Handled = true;

            // TODO: Восстановить управление полноэкранным режимом
            // через родительское Window.

            return;
        }

        switch (e.Key)
        {
            case Key.F1:
                ShowPressureScreen();
                e.Handled = true;
                break;

            case Key.F2:
                ShowTemperatureScreen();
                e.Handled = true;
                break;

            case Key.F3:
                ShowScfVolumeScreen();
                e.Handled = true;
                break;

            case Key.F4:
                ShowVolumeScreen();
                e.Handled = true;
                break;

            case Key.F5:
                ShowEngineScreen();
                e.Handled = true;
                break;

            case Key.F6:
                ShowRecordScreen();
                e.Handled = true;
                break;

            case Key.F7:
                // TODO: В Legacy экран StagesControl не реализован.
                e.Handled = true;
                break;

            case Key.F8:
                ShowMenuScreen();
                e.Handled = true;
                break;

            case Key.D1:
                HandlerSCFMLineDisplay();
                break;

            case Key.D2:
                HandlerTempLineDisplay();
                break;

            case Key.D3:
                HandlerVolumeLineDisplay();
                break;
        }
    }

    private void Pressure_Click(object? sender, RoutedEventArgs e)
    {
        ShowPressureScreen();
    }

    private void Volume_Click(object? sender, RoutedEventArgs e)
    {
        ShowVolumeScreen();
    }

    private void Engine_Click(object? sender, RoutedEventArgs e)
    {
        ShowEngineScreen();
    }

    private void Menu_Click(object? sender, RoutedEventArgs e)
    {
        ShowMenuScreen();
    }

    private void Temp_Click(object? sender, RoutedEventArgs e)
    {
        ShowTemperatureScreen();
    }

    private void Stages_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Перенести StagesControl.
        // В Legacy вызов экрана также был закомментирован.
    }

    private void Scf_Click(object? sender, RoutedEventArgs e)
    {
        ShowScfVolumeScreen();
    }

    private void Record_Click(object? sender, RoutedEventArgs e)
    {
        ShowRecordScreen();
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

    private void HandlerSCFMLineDisplay()
    {
        if (!HeaderRate_A.IsVisible)
        {
            HeaderRate_A.IsVisible = true;
            ValueRate_A.IsVisible = true;
            ValueRate.IsVisible = false;
            HeaderRate_B.IsVisible = true;
            ValueRate_B.IsVisible = true;

            HeaderRate.Text = "SCF";
        }
        else
        {
            HeaderRate_A.IsVisible = false;
            ValueRate_A.IsVisible = false;
            ValueRate.IsVisible = true;
            HeaderRate_B.IsVisible = false;
            ValueRate_B.IsVisible = false;

            HeaderRate.Text = "Σ SCFM";
        }
    }

    private static bool IsCommandKey(Key key)
    {
        return key == Key.Escape
            || key == Key.Enter
            || key is >= Key.F1 and <= Key.F12;
    }

    public void ShowPressureScreen()
    {
        // TODO: ScreenFactory.CreatePressureScreen()
        // TODO: ScreenNavigator.ShowScreen()
    }

    public void ShowEngineScreen()
    {
        // TODO: ScreenFactory.CreateEngineScreen()
        // TODO: ScreenNavigator.ShowScreen()
    }

    public void ShowScfVolumeScreen()
    {
        // TODO: ScreenFactory.CreateScfVolumeScreen()
        // TODO: ScreenNavigator.ShowScreen()
    }

    private void ShowTemperatureScreen()
    {
        // TODO: Создать TemperatureControl и открыть через ScreenNavigator.
    }

    private void ShowVolumeScreen()
    {
        // TODO: Создать VolumeControl и открыть через ScreenNavigator.
    }

    private void ShowRecordScreen()
    {
        // TODO: Создать RecordControl и открыть через ScreenNavigator.
    }

    private void ShowMenuScreen()
    {
        // TODO: Пересмотреть жизненный цикл экранов.
        // Рассмотреть повторное использование MenuControl вместо создания при каждом открытии.
        var menu = new MenuControl();

        menu.CloseRequested += (_, _) => CloseScreen();

        ShowScreen(menu);
    }

    public void ShowScreen(UserControl screen)
    {
        ScreenHost.Content = screen;
        ScreenHost.IsVisible = true;

        screen.Focus();

        // TODO: Восстановить историю навигации
        // после переноса остальных экранов.
    }

    public void BackScreen()
    {
        // TODO: ScreenNavigator.BackScreen().
        // TODO: Восстановить текущий экран и его видимость.
        // TODO: Восстановить управление фокусом.
    }

    public void CloseScreen()
    {
        ScreenHost.Content = null;
        ScreenHost.IsVisible = false;

        Focus();

        // TODO: Восстановить ScreenNavigator.CloseScreen().
    }
}
