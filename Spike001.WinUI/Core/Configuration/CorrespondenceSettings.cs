namespace Spike001.WinUI.Core.Configuration;

public sealed record CorrespondenceSettings
{
    public bool RequireRecipient { get; init; } = true;
    public int DefaultResponseDays { get; init; } = 5;
}
