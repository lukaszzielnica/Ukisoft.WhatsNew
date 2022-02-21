using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Ukisoft.WhatsNew.Exceptions;

namespace Ukisoft.WhatsNew.Utils;

internal static class XmlUtils
{
    internal static string SerializeToText<TObject>(TObject obj)
        where TObject : class
    {
        using var stringWriter = new StringWriter();
        using var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
        new XmlSerializer(typeof(TObject)).Serialize(xmlTextWriter, obj);
        return stringWriter.ToString();
    }

    internal static void SerializeToFile<TObject>(TObject obj, string xmlFilePath)
        where TObject : class
    {
        var xmlText = SerializeToText(obj);
        File.WriteAllText(xmlFilePath, xmlText);
    }

    internal static async Task SerializeToFileAsync<TObject>(TObject obj, string xmlFilePath)
        where TObject : class
    {
        var xmlText = SerializeToText(obj);
        await File.WriteAllTextAsync(xmlFilePath, xmlText);
    }

    internal static TObject DeserializeFromText<TObject>(string xmlText)
       where TObject : class
    {
        var xmlSerializer = new XmlSerializer(typeof(TObject));
        using var textReader = new StringReader(xmlText);
        var deserializedObj = xmlSerializer.Deserialize(textReader);
        return (TObject)(deserializedObj ?? throw new InvalidCastException("Deserialization returned null object."));
    }

    internal static TObject DeserializeFromFile<TObject>(string xmlFilePath)
        where TObject : class
    {
        var xmlSerializer = new XmlSerializer(typeof(TObject));
        using var streamReader = new StreamReader(xmlFilePath);
        var deserializedObj = xmlSerializer.Deserialize(streamReader);
        return (TObject)(deserializedObj ?? throw new InvalidCastException("Deserialization returned null object."));
    }

    internal static async Task<TObject> DeserializeFromFileAsync<TObject>(string xmlFilePath)
        where TObject : class
    {
        var xmlText = await File.ReadAllTextAsync(xmlFilePath);
        return DeserializeFromText<TObject>(xmlText);
    }

    internal static void ValidateAgainstSchema(string xmlFilePath)
    {
        using var stringReader = new StringReader(File.ReadAllText(xmlFilePath));
        using var reader = XmlReader.Create(stringReader, GetXmlReaderSettings(async: false));
        while (reader.Read());
    }

    internal static async Task ValidateAgainstSchemaAsync(string xmlFilePath)
    {
        using var stringReader = new StringReader(await File.ReadAllTextAsync(xmlFilePath));
        using var reader = XmlReader.Create(stringReader, GetXmlReaderSettings(async: true));
        while (await reader.ReadAsync());
    }

    private static XmlReaderSettings GetXmlReaderSettings(bool async)
    {
        var settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            Async = async,
        };

        settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
        settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
        settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

        var xsdSchema = ResourcesUtils.ReadXsdResource("Ukisoft.WhatsNew.Data.Internal.WhatsNew.xsd");
        settings.Schemas.Add(xsdSchema);

        settings.ValidationEventHandler += (object? _, ValidationEventArgs e) =>
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    throw new InvalidXmlException($"Validation error: {e.Message}", e.Exception);              
                case XmlSeverityType.Warning:
                    throw new InvalidXmlException($"Warning: Matching schema not found. No validation occurred. {e.Message}", e.Exception);
                default:
                    throw new InvalidOperationException();
            }
        };

        return settings;
    }
}
