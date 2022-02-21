using Ukisoft.WhatsNew.Data;
using Ukisoft.WhatsNew.Data.Internal;

namespace Ukisoft.WhatsNew.Utils;

internal static class DataConverters
{
    internal static AppChangelog ConvertToChangelog(this WhatsNewRoot whatsNew)
    {
        var changelog = new AppChangelog();

        foreach (var version in whatsNew.Versions)
        {
            var changesInVersion = new AppChangesInVersion() { Version = version.Number };
            CopyChanges(version.Features, changesInVersion.Features);
            CopyChanges(version.Bugfixes, changesInVersion.Bugfixes);
            changelog.ChangesInVersion.Add(changesInVersion);
        }

        return changelog;
    }

    internal static WhatsNewRoot ConvertToWhatsNew(this AppChangelog changelog)
    {
        var whatsNew = new WhatsNewRoot();

        foreach (var changeInVersion in changelog.ChangesInVersion)
        {
            var inVersion = new InVersion() { Number = changeInVersion.Version };
            CopyChanges(changeInVersion.Features, inVersion.Features);
            CopyChanges(changeInVersion.Bugfixes, inVersion.Bugfixes);
            whatsNew.Versions.Add(inVersion);
        }

        return whatsNew;
    }

    private static void CopyChanges(IEnumerable<Change> source, IList<AppChangeDescription> destination)
    {
        foreach (var change in source)
        {
            var appChange = new AppChangeDescription();

            foreach (var description in change.Descriptions)
            {
                appChange.Add(description.Language, description.Value);
            }

            destination.Add(appChange);
        }
    }

    private static void CopyChanges(IEnumerable<AppChangeDescription> source, IList<Change> destination)
    {
        foreach (var appChange in source)
        {
            var change = new Change();

            foreach (var appDescription in appChange)
            {
                var description = new Description
                {
                    Language = appDescription.Key, 
                    Value = appDescription.Value,
                };

                change.Descriptions.Add(description);
            }

            destination.Add(change);
        }
    }
}
