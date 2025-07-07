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
        Assert.That(CommandLineProcessor.ValidateOption(new NonNullableString(option)), Is.EqualTo(expected));
        
    }
    [TestCase ("-a", false, true,"python","ruby")]
    public void ShouldGenerateSettingsFromOptions(string options,  bool expectedSilent, bool expectedAll, params string[] files)
    {
        var convertedStrings = files.Select((x) => new NonNullableString(x)).ToImmutableArray();
        var settings = CommandLineProcessor.OptionsToSettings(new NonNullableString(options), convertedStrings);
      Assert.That(settings.Value.ListAll, Is.EqualTo(true));
      Assert.That(settings.Value.SilentMode, Is.EqualTo(false));
      for (int n = 0; n < files.Length; n++)
      {
          Assert.That(settings.Value.Files[n].ToString(), Is.EqualTo(files[n]));
      }

    }
}