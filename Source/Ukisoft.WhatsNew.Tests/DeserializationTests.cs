using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ukisoft.WhatsNew.Data.Internal;
using Ukisoft.WhatsNew.Utils;
using Xunit;

namespace Ukisoft.WhatsNew.Tests;

public class DeserializationTests
{
    [Theory]
    [InlineData("WhatsNewCorrectFromSavedObject.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandom.xml")]
    [InlineData("WhatsNewCorrectFromSavedObjectWithPolishResources.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandomWithPolishResources.xml")]
    public void DeserializeFromCorrectXmlFile(string filePath)
    { 
        var whatsNew = XmlUtils.DeserializeFromFile<WhatsNewRoot>($@"Data\{filePath}");
        
        Assert.NotNull(whatsNew);
        Assert.NotNull(whatsNew.Versions);
    }

    [Theory]
    [InlineData("WhatsNewCorrectFromSavedObject.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandom.xml")]
    [InlineData("WhatsNewCorrectFromSavedObjectWithPolishResources.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandomWithPolishResources.xml")]
    public void DeserializeFromText(string filePath)
    {
        var text = File.ReadAllText($@"Data\{filePath}");
        var whatsNew = XmlUtils.DeserializeFromText<WhatsNewRoot>(text);

        Assert.NotNull(whatsNew);
        Assert.NotNull(whatsNew.Versions);
    }

    [Theory]
    [InlineData("WhatsNewCorrectFromSavedObject.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandom.xml")]
    [InlineData("WhatsNewCorrectFromSavedObjectWithPolishResources.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandomWithPolishResources.xml")]
    public async Task DeserializeFromCorrectXmlFileAsync(string filePath)
    {
        var whatsNew = await XmlUtils.DeserializeFromFileAsync<WhatsNewRoot>($@"Data\{filePath}");
        
        Assert.NotNull(whatsNew);
        Assert.NotNull(whatsNew.Versions);
    }

    [Fact]
    public void DeserializeFromIncorrectXmlFileWithNoFeatures()
    {
        var whatsNew = XmlUtils.DeserializeFromFile<WhatsNewRoot>($@"Data\WhatsNewIncorrectNoFeatures.xml");

        Assert.NotNull(whatsNew);
        Assert.NotNull(whatsNew.Versions);

        var featuresCount = whatsNew.Versions.Sum(v => v.Features.Count);
        Assert.Equal(0, featuresCount);
    }

    [Fact]
    public void DeserializeFromIncorrectXmlFileWithNoBugfixes()
    {
        var whatsNew = XmlUtils.DeserializeFromFile<WhatsNewRoot>($@"Data\WhatsNewIncorrectNoBugfixes.xml");

        Assert.NotNull(whatsNew);
        Assert.NotNull(whatsNew.Versions);

        var featuresCount = whatsNew.Versions.Sum(v => v.Bugfixes.Count);
        Assert.Equal(0, featuresCount);
    }

    [Fact]
    public void DeserializeFromIncorrectXmlFileWithNoVersion()
    {
        var whatsNew = XmlUtils.DeserializeFromFile<WhatsNewRoot>($@"Data\WhatsNewIncorrectNoVersion.xml");

        Assert.NotNull(whatsNew);
        Assert.NotNull(whatsNew.Versions);

        var versionsTotalLength = whatsNew.Versions.Sum(v => v.Number.Length);
        Assert.Equal(0, versionsTotalLength);
    }

    [Fact]
    public void DeserializeFromIncorrectXmlFileEmptyXml()
    {
        static void action() => XmlUtils.DeserializeFromFile<WhatsNewRoot>($@"Data\WhatsNewIncorrectEmpty.xml");
        Assert.Throws<InvalidOperationException>(() => action());
    }

    private class DummyPrivateWhatsNew
    {
        public WhatsNewRoot Dummy { get; set; } = new();
    }

    [Fact]
    public void DeserializeToIncorrectPrivateObjectType()
    {
        static void action() => XmlUtils.DeserializeFromFile<DummyPrivateWhatsNew>($@"Data\WhatsNewCorrectFromSavedObject.xml");
        Assert.Throws<InvalidOperationException>(() => action());
    }

    private class DummyPublicWhatsNew
    {
        public WhatsNewRoot Dummy { get; set; } = new();
    }

    [Fact]
    public void DeserializeToIncorrectPublicObjectType()
    {
        static void action() => XmlUtils.DeserializeFromFile<DummyPublicWhatsNew>($@"Data\WhatsNewCorrectFromSavedObject.xml");
        Assert.Throws<InvalidOperationException>(() => action());
    }
}
