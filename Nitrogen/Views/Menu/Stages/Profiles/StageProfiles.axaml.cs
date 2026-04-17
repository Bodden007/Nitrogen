using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;

namespace Nitrogen.Views.Menu.Stage.Profiles;

public partial class StageProfiles : Window
{
    public StageProfiles()
    {
        InitializeComponent();

        WindowState = WindowState.FullScreen;

        this.KeyDown += StageProfiles_KeyDown;

        Closed += StageProfiles_Closed;
    }
    private void StageProfiles_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape: Close();break;
        }
    }

    private void StageProfiles_Closed(object? sender, System.EventArgs e)
    {
        this.KeyDown -= StageProfiles_KeyDown;
        Closed -= StageProfiles_Closed;
    }


}