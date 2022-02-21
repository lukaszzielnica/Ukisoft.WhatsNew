namespace Ukisoft.WhatsNew.Data;

/// <summary>
/// Changelog of the application.
/// </summary>
public class AppChangelog
{
    /// <summary>
    /// Gets changelog for a given application version.
    /// </summary>
    /// <param name="version">Application version.</param>
    /// <returns>Application changelog for a given application version.</returns>
    public AppChangesInVersion this[string version] => ChangesInVersion.First(v => v.Version == version);

    /// <summary>
    /// List of all application versions concerned by the changelog.
    /// </summary>
    public List<AppChangesInVersion> ChangesInVersion { get; internal set; } = new List<AppChangesInVersion>();
}
