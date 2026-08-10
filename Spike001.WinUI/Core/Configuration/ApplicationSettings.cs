namespace Spike001.WinUI.Core.Configuration;

public sealed record ApplicationSettings
{
    public string ApplicationName { get; init; } = "OficialiaTechLab";
    public bool FirstRunCompleted { get; init; }
}
