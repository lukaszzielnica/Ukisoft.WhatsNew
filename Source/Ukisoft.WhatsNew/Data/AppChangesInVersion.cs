namespace Ukisoft.WhatsNew.Data;

/// <summary>
/// Encapsulates application changes (features and bugfixes) for a specific application version.
/// </summary>
public class AppChangesInVersion
{
    /// <summary>
    /// Application version.
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Application features developed for the current application version.
    /// </summary>
    public List<AppChangeDescription> Features { get; internal set; } = new List<AppChangeDescription>();

    /// <summary>
    /// Application bugfixes developed for the current applcation version.
    /// </summary>
    public List<AppChangeDescription> Bugfixes { get; internal set; } = new List<AppChangeDescription>();
}
