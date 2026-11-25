
using Nitrogen.Views.MainView;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Nitrogen.Views.Settings.PressureSet;

public sealed class PressureSetControlViewModel : ReactiveObject, IDisposable
{
    private readonly MainViewModel _mainVm;

    public PressureSetControlViewModel(MainViewModel mainVm)
    {
        _mainVm = mainVm;

        _mainVm.PropertyChanged += MainVmOnPropertyChanged;
    }

    public string Pressure_1 => _mainVm.Pressure_1;

    private string _pressure_1SrcMinEdit = "";

    public string Pressure_1SrcMinEdit
    {
        get => _pressure_1SrcMinEdit;
        set => this.RaiseAndSetIfChanged(ref _pressure_1SrcMinEdit, value);
    }

    private string _pressure_1SrcMaxEdit = "";

    public string Pressure_1SrcMaxEdit
    {
        get => _pressure_1SrcMaxEdit;
        set => this.RaiseAndSetIfChanged(ref _pressure_1SrcMaxEdit, value);
    }

    private string _pressure_1TargMinEdit = "";

    public string Pressure_1TargMinEdit
    {
        get => _pressure_1TargMinEdit;
        set => this.RaiseAndSetIfChanged(ref _pressure_1TargMinEdit, value);
    }

    private string _pressure_1TargMaxEdit = "";

    public string Pressure_1TargMaxEdit
    {
        get => _pressure_1TargMaxEdit;
        set => this.RaiseAndSetIfChanged(ref _pressure_1TargMaxEdit, value);
    }

    public Task LoadAsync()
    {
        // TODO [CoreLink]:
        // Загрузка параметров калибровки давления.
        //
        // Legacy: чтение восьми Input Registers:
        //
        // Pressure_1SrcMinLo
        // Pressure_1SrcMinHi
        // Pressure_1SrcMaxLo
        // Pressure_1SrcMaxHi
        // Pressure_1TargMinLo
        // Pressure_1TargMinHi
        // Pressure_1TargMaxLo
        // Pressure_1TargMaxHi
        //
        // Преобразование каждой пары регистров в float.
        //
        // Полученные значения записываются в:
        // Pressure_1SrcMinEdit
        // Pressure_1SrcMaxEdit
        // Pressure_1TargMinEdit
        // Pressure_1TargMaxEdit
        //
        // Форматирование: "0.###", InvariantCulture.

        return Task.CompletedTask;
    }

    public Task SaveAsync()
    {
        // TODO [CoreLink]:
        // Сохранение параметров калибровки давления.
        //
        // Legacy:
        // 1. Преобразование четырёх Edit-свойств в float
        //    с использованием InvariantCulture.
        //
        // 2. Запись восьми Holding Registers:
        //
        // Pressure_1SrcMinLo
        // Pressure_1SrcMinHi
        // Pressure_1SrcMaxLo
        // Pressure_1SrcMaxHi
        // Pressure_1TargMinLo
        // Pressure_1TargMinHi
        // Pressure_1TargMaxLo
        // Pressure_1TargMaxHi
        //
        // 3. После успешной записи параметров
        //    отправка команды Pressure_1SetPrmLo = 1.

        return Task.CompletedTask;
    }

    private void MainVmOnPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.Pressure_1))
        {
            this.RaisePropertyChanged(nameof(Pressure_1));
        }
    }

    public void Dispose()
    {
        _mainVm.PropertyChanged -= MainVmOnPropertyChanged;
    }
}
