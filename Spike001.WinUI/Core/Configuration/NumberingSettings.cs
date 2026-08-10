namespace Spike001.WinUI.Core.Configuration;

public sealed record NumberingSettings
{
    public string Prefix { get; init; } = "OF";
    public int InitialNumber { get; init; } = 1;
}
