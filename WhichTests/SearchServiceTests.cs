using System.Collections.Immutable;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using WhichLib.Data;
using WhichLib.Interfaces;
using WhichLib.Services;

namespace WhichTests;

public class SearchServiceTests
{


    public class MockLogger : ILogger
    {
        public int CallCount = 0;
        public void Log(string message)
        {
            Console.WriteLine(message);
            CallCount++;
        }
    }
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
    
    [Test]
    public void ShouldListOnePath()
    {

        string path = "/usr/bin:/usr/anotherfolder";
        string[] files = new string[] { "ruby" };
        MockLogger m = new MockLogger();
        var exitCode = SearchService.Search(m, path,new Settings(false,false,new NonNullableString[]{new NonNullableString("ruby")}.ToImmutableArray()), _mockFileSystem);
        Assert.That(m.CallCount,Is.EqualTo(1));
        Assert.That(exitCode, Is.EqualTo(0)); // Success exit code
        
    }
    [Test]
    public void ShouldListMultiplePath()
    {

        string path = "/usr/bin:/usr/anotherfolder";
        string[] files = new string[] { "ruby" };
        MockLogger m = new MockLogger();
        var exitCode = SearchService.Search(m, path,new Settings(false,true,new NonNullableString[]{new NonNullableString("ruby")}.ToImmutableArray()), _mockFileSystem);
        Assert.That(m.CallCount,Is.EqualTo(2));
        Assert.That(exitCode, Is.EqualTo(0)); // Success exit code
        
    }
    
    [Test]
    public void ShouldRespectSilentMode()
    {
        string path = "/usr/bin:/usr/anotherfolder";
        MockLogger m = new MockLogger();
        var exitCode = SearchService.Search(m, path, new Settings(true, false, new NonNullableString[]{new NonNullableString("ruby")}.ToImmutableArray()), _mockFileSystem);
        
        // Silent mode should not log any output
        Assert.That(m.CallCount, Is.EqualTo(0));
        Assert.That(exitCode, Is.EqualTo(0)); // Still success, just silent
    }
    
    [Test]
    public void ShouldReturnFailureExitCodeWhenFileNotFound()
    {
        string path = "/usr/bin:/usr/anotherfolder";
        MockLogger m = new MockLogger();
        var exitCode = SearchService.Search(m, path, new Settings(false, false, new NonNullableString[]{new NonNullableString("nonexistent")}.ToImmutableArray()), _mockFileSystem);
        
        Assert.That(exitCode, Is.EqualTo(1)); // Failure exit code
    }
    
    [Test]
    public void ShouldReturnFailureExitCodeWhenFileNotFoundInSilentMode()
    {
        string path = "/usr/bin:/usr/anotherfolder";
        MockLogger m = new MockLogger();
        var exitCode = SearchService.Search(m, path, new Settings(true, false, new NonNullableString[]{new NonNullableString("nonexistent")}.ToImmutableArray()), _mockFileSystem);
        
        // Silent mode should not log errors
        Assert.That(m.CallCount, Is.EqualTo(0));
        Assert.That(exitCode, Is.EqualTo(1)); // Failure exit code
    }
}