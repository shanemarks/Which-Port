using System.Collections.Immutable;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using WhichLib.Services;

namespace WhichTests;

[TestFixture]
public class PathOperationTests
{
    private IFileSystem _mockFileSystem;
    [SetUp]
    public void Setup()
    {
        _mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
        {
            { "/usr/bin/ruby", new MockFileData("") },
            { "~/test/python", new MockFileData("") },
            { "/usr/anotherfolder/ruby", new MockFileData("") },


        });
    }

    [TestCase("")]
    [TestCase(":")]

    public void WhenEmptyArgumentGetPathsHasNoValue(string path)
    {
        var result = PathOperations.GetPaths(new NonNullableString(path));
        Assert.That(result.Error == "No Paths Specified");
    }

    [TestCase("~/test")]
    [TestCase("~/test:" )]

    public void WhenSinglePathReturnSinglePath(string path)
    {
        var result = PathOperations.GetPaths(new NonNullableString(path));
        Assert.That(result.Value.Length, Is.EqualTo(1));
        Assert.That(result.Value[0].ToString(), Is.EqualTo("~/test"));
    }

    [TestCase("~/test:/Foo")]
    [TestCase("~/test:3sdfsd##\0#**%%:/Foo")]

    public void WhenMultiplePathsReturnValidPaths(string path)
    {
        var result = PathOperations.GetPaths(new NonNullableString(path));
        Assert.That(result.Value.Length, Is.EqualTo(2));
        Assert.That(result.Value[0].ToString(), Is.EqualTo("~/test"));
        Assert.That(result.Value[1].ToString(), Is.EqualTo("/Foo"));

    }
    
    [TestCase("~/test:/usr/bin:/invalid")]

    public void WhenCheckDirectoriesOnlyReturnExisting(string path)
    {
        var result = PathOperations.FilterExistingPaths(PathOperations.GetPaths(new NonNullableString(path)), _mockFileSystem);
        Assert.That(result.Value.Length, Is.EqualTo(2));
        Assert.That(result.Value[0].ToString(), Is.EqualTo("~/test"));
        Assert.That(result.Value[1].ToString(), Is.EqualTo("/usr/bin"));

    }
    [TestCase("")]
    [TestCase("sdafsd\0")]
    public void WhenFailureScanningDirectoriesShouldFail(string path)
     {
         var result = PathOperations.FilterExistingPaths(PathOperations.GetPaths(new NonNullableString(path)), _mockFileSystem);
         Assert.That(result.IsSuccessful,Is.EqualTo(false));
     }

    [TestCase("~/test:/usr/bin:/invalid","ruby")]
    
    public void ShouldReturnFileWhenScanningInPath(string path, string filename)
    {
        var result = PathOperations.FindFile(PathOperations.FilterExistingPaths(PathOperations.GetPaths(new NonNullableString(path)), _mockFileSystem)
        ,new NonNullableString(filename), _mockFileSystem);
        Assert.That(result.IsSuccessful,Is.EqualTo(true));
        Assert.That(result.Value[0].ToString(),Is.EqualTo("/usr/bin/ruby"));
    }
    
    [TestCase("~/test:/usr/bin:/invalid","sdfsd")]
    
    public void ShouldFailIfCantFindFile(string path, string filename)
    {
        var result = PathOperations.FindFile(PathOperations.FilterExistingPaths(PathOperations.GetPaths(new NonNullableString(path)), _mockFileSystem)
            ,new NonNullableString(filename), _mockFileSystem);
        Assert.That(result.IsSuccessful,Is.EqualTo(false));
        Assert.That(result.Error,Is.EqualTo("could not find file"));

    }
    [TestCase("~sdfsdfsd\0","sdfsd")]
    
    public void ShouldPassThroughFailureToFindFile(string path, string filename)
    {
        var result = PathOperations.FindFile(PathOperations.FilterExistingPaths(PathOperations.GetPaths(new NonNullableString(path)), _mockFileSystem)
            ,new NonNullableString(filename), _mockFileSystem);
        Assert.That(result.IsSuccessful,Is.EqualTo(false));
        Assert.That(result.Error,Is.EqualTo("No Paths Specified"));
    }
    
    [TestCase("~/test:/usr/bin:/invalid:/usr/anotherfolder/","ruby","python")]
    public void ShouldReturnMultipleResultsWithMultipleFiles(string path, params string[] files)
    {
        var result = PathOperations.FindFiles(PathOperations.FilterExistingPaths(PathOperations.GetPaths(new NonNullableString(path)), _mockFileSystem)
            ,files.Select(x=>new NonNullableString(x)).ToImmutableArray(), _mockFileSystem);
        Assert.That(result.IsSuccessful,Is.EqualTo(true));
        Assert.That(result.Value[0].Length,Is.EqualTo(2));
        Assert.That(result.Value[0][0].ToString(),Is.EqualTo("/usr/bin/ruby"));
        Assert.That(result.Value[0][1].ToString(),Is.EqualTo("/usr/anotherfolder/ruby"));

        Assert.That(result.Value[1][0].ToString(),Is.EqualTo("~/test/python"));

    }
}