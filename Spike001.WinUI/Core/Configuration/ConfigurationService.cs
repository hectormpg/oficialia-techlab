using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Spike001.WinUI.Core.Configuration;

public sealed class ConfigurationService : IConfigurationService
{
    private const string SettingsFileName = "settings.json";
    private const string ApplicationDirectoryName = "OficialiaTechLab";

    private readonly SettingsSerializer _serializer;
    private readonly SettingsValidator _validator;
    private readonly string _settingsPath;
    private AppSettings _current = AppSettings.CreateDefault();

    public ConfigurationService(
        SettingsSerializer serializer,
        SettingsValidator validator,
        string? settingsPath = null)
    {
        _serializer = serializer;
        _validator = validator;
        _settingsPath = settingsPath ?? GetDefaultSettingsPath();
    }

    public AppSettings Current => _current;

    public ConfigurationResult Load()
    {
        var result = new ConfigurationResult();

        if (!File.Exists(_settingsPath))
        {
            _current = AppSettings.CreateDefault();
            result.AddInformation(
                ConfigurationIssueSource.Service,
                ConfigurationIssueCode.DefaultConfigurationCreated,
                "No existía settings.json; se creó una configuración por defecto en memoria.");
            return result;
        }

        string serializedSettings;
        try
        {
            serializedSettings = File.ReadAllText(_settingsPath, Encoding.UTF8);
        }
        catch (IOException exception)
        {
            result.AddError(
                ConfigurationIssueSource.FileSystem,
                ConfigurationIssueCode.FileReadFailed,
                $"No se pudo leer settings.json: {exception.Message}");
            return result;
        }
        catch (UnauthorizedAccessException exception)
        {
            result.AddError(
                ConfigurationIssueSource.FileSystem,
                ConfigurationIssueCode.FileReadFailed,
                $"No se pudo acceder a settings.json: {exception.Message}");
            return result;
        }

        if (!_serializer.TryDeserialize(serializedSettings, out var loadedSettings, result))
        {
            return result;
        }

        var validation = _validator.Validate(loadedSettings);
        result.Merge(validation);
        if (!validation.Succeeded)
        {
            return result;
        }

        _current = loadedSettings!;
        result.AddInformation(
            ConfigurationIssueSource.Service,
            ConfigurationIssueCode.ConfigurationLoaded,
            "La configuración se cargó correctamente.");
        return result;
    }

    public ConfigurationResult Save()
    {
        return Save(_current);
    }

    public ConfigurationResult Save(AppSettings settings)
    {
        var result = _validator.Validate(settings);
        if (!result.Succeeded)
        {
            return result;
        }

        string serializedSettings;
        try
        {
            serializedSettings = _serializer.Serialize(settings);
        }
        catch (Exception exception) when (exception is JsonException or NotSupportedException)
        {
            result.AddError(
                ConfigurationIssueSource.Serialization,
                ConfigurationIssueCode.SerializationFailed,
                $"No se pudo serializar la configuración: {exception.Message}");
            return result;
        }

        var directory = Path.GetDirectoryName(_settingsPath);
        var temporaryPath = Path.Combine(
            directory ?? AppContext.BaseDirectory,
            $"{SettingsFileName}.{Path.GetRandomFileName()}.tmp");

        try
        {
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var stream = new FileStream(
                       temporaryPath,
                       FileMode.CreateNew,
                       FileAccess.Write,
                       FileShare.None,
                       bufferSize: 4096,
                       options: FileOptions.WriteThrough))
            using (var writer = new StreamWriter(stream, new UTF8Encoding(false), leaveOpen: true))
            {
                writer.Write(serializedSettings);
                writer.Flush();
                stream.Flush(flushToDisk: true);
            }

            if (File.Exists(_settingsPath))
            {
                File.Replace(temporaryPath, _settingsPath, destinationBackupFileName: null);
            }
            else
            {
                File.Move(temporaryPath, _settingsPath);
            }

            _current = settings;
            result.AddInformation(
                ConfigurationIssueSource.Service,
                ConfigurationIssueCode.ConfigurationSaved,
                "La configuración se guardó mediante archivo temporal y reemplazo seguro.");
            return result;
        }
        catch (IOException exception)
        {
            result.AddError(
                ConfigurationIssueSource.FileSystem,
                ConfigurationIssueCode.FileWriteFailed,
                $"No se pudo escribir settings.json de forma segura: {exception.Message}");
            return result;
        }
        catch (UnauthorizedAccessException exception)
        {
            result.AddError(
                ConfigurationIssueSource.FileSystem,
                ConfigurationIssueCode.FileWriteFailed,
                $"No se pudo acceder a la ruta de configuración: {exception.Message}");
            return result;
        }
        finally
        {
            try
            {
                if (File.Exists(temporaryPath))
                {
                    File.Delete(temporaryPath);
                }
            }
            catch (Exception)
            {
                // La limpieza es best-effort y no debe alterar el resultado principal de Save.
            }
        }
    }

    public string Export()
    {
        return _serializer.Serialize(_current);
    }

    public ConfigurationResult Import(string serializedSettings)
    {
        var result = new ConfigurationResult();
        if (!_serializer.TryDeserialize(serializedSettings, out var importedSettings, result))
        {
            return result;
        }

        var validation = _validator.Validate(importedSettings);
        result.Merge(validation);
        if (!validation.Succeeded)
        {
            return result;
        }

        _current = importedSettings!;
        result.AddInformation(
            ConfigurationIssueSource.Service,
            ConfigurationIssueCode.ConfigurationImported,
            "La configuración se importó en memoria; debe guardarse explícitamente.");
        return result;
    }

    private static string GetDefaultSettingsPath()
    {
        var localApplicationData = Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(localApplicationData, ApplicationDirectoryName, SettingsFileName);
    }
}
