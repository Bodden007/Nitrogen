using Avalonia.Controls;

namespace Nitrogen;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

       SystemDecorations = SystemDecorations.None;
       WindowState = WindowState.FullScreen;
    }
}