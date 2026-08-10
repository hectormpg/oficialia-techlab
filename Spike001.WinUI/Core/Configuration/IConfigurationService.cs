namespace Spike001.WinUI.Core.Configuration;

public interface IConfigurationService
{
    AppSettings Current { get; }

    ConfigurationResult Load();

    ConfigurationResult Save();

    ConfigurationResult Save(AppSettings settings);

    string Export();

    ConfigurationResult Import(string serializedSettings);
}
