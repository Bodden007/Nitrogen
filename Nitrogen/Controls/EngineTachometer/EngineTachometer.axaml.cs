using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Nitrogen.Controls.EngineTachometer;

public partial class EngineTachometer : UserControl
{
    public EngineTachometer()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}