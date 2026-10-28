using CommunityToolkit.Mvvm.ComponentModel;

namespace Nitrogen.ViewModels;

// FIXME: Replace CommunityToolkit.Mvvm ObservableProperty with ReactiveUI.
public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";
}
