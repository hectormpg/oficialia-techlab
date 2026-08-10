namespace Spike001.WinUI.Core.Configuration;

public sealed record DiagnosticSettings
{
    public bool Enabled { get; init; }
    public bool IncludeEnvironmentDetails { get; init; }
}
