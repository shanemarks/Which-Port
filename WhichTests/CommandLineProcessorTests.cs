using System.Collections.Immutable;
using WhichLib.Data;
using WhichLib.Services;

namespace WhichTests;

public class CommandLineProcessorTests
{
    [TestCase("-a", true)]
    [TestCase("-s", true)]
    [TestCase("-sa", true)]
    [TestCase("-as", true)]
    [TestCase("-g", false)]
    [TestCase("", true)] //Empty string is valid


    public void ShouldAcceptOnlyValidOptions(string option, bool expected)
    {
        var result = CommandLineProcessor.ValidateOption(new NonNullableString(option));
        Assert.That(result.IsValid, Is.EqualTo(expected));
        
    }
    [TestCase ("-a", false, true,"python","ruby")]
    public void ShouldGenerateSettingsFromOptions(string options,  bool expectedSilent, bool expectedAll, params string[] files)
    {
        var convertedStrings = files.Select((x) => new NonNullableString(x)).ToImmutableArray();
        var commandOptions = new CommandOptions(new NonNullableString(options));
        var fileNamesToSearch = FileNamesToSearch.FromNonNullableStrings(convertedStrings);
        var settings = CommandLineProcessor.OptionsToSettings(commandOptions, fileNamesToSearch);
      Assert.That(settings.Value.ListAll, Is.EqualTo(true));
      Assert.That(settings.Value.SilentMode, Is.EqualTo(false));
      for (int n = 0; n < files.Length; n++)
      {
          Assert.That(settings.Value.Files[n].ToString(), Is.EqualTo(files[n]));
      }

    }
    
    [TestCase("-x", 'x')]
    [TestCase("-axb", 'x')]
    [TestCase("-sxy", 'x')]
    public void ShouldReportFirstInvalidCharacter(string option, char expectedInvalidChar)
    {
        var result = CommandLineProcessor.ValidateOption(new NonNullableString(option));
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.InvalidCharacter, Is.EqualTo(expectedInvalidChar));
    }
    
    [Test]
    public void ShouldGenerateProperErrorMessageForInvalidOption()
    {
        var commandOptions = new CommandOptions(new NonNullableString("-x"));
        var fileNamesToSearch = FileNamesToSearch.FromStrings(new[] { "ruby" });
        var result = CommandLineProcessor.OptionsToSettings(commandOptions, fileNamesToSearch);
        
        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.Error, Is.EqualTo("which: illegal option -- x"));
    }
}