using System;

namespace Spike001.WinUI.Core.Configuration;

public sealed class SettingsValidator
{
    public const int CurrentConfigurationVersion = 1;

    public ConfigurationResult Validate(AppSettings? settings)
    {
        var result = new ConfigurationResult();

        if (settings is null)
        {
            result.AddError(
                ConfigurationIssueSource.Validation,
                ConfigurationIssueCode.InvalidValue,
                "La configuración no puede ser nula.");
            return result;
        }

        if (settings.ConfigurationVersion != CurrentConfigurationVersion)
        {
            result.AddError(
                ConfigurationIssueSource.Validation,
                ConfigurationIssueCode.UnsupportedVersion,
                $"La versión de configuración {settings.ConfigurationVersion} no está soportada.");
        }

        if (settings.Application is null ||
            string.IsNullOrWhiteSpace(settings.Application.ApplicationName))
        {
            result.AddError(
                ConfigurationIssueSource.Validation,
                ConfigurationIssueCode.InvalidValue,
                "Application.ApplicationName es obligatorio.");
        }

        if (settings.Appearance is null ||
            !Enum.IsDefined(settings.Appearance.Theme))
        {
            result.AddError(
                ConfigurationIssueSource.Validation,
                ConfigurationIssueCode.InvalidValue,
                "Appearance.Theme no contiene un valor válido.");
        }

        if (settings.Institution is null)
        {
            result.AddError(
                ConfigurationIssueSource.Validation,
                ConfigurationIssueCode.InvalidValue,
                "Institution es obligatorio.");
        }

        if (settings.Correspondence is null ||
            settings.Correspondence.DefaultResponseDays < 0)
        {
            result.AddError(
                ConfigurationIssueSource.Validation,
                ConfigurationIssueCode.InvalidValue,
                "Correspondence.DefaultResponseDays no puede ser negativo.");
        }

        if (settings.Numbering is null ||
            settings.Numbering.InitialNumber < 0)
        {
            result.AddError(
                ConfigurationIssueSource.Validation,
                ConfigurationIssueCode.InvalidValue,
                "Numbering.InitialNumber no puede ser negativo.");
        }

        if (settings.Ocr is null ||
            settings.Ocr.MinimumConfidence is < 0 or > 100)
        {
            result.AddError(
                ConfigurationIssueSource.Validation,
                ConfigurationIssueCode.InvalidValue,
                "Ocr.MinimumConfidence debe estar entre 0 y 100.");
        }

        if (settings.Diagnostic is null)
        {
            result.AddError(
                ConfigurationIssueSource.Validation,
                ConfigurationIssueCode.InvalidValue,
                "Diagnostic es obligatorio.");
        }

        if (result.Succeeded)
        {
            result.AddInformation(
                ConfigurationIssueSource.Validation,
                ConfigurationIssueCode.ValidationSucceeded,
                "La configuración pasó la validación tipada.");
        }

        return result;
    }
}
