using CommunityToolkit.Mvvm.ComponentModel;

namespace Spike001.WinUI.ViewModels;

public sealed partial class TestsViewModel : ObservableObject
{
    public string ViewName => "TestsPage";

    [ObservableProperty]
    public partial string Purpose { get; set; } = "Valida el recorrido básico de navegación entre vistas.";
}
