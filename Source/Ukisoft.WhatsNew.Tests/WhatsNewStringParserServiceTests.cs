﻿using System.IO;
using System.Collections.Generic;
using Xunit;

namespace Ukisoft.WhatsNew.Tests;

public class WhatsNewStringParserServiceTests
{
    [Fact]
    public void ParseWhatsNewUsingDefaultSettings()
    {
        var changelog = Changelog.LoadFromXmlFile(@"Data\WhatsNewCorrectFromSavedObject.xml");
        
        string whatsNewText = string.Empty;
        _service.TextDisplayerSettings = TextDisplayerSettings.Default;
        _service.ParseWhatsNew(changelog, ref whatsNewText);

        var expectedResult = GetExpectedServiceResult("WhatsNewCorrectFromSavedObjectSettingsDefault.txt");
        Assert.Equal(expectedResult, whatsNewText);
    }

    [Theory]
    [MemberData(nameof(TestSettings))]
    public void ParseWhatsNewUsingModifiedSettings(TestTextDisplayerSettings settings)
    {
        var changelog = Changelog.LoadFromXmlFile(@"Data\WhatsNewCorrectFromSavedObject.xml");

        string whatsNewText = string.Empty;
        _service.TextDisplayerSettings = settings;
        _service.ParseWhatsNew(changelog, ref whatsNewText);

        var expectedResult = GetExpectedServiceResult(settings.ExpectedResultFile);
        Assert.Equal(expectedResult, whatsNewText);
    }

    public static IEnumerable<object[]> TestSettings => new List<object[]>()
    {
        new object[] 
        { 
            new TestTextDisplayerSettings()
            {
                ChangesNumbering = false,
                MessageIfNoFeatures = string.Empty,
                MessageIfNoBugfixes = string.Empty,
                MessageIfNoChangesInVersion = string.Empty,
                ExpectedResultFile = "WhatsNewCorrectFromSavedObjectSettings1.txt",
            }
        },
        new object[] 
        { 
            new TestTextDisplayerSettings()
            {
                ChangesNumbering = true,
                MessageIfNoFeatures = "No new features were added in this version. Stay tuned!",
                MessageIfNoBugfixes = string.Empty,
                MessageIfNoChangesInVersion = string.Empty,
                ExpectedResultFile = "WhatsNewCorrectFromSavedObjectSettings2.txt",
            }
        },
        new object[] 
        {
            new TestTextDisplayerSettings()
            {
                ChangesNumbering = true,
                MessageIfNoFeatures = string.Empty,
                MessageIfNoBugfixes = "The application is bugless! Come'on :)",
                MessageIfNoChangesInVersion = string.Empty,
                ExpectedResultFile = "WhatsNewCorrectFromSavedObjectSettings3.txt",
            }
        },
        new object[] 
        { 
            new TestTextDisplayerSettings()
            {
                ChangesNumbering = true,
                MessageIfNoFeatures = "Blah blah",
                MessageIfNoBugfixes = "Shall be the one",
                MessageIfNoChangesInVersion = "No changes at all",
                ExpectedResultFile = "WhatsNewCorrectFromSavedObjectSettings4.txt",
            }
        },
    };

    public class TestTextDisplayerSettings : TextDisplayerSettings
    {
        public string ExpectedResultFile { get; set; } = string.Empty;
    }

    private static string GetExpectedServiceResult(string fileName)
    {
        var filePath = $@"Data\WhatsNewStringParserServiceResults\{fileName}";
        return File.ReadAllText(filePath); 
    }

    private readonly WhatsNewStringParserService _service = new();
}
