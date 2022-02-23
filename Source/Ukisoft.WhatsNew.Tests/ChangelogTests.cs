using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ukisoft.WhatsNew.Data;
using Ukisoft.WhatsNew.Exceptions;
using Xunit;

namespace Ukisoft.WhatsNew.Tests;

public class ChangelogTests : IDisposable
{
    [Theory]
    [InlineData("WhatsNewCorrectFromSavedObject.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandom.xml")]
    [InlineData("WhatsNewCorrectFromSavedObjectWithPolishResources.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandomWithPolishResources.xml")]
    public void LoadCorrectChangelogsFromXmlFile(string filePath)
    {
        var changelog = Changelog.LoadFromXmlFile($@"Data\{filePath}");
        Assert.NotNull(changelog);
    }

    [Theory]
    [InlineData("WhatsNewCorrectFromSavedObject.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandom.xml")]
    [InlineData("WhatsNewCorrectFromSavedObjectWithPolishResources.xml")]
    [InlineData("WhatsNewCorrectVisualStudioRandomWithPolishResources.xml")]
    public async Task LoadCorrectChangelogsFromXmlFileAsync(string filePath)
    {
        var changelog = await Changelog.LoadFromXmlFileAsync($@"Data\{filePath}");
        Assert.NotNull(changelog);
    }

    [Theory]
    [InlineData("WhatsNewIncorrectEmpty.xml")]
    [InlineData("WhatsNewIncorrectNoBugfixes.xml")]
    [InlineData("WhatsNewIncorrectNoFeatures.xml")]
    [InlineData("WhatsNewIncorrectNoVersion.xml")]
    public void LoadIncorrectChangelogsFromXmlFile(string filePath)
    {
        AppChangelog LoadChangelog() => Changelog.LoadFromXmlFile($@"Data\{filePath}");
        Assert.Throws<InvalidXmlException>(() => LoadChangelog());
    }

    [Theory]
    [InlineData("WhatsNewIncorrectEmpty.xml")]
    [InlineData("WhatsNewIncorrectNoBugfixes.xml")]
    [InlineData("WhatsNewIncorrectNoFeatures.xml")]
    [InlineData("WhatsNewIncorrectNoVersion.xml")]
    public async Task LoadIncorrectChangelogsFromXmlFileAsync(string filePath)
    {
        async Task<AppChangelog> LoadChangelogAsync() => await Changelog.LoadFromXmlFileAsync($@"Data\{filePath}");
        await Assert.ThrowsAsync<InvalidXmlException>(async () => await LoadChangelogAsync());
    }

    [Fact]
    public void SaveCorrectChangelogToXmlFile()
    {
        var xmlFilePath = $"{nameof(SaveCorrectChangelogToXmlFile)}.xml";
        Changelog.SaveToXmlFile(_correctChangelog, xmlFilePath);

        var changelog = Changelog.LoadFromXmlFile(xmlFilePath);
        Assert.True(ChangelogsEquals(_correctChangelog, changelog));
    }

    [Fact]
    public void SaveEmptyChangelogToXmlFile()
    {
        var xmlFilePath = $"{nameof(SaveEmptyChangelogToXmlFile)}.xml";
        Changelog.SaveToXmlFile(_emptyChangelog, xmlFilePath);

        AppChangelog LoadChangelog() => Changelog.LoadFromXmlFile(xmlFilePath);
        Assert.Throws<InvalidXmlException>(() => LoadChangelog());
    }

    [Fact]
    public async Task SaveCorrectChangelogToXmlFileAsync()
    {
        var xmlFilePath = $"{nameof(SaveCorrectChangelogToXmlFile)}.xml";
        await Changelog.SaveToXmlFileAsync(_correctChangelog, xmlFilePath);

        var changelog = await Changelog.LoadFromXmlFileAsync(xmlFilePath);
        Assert.True(ChangelogsEquals(_correctChangelog, changelog));
    }

