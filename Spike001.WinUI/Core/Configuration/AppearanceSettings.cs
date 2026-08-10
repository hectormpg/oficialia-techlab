namespace Spike001.WinUI.Core.Configuration;

public sealed record AppearanceSettings
{
    public ThemeMode Theme { get; init; } = ThemeMode.System;
    public bool HighContrast { get; init; }
}

public enum ThemeMode
{
    System,
    Light,
    Dark
}
