namespace Spike001.WinUI.Core.Configuration;

public sealed record OcrSettings
{
    public bool Enabled { get; init; }
    public int MinimumConfidence { get; init; } = 70;
}