    [Fact]
    public async Task SaveEmptyChangelogToXmlFileAsync()
    {
        var xmlFilePath = $"{nameof(SaveEmptyChangelogToXmlFileAsync)}.xml";
        await Changelog.SaveToXmlFileAsync(_emptyChangelog, xmlFilePath);

        async Task<AppChangelog> LoadChangelogAsync() => await Changelog.LoadFromXmlFileAsync(xmlFilePath);
        await Assert.ThrowsAsync<InvalidXmlException>(async () => await LoadChangelogAsync());
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        File.Delete($"{nameof(SaveCorrectChangelogToXmlFile)}.xml");
        File.Delete($"{nameof(SaveEmptyChangelogToXmlFile)}.xml");
        File.Delete($"{nameof(SaveCorrectChangelogToXmlFileAsync)}.xml");
        File.Delete($"{nameof(SaveEmptyChangelogToXmlFileAsync)}.xml");
    }

    private static bool ChangelogsEquals(AppChangelog lhsChangelog, AppChangelog rhsChangelog)
    {
        if (lhsChangelog.ChangesInVersion.Count != rhsChangelog.ChangesInVersion.Count)
            return false;

        for (int i = 0; i < lhsChangelog.ChangesInVersion.Count; i++)
        {
            var lhsChangesInVersion = lhsChangelog.ChangesInVersion[i];
            var rhsChangesInVersion = rhsChangelog.ChangesInVersion[i];

            if (lhsChangesInVersion.Version != rhsChangesInVersion.Version)
                return false;

            if (!ChangesEquals(lhsChangesInVersion.Features, rhsChangesInVersion.Features))
                return false;

            if (!ChangesEquals(lhsChangesInVersion.Bugfixes, rhsChangesInVersion.Bugfixes))
                return false;
        }

        return true;
    }

    private static bool ChangesEquals(List<AppChangeDescription> lhsChanges, List<AppChangeDescription> rhsChanges)
    {
        if (lhsChanges.Count != rhsChanges.Count)
            return false;

        for (int i = 0; i < lhsChanges.Count; i++)
        {
            bool dictionariesEqual =
                lhsChanges[i].Keys.Count == rhsChanges[i].Keys.Count &&
                lhsChanges[i].Keys.All(k => rhsChanges[i].ContainsKey(k) && Equals(rhsChanges[i][k], lhsChanges[i][k]));

            if (!dictionariesEqual)
                return false;
        }

        return true;
    }

    private readonly AppChangelog _correctChangelog = new()
    {
        ChangesInVersion = new List<AppChangesInVersion>
        {
            new AppChangesInVersion()
            {
                Version = "0.4.22237.1",
                Features = new List<AppChangeDescription>
                {
                    new AppChangeDescription() { { "en-US", "This is some description in english." } },
                    new AppChangeDescription() { { "en-US", "This is some other description in english." } },
                },
                Bugfixes = new List<AppChangeDescription>
                {
                     new AppChangeDescription() { { "en-US",  "This is bug fix." } },
                },
            },
            new AppChangesInVersion()
            {
                Version = "0.3.22112.1",
                Features = new List<AppChangeDescription>
                {
                    new AppChangeDescription() { { "en-US", "Only features in this release." } },
                },
                Bugfixes = new List<AppChangeDescription>(),
            },
            new AppChangesInVersion()
            {
                Version = "0.2.22031.2",
                Features = new List<AppChangeDescription>(),
                Bugfixes = new List<AppChangeDescription>()
                {
                    new AppChangeDescription() { { "en-US", "Only bugfixes in this version." } },
                    new AppChangeDescription() { { "en-US", "Some bugfix." } },
                },
            },
            new AppChangesInVersion()
            {
                Version = "0.1.22001.1",
                Features = new List<AppChangeDescription>(),
                Bugfixes = new List<AppChangeDescription>(),
            },
        }
    };

    private readonly AppChangelog _emptyChangelog = new();
}
