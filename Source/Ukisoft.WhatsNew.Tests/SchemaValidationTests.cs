using System.Xml;
using System.Threading.Tasks;
using Ukisoft.WhatsNew.Exceptions;
using Ukisoft.WhatsNew.Utils;
using Xunit;

namespace Ukisoft.WhatsNew.Tests;

public class SchemaValidationTests
{
    [Theory]
    [InlineData("WhatsNewCorrectFromSavedObject.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandom.xml")]
    public void ValidateCorrectXmlFile(string filePath)
    {
        XmlUtils.ValidateAgainstSchema($@"Data\{filePath}");
    }

    [Theory]
    [InlineData("WhatsNewCorrectFromSavedObject.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandom.xml")]
    public async Task ValidateCorrectXmlFileAsync(string filePath)
    {
        await XmlUtils.ValidateAgainstSchemaAsync($@"Data\{filePath}");
    }

    [Fact]
    public void ValidateIncorrectXmlFileEmpty()
    {
        static void ValidateAgainstSchema() => XmlUtils.ValidateAgainstSchema($@"Data\WhatsNewIncorrectEmpty.xml");
        var exception = Assert.Throws<XmlException>(() => ValidateAgainstSchema());
        Assert.Equal("Root element is missing.", exception.Message);
    }

    [Fact]
    public void ValidateIncorrectXmlFileNoBugfixes()
    {
        static void ValidateAgainstSchema() => XmlUtils.ValidateAgainstSchema($@"Data\WhatsNewIncorrectNoBugfixes.xml");
        var exception = Assert.Throws<InvalidXmlException>(() => ValidateAgainstSchema());
        Assert.True(exception.Message.Contains("'InVersion'") && exception.Message.Contains("'Bugfixes'"));
    }

    [Fact]
    public void ValidateIncorrectXmlFileNoFeatures()
    {
        static void ValidateAgainstSchema() => XmlUtils.ValidateAgainstSchema($@"Data\WhatsNewIncorrectNoFeatures.xml");
        var exception = Assert.Throws<InvalidXmlException>(() => ValidateAgainstSchema());
        Assert.True(exception.Message.Contains("'InVersion'") && exception.Message.Contains("'Features'"));
    }

    [Fact]
    public void ValidateIncorrectXmlFileNoVersion()
    {
        static void ValidateAgainstSchema() => XmlUtils.ValidateAgainstSchema($@"Data\WhatsNewIncorrectNoVersion.xml");
        var exception = Assert.Throws<InvalidXmlException>(() => ValidateAgainstSchema());
        Assert.Equal("Validation error: The required attribute 'number' is missing.", exception.Message);
    }

    [Fact]
    public async Task ValidateIncorrectXmlFileEmptyAsync()
    {
        static async Task ValidateAgainstSchemaAsync() => await XmlUtils.ValidateAgainstSchemaAsync($@"Data\WhatsNewIncorrectEmpty.xml");
        var exception = await Assert.ThrowsAsync<XmlException>(async () => await ValidateAgainstSchemaAsync());
        Assert.Equal("Root element is missing.", exception.Message);
    }

    [Fact]
    public async Task ValidateIncorrectXmlFileNoBugfixesAsync()
    {
        static async Task ValidateAgainstSchemaAsync() => await XmlUtils.ValidateAgainstSchemaAsync($@"Data\WhatsNewIncorrectNoBugfixes.xml");
        var exception = await Assert.ThrowsAsync<InvalidXmlException>(async () => await ValidateAgainstSchemaAsync());
        Assert.True(exception.Message.Contains("'InVersion'") && exception.Message.Contains("'Bugfixes'"));
    }

    [Fact]
    public async Task ValidateIncorrectXmlFileNoFeaturesAsync()
    {
        static async Task ValidateAgainstSchemaAsync() => await XmlUtils.ValidateAgainstSchemaAsync($@"Data\WhatsNewIncorrectNoFeatures.xml");
        var exception = await Assert.ThrowsAsync<InvalidXmlException>(async () => await ValidateAgainstSchemaAsync());
        Assert.True(exception.Message.Contains("'InVersion'") && exception.Message.Contains("'Features'"));
    }

    [Fact]
    public async Task ValidateIncorrectXmlFileNoVersionAsync()
    {
        static async Task ValidateAgainstSchemaAsync() => await XmlUtils.ValidateAgainstSchemaAsync($@"Data\WhatsNewIncorrectNoVersion.xml");
        var exception = await Assert.ThrowsAsync<InvalidXmlException>(async () => await ValidateAgainstSchemaAsync());
        Assert.Equal("Validation error: The required attribute 'number' is missing.", exception.Message);
    }
}
