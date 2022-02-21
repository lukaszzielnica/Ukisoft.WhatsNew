using Ukisoft.WhatsNew.Data;

namespace Ukisoft.WhatsNew;

/// <summary>
/// Generic text parser service which allows to parse the changelog and display it using the text displayer instance.
/// </summary>
/// <typeparam name="TextDisplayer">Type which is meant to store parsed text.</typeparam>
public interface IWhatsNewTextParserService<TextDisplayer> 
    where TextDisplayer : notnull
{
    /// <summary>
    /// Text displayer settings used in the ParseWhatsNew methods to change the way in which text is being parsed and/or displayed.
    /// </summary>
    TextDisplayerSettings TextDisplayerSettings { get; protected set; }

    /// <summary>
    /// Method parses the application changelog and displays it using the text displayer instance.
    /// </summary>
    /// <param name="changelog">Application changelog.</param>
    /// <param name="textDisplayer">Text displayer, to store parsed changelog.</param>
    void ParseWhatsNew(AppChangelog changelog, ref TextDisplayer textDisplayer);

    /// <summary>
    /// Method parses the application changelog for a specific version and displays it using the text displayer instance.
    /// </summary>
    /// <param name="changelog">Application changelog for a specific version.</param>
    /// <param name="textDisplayer">Text displayer, to store parsed changelog.</param>
    void ParseWhatsNew(AppChangesInVersion changelog, ref TextDisplayer textDisplayer);
}