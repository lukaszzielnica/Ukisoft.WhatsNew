using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace Ukisoft.WhatsNew.WinUI.Tests;

[TestClass]
public class WhatsNewRichTextBlockParserServiceTests
{
    [UITestMethod]
    public void ParseWhatsNewUsingDefaultSettings()
    {
        var changelog = Changelog.LoadFromXmlFile(@"Tests\Data\WhatsNewCorrectFromSavedObject.xml");

        var whatsNewRichTextBlock = new RichTextBlock();
        _service.TextDisplayerSettings = TextDisplayerSettings.Default;
        _service.ParseWhatsNew(changelog, ref whatsNewRichTextBlock);
    }

    private readonly WhatsNewRichTextBlockParserService _service = new();
}
