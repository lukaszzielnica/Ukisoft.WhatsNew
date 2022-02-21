using Ukisoft.WhatsNew.Data;
using Ukisoft.WhatsNew.Data.Internal;
using Ukisoft.WhatsNew.Exceptions;
using Ukisoft.WhatsNew.Utils;

namespace Ukisoft.WhatsNew;

/// <summary>
/// Allows to load and save XML representation of the application changelog.
/// </summary>
public static class Changelog
{
    /// <summary>
    /// Loads the application changelog from the XML file.
    /// </summary>
    /// <param name="filePath">XML file path to read from.</param>
    /// <returns>Application changelog fetched from the XML file.</returns>
    public static AppChangelog LoadFromXmlFile(string filePath)
    {
        ValidateXmlFile(filePath);
        var whatsNew = XmlUtils.DeserializeFromFile<WhatsNewRoot>(filePath);
        return whatsNew.ConvertToChangelog();
    }

    /// <summary>
    /// Asynchronous version of LoadFromXmlFile method.
    /// </summary>
    /// <see cref="LoadFromXmlFile(string)"/>
    /// <param name="filePath">XML file path to read from.</param>
    /// <returns>Application changelog fetched from the XML file.</returns>
    public static async Task<AppChangelog> LoadFromXmlFileAsync(string filePath)
    {
        await ValidateXmlFileAsync(filePath);
        var whatsNew = await XmlUtils.DeserializeFromFileAsync<WhatsNewRoot>(filePath);
        return whatsNew.ConvertToChangelog();
    }

    /// <summary>
    /// Saves the application changelog into the provided file.
    /// </summary>
    /// <param name="changelog">Data to be saved in the XML.</param>
    /// <param name="filePath">XML file path to save the data.</param>
    public static void SaveToXmlFile(AppChangelog changelog, string filePath)
    {
        var whatsNew = changelog.ConvertToWhatsNew();
        XmlUtils.SerializeToFile(whatsNew, filePath);
    }

    /// <summary>
    /// Asynchronous version of SaveToXmlFile method.
    /// </summary>
    /// <see cref="SaveToXmlFile(AppChangelog, string)"/>
    /// <param name="changelog">Data to be saved in the XML.</param>
    /// <param name="filePath">XML file path to save the data.</param>
    public static async Task SaveToXmlFileAsync(AppChangelog changelog, string filePath)
    {
        var whatsNew = changelog.ConvertToWhatsNew();
        await XmlUtils.SerializeToFileAsync(whatsNew, filePath);
    }

    private static void ValidateXmlFile(string filePath)
    {
        ValidateFilePath(filePath);

        try
        {
            XmlUtils.ValidateAgainstSchema(filePath);
        }
        catch (Exception exception)
        {
            throw new InvalidXmlException($"Provided file {filePath} is not valid XML. Check inner exception for details.", exception);
        }
    }

    private static async Task ValidateXmlFileAsync(string filePath)
    {
        ValidateFilePath(filePath);

        try
        {
            await XmlUtils.ValidateAgainstSchemaAsync(filePath);
        }
        catch (Exception exception)
        {
            throw new InvalidXmlException($"Provided file {filePath} is not valid XML. Check inner exception for details.", exception);
        }
    }

    private static void ValidateFilePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));
    }
}   
