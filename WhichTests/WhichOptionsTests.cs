using WhichLib.Data;

namespace WhichTests;

[TestFixture]
public class WhichOptionsTests
{
    private readonly FileNamesToSearch _testFiles = FileNamesToSearch.FromStrings(new[] { "ruby", "python" });

    [Test]
    public void ShouldCreateDefaultOptions()
    {
        var options = WhichOptions.Default(_testFiles);
        
        Assert.That(options.Output, Is.EqualTo(OutputMode.Normal));
        Assert.That(options.Search, Is.EqualTo(SearchMode.FirstMatch));
    }

    [Test]
    public void ShouldCreateSilentOptions()
    {
        var options = WhichOptions.Silent(_testFiles);
        
        Assert.That(options.Output, Is.EqualTo(OutputMode.Silent));
        Assert.That(options.Search, Is.EqualTo(SearchMode.FirstMatch));
    }

    [Test]
    public void ShouldCreateShowAllOptions()
    {
        var options = WhichOptions.ShowAll(_testFiles);
        
        Assert.That(options.Output, Is.EqualTo(OutputMode.Normal));
        Assert.That(options.Search, Is.EqualTo(SearchMode.AllMatches));
    }

    [Test]
    public void ShouldCreateSilentShowAllOptions()
    {
        var options = WhichOptions.SilentShowAll(_testFiles);
        
        Assert.That(options.Output, Is.EqualTo(OutputMode.Silent));
        Assert.That(options.Search, Is.EqualTo(SearchMode.AllMatches));
    }

    [Test]
    public void ShouldDetectVerboseMode()
    {
        var options = new WhichOptions(OutputMode.Verbose, SearchMode.FirstMatch, _testFiles);
        
        Assert.That(options.IsVerbose, Is.True);
    }

    // Legacy constructor test removed - no longer supporting backward compatibility

    [Test]
    public void ShouldWorkWithNewWhichOptionsConstructor()
    {
        var whichOptions = WhichOptions.SilentShowAll(_testFiles);
        var settings = new Settings(whichOptions);
        
        Assert.That(settings.Options.Output, Is.EqualTo(OutputMode.Silent));
        Assert.That(settings.Options.Search, Is.EqualTo(SearchMode.AllMatches));
        Assert.That(settings.Options.FilesToSearch.Names.Length, Is.EqualTo(2));
        Assert.That(settings.Options.FilesToSearch.Names[0].ToString(), Is.EqualTo("ruby"));
    }
}