namespace Spike001.WinUI.Core.Configuration;

public sealed record AppSettings
{
    public int ConfigurationVersion { get; init; } = 1;
    public ApplicationSettings Application { get; init; } = new();
    public AppearanceSettings Appearance { get; init; } = new();
    public InstitutionSettings Institution { get; init; } = new();
    public CorrespondenceSettings Correspondence { get; init; } = new();
    public NumberingSettings Numbering { get; init; } = new();
    public OcrSettings Ocr { get; init; } = new();
    public DiagnosticSettings Diagnostic { get; init; } = new();

    public static AppSettings CreateDefault() => new();
}
