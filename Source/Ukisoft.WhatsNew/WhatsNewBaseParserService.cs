using Ukisoft.WhatsNew.Data;

namespace Ukisoft.WhatsNew;

/// <summary>
/// Base implementation of IWhatsNewTextParserService with the default TextDisplayerSettings property implemented.
/// </summary>
/// <typeparam name="TextDisplayer">Type which is meant to store parsed text.</typeparam>
public abstract class WhatsNewBaseParserService<TextDisplayer> : IWhatsNewTextParserService<TextDisplayer>
    where TextDisplayer : notnull
{
    private TextDisplayerSettings _settings = TextDisplayerSettings.Default;

    /// <inheritdoc/>
    public virtual TextDisplayerSettings TextDisplayerSettings 
    { 
        get => _settings;
        set => _settings = value;
    }

    /// <inheritdoc/>
    public abstract void ParseWhatsNew(AppChangelog changelog, ref TextDisplayer textDisplayer);

    /// <inheritdoc/>
    public abstract void ParseWhatsNew(AppChangesInVersion changelog, ref TextDisplayer textDisplayer);
}
