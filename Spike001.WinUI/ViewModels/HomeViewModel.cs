using CommunityToolkit.Mvvm.ComponentModel;

namespace Spike001.WinUI.ViewModels;

public sealed partial class HomeViewModel : ObservableObject
{
    public string ViewName => "HomePage";

    [ObservableProperty]
    public partial string Purpose { get; set; } = "Valida la vista inicial y el composition root experimental.";
}
