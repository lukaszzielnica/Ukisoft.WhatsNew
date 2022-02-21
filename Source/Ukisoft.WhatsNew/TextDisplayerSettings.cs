namespace Ukisoft.WhatsNew;

/// <summary>
/// Text displayer settings.
/// </summary>
public class TextDisplayerSettings
{
    /// <summary>
    /// Flag decides whether application changes (features and bugfixes) should be numbered when displayed in the text displayer instance.
    /// </summary>
    public bool ChangesNumbering { get; set; } = true;

    /// <summary>
    /// Message being hold by the text displayer when there are no new features added in the current version (based on the application changelog provided).
    /// If empty, the whole features section is not displayed.
    /// </summary>
    public string MessageIfNoFeatures { get; set; } = string.Empty;

    /// <summary>
    /// Message being hold by the text displayer when there are no new bugfixes added in the current version (based on the application changelog provided).
    /// If empty, the whole bugfixes section is not displayed.
    /// </summary>
    public string MessageIfNoBugfixes { get; set; } = string.Empty;

    /// <summary>
    /// Message being hold by the text displayer when there are no changes (from the user's perspective, i.e. bugfixes or features) added in the current version (based on the application changelog provided).
    /// If empty, the whole version section is not displayed.
    /// </summary>
    public string MessageIfNoChangesInVersion { get; set; } = string.Empty;

    /// <summary>
    /// Default text displayer settings.
    /// </summary>
    public static TextDisplayerSettings Default => new();
}
