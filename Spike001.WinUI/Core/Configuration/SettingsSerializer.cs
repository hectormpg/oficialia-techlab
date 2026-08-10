using System;
using System.Text.Json;

namespace Spike001.WinUI.Core.Configuration;

public sealed class SettingsSerializer
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public string Serialize(AppSettings settings)
    {
        return JsonSerializer.Serialize(settings, _options);
    }

    public bool TryDeserialize(
        string serializedSettings,
        out AppSettings? settings,
        ConfigurationResult result)
    {
        settings = null;

        if (string.IsNullOrWhiteSpace(serializedSettings))
        {
            result.AddError(
                ConfigurationIssueSource.Serialization,
                ConfigurationIssueCode.EmptyDocument,
                "El documento de configuración está vacío.");
            return false;
        }

        try
        {
            settings = JsonSerializer.Deserialize<AppSettings>(serializedSettings, _options);
            if (settings is null)
            {
                result.AddError(
                    ConfigurationIssueSource.Serialization,
                    ConfigurationIssueCode.EmptyDocument,
                    "El documento no contiene una configuración.");
                return false;
            }

            return true;
        }
        catch (JsonException exception)
        {
            result.AddError(
                ConfigurationIssueSource.Serialization,
                ConfigurationIssueCode.InvalidJson,
                $"El JSON de configuración no es válido: {exception.Message}");
            return false;
        }
        catch (NotSupportedException exception)
        {
            result.AddError(
                ConfigurationIssueSource.Serialization,
                ConfigurationIssueCode.InvalidJson,
                $"El JSON contiene un tipo no soportado: {exception.Message}");
            return false;
        }
    }
}
