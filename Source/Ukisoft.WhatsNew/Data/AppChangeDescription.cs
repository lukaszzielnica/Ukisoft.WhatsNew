namespace Ukisoft.WhatsNew.Data;

/// <summary>
/// Description of a single change in the application.
/// TKey is language (xs:language datatype), TValue is language-specific change description.
/// </summary>
/// <example>
/// var changeDescription = new AppChangeDescription();
/// changeDescription.Add("en-US", "English description of the application change.");
/// changeDescription.Add("pl-PL", "Polski opis zmiany w aplikacji");
/// Console.WriteLine(changeDescription["en-US"]); // Displays english description.
/// Console.WriteLine(changeDescription["pl-PL"]); // Displays polish description.
/// </example>
public class AppChangeDescription : Dictionary<string, string>
{
}