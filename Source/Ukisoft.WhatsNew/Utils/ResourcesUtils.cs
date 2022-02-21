using System.Reflection;
using System.Xml.Schema;

namespace Ukisoft.WhatsNew.Utils;

internal static class ResourcesUtils
{
    internal static XmlSchema ReadXsdResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var resourceStream = assembly.GetManifestResourceStream(resourceName);
        if (resourceStream == null)
            throw new FileNotFoundException($"Embedded resource file {resourceName} was not found.");

        using var streamReader = new StreamReader(resourceStream);

        var xmlSchema = XmlSchema.Read(streamReader, null);
        if (xmlSchema == null)
            throw new XmlSchemaException($"Embedded XML Schema could not be retrieved.");

        return xmlSchema;
    }
}
