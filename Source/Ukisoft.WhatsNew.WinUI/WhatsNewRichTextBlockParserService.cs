using System.Linq;
using System.Collections.Generic;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Ukisoft.WhatsNew.Data;

namespace Ukisoft.WhatsNew.WinUI;

/// <summary>
/// Parser service which parses application changelog into the RichTextBlock instance.
/// </summary>
public sealed class WhatsNewRichTextBlockParserService : WhatsNewBaseParserService<RichTextBlock>
{
    /// <summary>
    /// Method parses the application changelog and saves the result in the RichTextBlock instance.
    /// </summary>
    /// <param name="changelog">Application changelog.</param>
    /// <param name="textDisplayer">RichTextBlock instance, to store parsed changelog.</param>
    public override void ParseWhatsNew(AppChangelog changelog, ref RichTextBlock textDisplayer)
    {
        foreach (var changeInVersion in changelog.ChangesInVersion)
        {
            ParseWhatsNew(changeInVersion, ref textDisplayer);
        }
    }

    /// <summary>
    /// Method parses the application changelog for a specific version and saves the result in the RichTextBlock instance.
    /// </summary>
    /// <param name="changelog">Application changelog for a specific version.</param>
    /// <param name="textDisplayer">RichTextBlock instance, to store parsed changelog.</param>
    public override void ParseWhatsNew(AppChangesInVersion changelog, ref RichTextBlock textDisplayer)
    {
        var hasFeatures = changelog.Features.Any();
        var hasBugfixes = changelog.Bugfixes.Any();

        var paragraph = new Paragraph();

        if (hasFeatures || hasBugfixes)
        {
            AddVersionInfo(paragraph, changelog.Version);

            if (hasFeatures)
                AddChanges(paragraph, changelog.Features, FeaturesText);
            else if (!string.IsNullOrEmpty(TextDisplayerSettings.MessageIfNoFeatures))
                AddChanges(paragraph, TextDisplayerSettings.MessageIfNoFeatures, FeaturesText);

            if (hasBugfixes)
                AddChanges(paragraph, changelog.Bugfixes, BugfixesText);
            else if (!string.IsNullOrEmpty(TextDisplayerSettings.MessageIfNoBugfixes))
                AddChanges(paragraph, TextDisplayerSettings.MessageIfNoBugfixes, BugfixesText);

            paragraph.Inlines.Add(new LineBreak());
        }
        else if (!string.IsNullOrEmpty(TextDisplayerSettings.MessageIfNoChangesInVersion))
        {
            AddVersionInfo(paragraph, changelog.Version);
            AddSimpleMessage(paragraph, TextDisplayerSettings.MessageIfNoChangesInVersion);
            paragraph.Inlines.Add(new LineBreak());
        }

        paragraph.Inlines.Add(new LineBreak());
        textDisplayer.Blocks.Add(paragraph);
    }

    private static void AddVersionInfo(Paragraph paragraph, string version)
    {
        paragraph.Inlines.Add(new Run
        {
            FontWeight = FontWeights.Normal,
            FontSize = 20,
            Text = $"v{version}",
        });
    }

    private void AddChanges(Paragraph paragraph, IEnumerable<AppChangeDescription> changes, string changesTitle)
    {
        paragraph.Inlines.Add(new LineBreak());
        paragraph.Inlines.Add(new LineBreak());
        paragraph.Inlines.Add(GetChangesSpan(changes, changesTitle));
    }

    private Span GetChangesSpan(IEnumerable<AppChangeDescription> changes, string changesTitle)
    {
        var span = new Span();

        AddChangesTitle(span, changesTitle);
        span.Inlines.Add(new LineBreak());
        span.Inlines.Add(new LineBreak());

        var counter = 1;
        foreach (var change in changes)
        {
            span.Inlines.Add(new Run() { Text = GetChangeDescriptionText(change, counter) });
            span.Inlines.Add(new LineBreak());
            counter++;
        }

        return span;
    }

    private static void AddChanges(Paragraph paragraph, string noChangesMessage, string changesTitle)
    {
        paragraph.Inlines.Add(new LineBreak());
        paragraph.Inlines.Add(new LineBreak());
        paragraph.Inlines.Add(GetChangesSpan(noChangesMessage, changesTitle));
    }

    private static Span GetChangesSpan(string noChangesMessage, string changesTitle)
    {
        var span = new Span();

        AddChangesTitle(span, changesTitle);
        span.Inlines.Add(new LineBreak());
        span.Inlines.Add(new LineBreak());

        span.Inlines.Add(new Run() { Text = noChangesMessage });
        span.Inlines.Add(new LineBreak());

        return span;
    }

    private static void AddChangesTitle(Span span, string changesTitle)
    {
        span.Inlines.Add(new Run
        {
            FontWeight = FontWeights.Medium,
            FontSize = 18,
            Text = changesTitle,
        });
    }

    private static void AddSimpleMessage(Paragraph paragraph, string message)
    {
        paragraph.Inlines.Add(new LineBreak());
        paragraph.Inlines.Add(new LineBreak());
        paragraph.Inlines.Add(new Run { Text = message });
        paragraph.Inlines.Add(new LineBreak());
    }

    private string GetChangeDescriptionText(AppChangeDescription description, int counter)
    {
        var changeDescription = string.Empty;
        if (TextDisplayerSettings.ChangesNumbering)
            changeDescription = $"{counter}. ";

        changeDescription += $"{description[TextDisplayerSettings.Language]}";
        return changeDescription;
    }
}
