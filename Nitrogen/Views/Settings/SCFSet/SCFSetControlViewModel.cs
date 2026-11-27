
using Nitrogen.Views.MainView;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Nitrogen.Views.Settings.SCFSet;

public sealed class SCFSetControlViewModel : ReactiveObject, IDisposable
{
    private readonly MainViewModel _mainVm;

    public double Pump1Rpm => _mainVm.Pump1Rpm;

    // NOTE: Текущий расход азота с единицей измерения.
    public string Pump1TotalScfm => _mainVm.Pump1TotalScfm;

    public SCFSetControlViewModel(MainViewModel mainVm)
    {
        _mainVm = mainVm;

        _mainVm.PropertyChanged += MainVmOnPropertyChanged;
    }

    private string _pump_1TeethCountEdit = "";

    public string Pump_1TeethCountEdit
    {
        get => _pump_1TeethCountEdit;
        set => this.RaiseAndSetIfChanged(
            ref _pump_1TeethCountEdit, value);
    }

    private string _pump_1PumpFactorEdit = "";

    public string Pump_1PumpFactorEdit
    {
        get => _pump_1PumpFactorEdit;
        set => this.RaiseAndSetIfChanged(
            ref _pump_1PumpFactorEdit, value);
    }

    private string _pump_1PumpEfficiencyFactorEdit = "";

    public string Pump_1PumpEfficiencyFactorEdit
    {
        get => _pump_1PumpEfficiencyFactorEdit;
        set => this.RaiseAndSetIfChanged(
            ref _pump_1PumpEfficiencyFactorEdit, value);
    }

    public Task LoadAsync()
    {
        // TODO [CoreLink]:
        
        // Загрузка параметров калибровки SCF.
        //
        // Legacy: чтение блока Input Registers.
        //
        // registers[0] — PUMP_1TeethCountLo (ushort).
        // registers[1] — резервный регистр 121.
        //
        // registers[2] — PUMP_1PumpFactorLo.
        // registers[3] — PUMP_1PumpFactorHi.
        //
        // registers[4] — PUMP_1PumpEfficiencyFactorLo.
        // registers[5] — PUMP_1PumpEfficiencyFactorHi.
        //
        // PumpFactor и EfficiencyFactor преобразуются в float.
        //
        // Полученные значения записываются в:
        // Pump_1TeethCountEdit
        // Pump_1PumpFactorEdit
        // Pump_1PumpEfficiencyFactorEdit
        //
        // Форматирование float: "0.###", InvariantCulture.

        return Task.CompletedTask;
    }

    public Task SaveAsync()
    {
        // TODO [CoreLink]:
        // Сохранение параметров калибровки SCF.
        //
        // Legacy:
        //
        // 1. Преобразование Edit-свойств:
        //    TeethCount       — ushort.
        //    PumpFactor       — float.
        //    EfficiencyFactor — float.
        //
        //    Используется InvariantCulture.
        //
        // 2. Формирование блока из шести Holding Registers:
        //
        //    registers[0] — TeethCount.
        //    registers[1] — резервный регистр 121 (0).
        //    registers[2] — PumpFactorLo.
        //    registers[3] — PumpFactorHi.
        //    registers[4] — EfficiencyFactorLo.
        //    registers[5] — EfficiencyFactorHi.
        //
        // 3. Запись блока регистров.
        //
        // 4. После успешной записи отправка команды:
        //    PUMP_1PumpSetPrmLo = 1.
        //
        // Способ передачи параметров через CoreLink
        // будет определён отдельно.

        return Task.CompletedTask;
    }

    private void MainVmOnPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.Pump1Rpm))
        {
            this.RaisePropertyChanged(nameof(Pump1Rpm));
        }

        if (e.PropertyName == nameof(MainViewModel.Pump1TotalScfm))
        {
            this.RaisePropertyChanged(nameof(Pump1TotalScfm));
        }
    }

    public void Dispose()
    {
        _mainVm.PropertyChanged -= MainVmOnPropertyChanged;
    }
}
