using System.Collections.Immutable;
using WhichLib.Data;
using WhichLib.Services;

namespace WhichTests;

[TestFixture]
public class CommandLineParserTests
{
    [Test]
    public void ShouldFailWhenNoArguments()
    {
        var emptyArgs = RawArguments.Empty;
        var result = CommandLineParser.Parse(emptyArgs);
        
        Assert.That(result.IsSuccessful, Is.False);
        Assert.That(result.Error, Is.EqualTo("No arguments provided"));
    }

    [Test]
    public void ShouldParseOptionsAndFiles()
    {
        var args = new RawArguments(new[] { 
            new NonNullableString("-a"), 
            new NonNullableString("ruby"), 
            new NonNullableString("python") 
        }.ToImmutableArray());
        
        var result = CommandLineParser.Parse(args);
        
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value.Options.ToString(), Is.EqualTo("-a"));
        Assert.That(result.Value.FilesToSearch.Count, Is.EqualTo(2));
        Assert.That(result.Value.FilesToSearch.Names[0].ToString(), Is.EqualTo("ruby"));
        Assert.That(result.Value.FilesToSearch.Names[1].ToString(), Is.EqualTo("python"));
    }

    [Test]
    public void ShouldParseFilesOnlyWhenNoOptions()
    {
        var args = new RawArguments(new[] { 
            new NonNullableString("ruby"), 
            new NonNullableString("python") 
        }.ToImmutableArray());
        
        var result = CommandLineParser.Parse(args);
        
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value.Options.IsEmpty, Is.True);
        Assert.That(result.Value.FilesToSearch.Count, Is.EqualTo(2));
        Assert.That(result.Value.FilesToSearch.Names[0].ToString(), Is.EqualTo("ruby"));
        Assert.That(result.Value.FilesToSearch.Names[1].ToString(), Is.EqualTo("python"));
    }

    [Test]
    public void ShouldParseSingleFileWithoutOptions()
    {
        var args = new RawArguments(new[] { 
            new NonNullableString("ruby")
        }.ToImmutableArray());
        
        var result = CommandLineParser.Parse(args);
        
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value.Options.IsEmpty, Is.True);
        Assert.That(result.Value.FilesToSearch.Count, Is.EqualTo(1));
        Assert.That(result.Value.FilesToSearch.Names[0].ToString(), Is.EqualTo("ruby"));
    }

    [Test]
    public void ShouldParseOptionsWithoutFiles()
    {
        var args = new RawArguments(new[] { 
            new NonNullableString("-s")
        }.ToImmutableArray());
        
        var result = CommandLineParser.Parse(args);
        
        Assert.That(result.IsSuccessful, Is.True);
        Assert.That(result.Value.Options.ToString(), Is.EqualTo("-s"));
        Assert.That(result.Value.FilesToSearch.IsEmpty, Is.True);
    }
}