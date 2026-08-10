namespace Spike001.WinUI.Core.Configuration;

public sealed record InstitutionSettings
{
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
}
