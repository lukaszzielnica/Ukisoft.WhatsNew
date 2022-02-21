using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ukisoft.WhatsNew.Utils;
using Ukisoft.WhatsNew.Data.Internal;
using Xunit;

namespace Ukisoft.WhatsNew.Tests;

public class SerializationTests : IDisposable
{
    [Fact]
    public void SerializeCorrectObjectToText()
    {
        var text = XmlUtils.SerializeToText(_correctObject);
        AssertTextAsXml(text);
    }

    [Fact]
    public void SerializeEmptyObjectToText()
    {
        var text = XmlUtils.SerializeToText(_emptyObject);
        AssertTextAsXml(text);
    }

    [Fact]
    public void SerializeCorrectObjectToXmlFile()
    {
        var xmlFile = $"{nameof(SerializeCorrectObjectToXmlFile)}.xml";
        XmlUtils.SerializeToFile(_correctObject, xmlFile);

        var text = File.ReadAllText(xmlFile);
        AssertTextAsXml(text);
    }

    [Fact]
    public void SerializeEmptyObjectToXmlFile()
    {
        var xmlFile = $"{nameof(SerializeEmptyObjectToXmlFile)}.xml";
        XmlUtils.SerializeToFile(_emptyObject, xmlFile);

        var text = File.ReadAllText(xmlFile);
        AssertTextAsXml(text);
    }

    [Fact]
    public async Task SerializeCorrectObjectToXmlFileAsync()
    {
        var xmlFile = $"{nameof(SerializeCorrectObjectToXmlFileAsync)}.xml";
        await XmlUtils.SerializeToFileAsync(_correctObject, xmlFile);

        var text = await File.ReadAllTextAsync(xmlFile);
        await AssertTextAsXmlAsync(text);
    }

    [Fact]
    public async Task SerializeEmptyObjectToXmlFileAsync()
    {
        var xmlFile = $"{nameof(SerializeEmptyObjectToXmlFileAsync)}.xml";
        await XmlUtils.SerializeToFileAsync(_emptyObject, xmlFile);

        var text = await File.ReadAllTextAsync(xmlFile);
        await AssertTextAsXmlAsync(text);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        File.Delete($"{nameof(SerializeCorrectObjectToXmlFile)}.xml");
        File.Delete($"{nameof(SerializeEmptyObjectToXmlFile)}.xml");
        File.Delete($"{nameof(SerializeCorrectObjectToXmlFileAsync)}.xml");
        File.Delete($"{nameof(SerializeEmptyObjectToXmlFileAsync)}.xml");
    }

    private static void AssertTextAsXml(string text)
    {
        Assert.NotEmpty(text);
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        while (xmlReader.Read());
    }

    private static async Task AssertTextAsXmlAsync(string text)
    {
        Assert.NotEmpty(text);
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings() { Async = true });
        while (await xmlReader.ReadAsync());
    }

    private readonly WhatsNewRoot _correctObject = new()
    {
        Versions = new List<InVersion>
        {
            new InVersion()
            {
                Number = "0.4.22237.1",
                Features = new List<Change>
                {
                    new Change() { Descriptions = new List<Description> { new Description() { Language = "en-US", Value = "This is some description in english." } } },
                    new Change() { Descriptions = new List<Description> { new Description() { Language = "en-US", Value = "This is some other description in english." } } }
                },
                Bugfixes = new List<Change>
                {
                    new Change() { Descriptions = new List<Description> { new Description() { Language = "en-US", Value = "This is bug fix." } } },
                },
            },
            new InVersion()
            {
                Number = "0.3.22112.1",
                Features = new List<Change>
                {
                    new Change() { Descriptions = new List<Description> { new Description() { Language = "en-US", Value = "Only features in this release." } } },
                },
                Bugfixes = new List<Change>(),
            },
            new InVersion()
            {
                Number = "0.2.22031.2",
                Features = new List<Change>(),
                Bugfixes = new List<Change>
                {
                    new Change() { Descriptions = new List<Description> { new Description() { Language = "en-US", Value = "Only bugfixes in this version." } } },
                    new Change() { Descriptions = new List<Description> { new Description() { Language = "en-US", Value = "Some bugfix." } } },
                },
            },
            new InVersion()
            {
                Number = "0.1.22001.1",
                Features = new List<Change>(),
                Bugfixes = new List<Change>(),
            },
        }
    };

    private readonly WhatsNewRoot _emptyObject = new();
}

