using CommunityToolkit.Mvvm.ComponentModel;

namespace Spike001.WinUI.ViewModels;

public sealed partial class PlatformViewModel : ObservableObject
{
    public string ViewName => "PlatformPage";

    [ObservableProperty]
    public partial string Purpose { get; set; } = "Valida una vista de plataforma sin acoplarla a servicios de infraestructura.";
}
