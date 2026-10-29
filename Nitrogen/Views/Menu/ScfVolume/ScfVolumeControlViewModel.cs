
using System;
using System.Globalization;
using System.Threading.Tasks;
using Nitrogen.Views.MainView;
using ReactiveUI;

namespace Nitrogen.Views.Menu.SCFVolume;

public sealed class ScfVolumeControlViewModel : ReactiveObject
{
    private readonly MainViewModel _mainVm;

    public ScfVolumeControlViewModel(MainViewModel mainVm)
    {
        _mainVm = mainVm;
    }

    private string _totalScfStageEdit = string.Empty;

    public string TotalScfStageEdit
    {
        get => _totalScfStageEdit;
        set => this.RaiseAndSetIfChanged(
            ref _totalScfStageEdit, value);
    }

    private string _pump1ScfStageEdit = string.Empty;

    public string Pump1ScfStageEdit
    {
        get => _pump1ScfStageEdit;
        set => this.RaiseAndSetIfChanged(
            ref _pump1ScfStageEdit, value);
    }

    private string _totalScfJobEdit = string.Empty;

    public string TotalScfJobEdit
    {
        get => _totalScfJobEdit;
        set => this.RaiseAndSetIfChanged(
            ref _totalScfJobEdit, value);
    }

    private string _pump1ScfJobEdit = string.Empty;

    public string Pump1ScfJobEdit
    {
        get => _pump1ScfJobEdit;
        set => this.RaiseAndSetIfChanged(
            ref _pump1ScfJobEdit, value);
    }

    public Task LoadAsync()
    {
        Pump1ScfStageEdit = Format(_mainVm.Pump1ScfStage);
        TotalScfStageEdit = Format(_mainVm.TotalScfStage);
        Pump1ScfJobEdit = Format(_mainVm.Pump1ScfJob);
        TotalScfJobEdit = Format(_mainVm.TotalScfJob);

        // TODO [CoreLink/Read]:
        // Заменить демонстрационные значения
        // данными CoreLink.
        //
        // Legacy: REAL из двух регистров,
        // формат "0.###", InvariantCulture.
        //
        // Rx принадлежит CoreLink.

        return Task.CompletedTask;
    }

    private static string Format(double value)
    {
        return value.ToString(
            "0.###",
            CultureInfo.InvariantCulture);
    }

    // TODO [CoreLink/Command]: Установка значений.
    //
    // Legacy последовательно записывает:
    // PUMP_1SCFStageLo
    // PUMP_1SCFJobLo
    // TotalSCFStageLo
    // TotalSCFJobLo
    // TotalSCFCommandJobSetLo = 1
    //
    // Затем ожидает 1000 мс и обновляет показания.
    //
    // Перед записью проверяются все четыре float.
    // Порядок операций сохранить в будущем API CoreLink.

    // TODO [CoreLink/Command]: Сброс стадии.
    //
    // TotalSCFCommandStageResetLo = 1
    // Ожидание 1000 мс, обновление показаний.

    // TODO [CoreLink/Command]: Сброс работы.
    //
    // TotalSCFCommandJobResetLo = 1
    // Ожидание 1000 мс, обновление показаний.

    private static bool TryParse(string text, out float value)
    {
        return float.TryParse(
            text.Replace(',', '.'),
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out value);
    }
}
