using System.Xml.Serialization;
namespace Ukisoft.WhatsNew.Data.Internal;

[XmlRoot(Namespace = "http://uki-soft.com/WhatsNew.xsd", ElementName = "WhatsNew")]
public sealed class WhatsNewRoot
{
    [XmlElement(ElementName = "InVersion")]
    public List<InVersion> Versions { get; set; } = new List<InVersion>();
}

public sealed class InVersion
{
    [XmlAttribute(AttributeName = "number")]
    public string Number { get; set; } = string.Empty;

    [XmlArrayItem(ElementName = "Feature")]
    public List<Change> Features { get; set; } = new List<Change>();

    [XmlArrayItem(ElementName = "Bugfix")]
    public List<Change> Bugfixes { get; set; } = new List<Change>();
}

public sealed class Change
{
    [XmlElement(ElementName = "Description")]
    public List<Description> Descriptions { get; set; } = new List<Description>();        
}

public sealed class Description
{
    [XmlAttribute(AttributeName = "language")]
    public string Language { get; set; } = string.Empty;
    [XmlAttribute(AttributeName = "value")]
    public string Value { get; set; } = string.Empty;
}