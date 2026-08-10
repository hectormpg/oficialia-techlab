using System.Collections.Generic;

namespace Spike001.WinUI.Core.Configuration;

public sealed class ConfigurationResult
{
    private readonly List<ConfigurationMessage> _errors = [];
    private readonly List<ConfigurationMessage> _warnings = [];
    private readonly List<ConfigurationMessage> _information = [];

    public IReadOnlyList<ConfigurationMessage> Errors => _errors;
    public IReadOnlyList<ConfigurationMessage> Warnings => _warnings;
    public IReadOnlyList<ConfigurationMessage> Information => _information;
    public bool Succeeded => _errors.Count == 0;

    internal void AddError(ConfigurationIssueSource source, ConfigurationIssueCode code, string message)
    {
        _errors.Add(new ConfigurationMessage(source, code, message));
    }

    internal void AddWarning(ConfigurationIssueSource source, ConfigurationIssueCode code, string message)
    {
        _warnings.Add(new ConfigurationMessage(source, code, message));
    }

    internal void AddInformation(ConfigurationIssueSource source, ConfigurationIssueCode code, string message)
    {
        _information.Add(new ConfigurationMessage(source, code, message));
    }

    internal void Merge(ConfigurationResult other)
    {
        _errors.AddRange(other.Errors);
        _warnings.AddRange(other.Warnings);
        _information.AddRange(other.Information);
    }
}

public sealed record ConfigurationMessage(
    ConfigurationIssueSource Source,
    ConfigurationIssueCode Code,
    string Message);

public enum ConfigurationIssueSource
{
    Validation,
    Serialization,
    FileSystem,
    Service
}

public enum ConfigurationIssueCode
{
    InvalidValue,
    UnsupportedVersion,
    InvalidJson,
    EmptyDocument,
    SerializationFailed,
    FileReadFailed,
    FileWriteFailed,
    ValidationSucceeded,
    DefaultConfigurationCreated,
    ConfigurationLoaded,
    ConfigurationSaved,
    ConfigurationImported,
    ConfigurationExported
}
