using System.Text;
using Ukisoft.WhatsNew.Data;

namespace Ukisoft.WhatsNew;

/// <summary>
/// Parser service which parses application changelog into the string instance.
/// </summary>
public sealed class WhatsNewStringParserService : WhatsNewBaseParserService<string>
{
    /// <summary>
    /// Method parses the application changelog and saves the result in the string instance.
    /// </summary>
    /// <param name="changelog">Application changelog.</param>
    /// <param name="textDisplayer">String instance, to store parsed changelog.</param>
    public override void ParseWhatsNew(AppChangelog changelog, ref string textDisplayer)
    {
        var stringBuilder = new StringBuilder();
        foreach (var changeInVersion in changelog.ChangesInVersion)
        {
            var tempTextDisplayer = string.Empty;
            ParseWhatsNew(changeInVersion, ref tempTextDisplayer);
            stringBuilder.Append(tempTextDisplayer);
        }

        textDisplayer = stringBuilder.ToString().TrimEnd();
    }

    /// <summary>
    /// Method parses the application changelog for a specific version and saves the result in the string instance.
    /// </summary>
    /// <param name="changelog">Application changelog for a specific version.</param>
    /// <param name="textDisplayer">String instance, to store parsed changelog.</param>
    public override void ParseWhatsNew(AppChangesInVersion changelog, ref string textDisplayer)
    {
        var stringBuilder = new StringBuilder();
        var hasFeatures = changelog.Features.Any();
        var hasBugfixes = changelog.Bugfixes.Any();

        if (hasFeatures || hasBugfixes)
        {
            stringBuilder.AppendLine($"v{changelog.Version}{Environment.NewLine}");

            if (hasFeatures)
                stringBuilder.AppendLine(ParseChanges(changelog.Features, FeaturesText));
            else if (!string.IsNullOrEmpty(TextDisplayerSettings.MessageIfNoFeatures))
                stringBuilder.AppendLine($"{FeaturesText}{Environment.NewLine}{TextDisplayerSettings.MessageIfNoFeatures}{Environment.NewLine}");

            if (hasBugfixes)
                stringBuilder.AppendLine(ParseChanges(changelog.Bugfixes, BugfixesText));
            else if (!string.IsNullOrEmpty(TextDisplayerSettings.MessageIfNoBugfixes))
                stringBuilder.AppendLine($"{BugfixesText}{Environment.NewLine}{TextDisplayerSettings.MessageIfNoBugfixes}{Environment.NewLine}");

            stringBuilder.AppendLine();
        }
        else if (!string.IsNullOrEmpty(TextDisplayerSettings.MessageIfNoChangesInVersion))
        {
            stringBuilder.AppendLine($"v{changelog.Version}{Environment.NewLine}");
            stringBuilder.AppendLine($"{TextDisplayerSettings.MessageIfNoChangesInVersion}");
            stringBuilder.AppendLine($"{Environment.NewLine}");
        }

        textDisplayer += stringBuilder.ToString();
    }

    private string ParseChanges(IEnumerable<AppChangeDescription> changes, string changesTitle)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{changesTitle}");

        var counter = 1;
        foreach (var changeDescription in changes)
        {
            if (TextDisplayerSettings.ChangesNumbering)
                stringBuilder.Append($"{counter}. ");
            
            stringBuilder.AppendLine($"{changeDescription[TextDisplayerSettings.Language]}");
            counter++;
        }

        return stringBuilder.ToString();
    }
}
