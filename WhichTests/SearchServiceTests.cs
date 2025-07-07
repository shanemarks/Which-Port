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
        SearchService.Search(m, path,new Settings(false,false,new NonNullableString[]{new NonNullableString("ruby")}.ToImmutableArray()), _mockFileSystem);
        Assert.That(m.CallCount,Is.EqualTo(1));
        
    }
    [Test]
    public void ShouldListMultiplePath()
    {

        string path = "/usr/bin:/usr/anotherfolder";
        string[] files = new string[] { "ruby" };
        MockLogger m = new MockLogger();
        SearchService.Search(m, path,new Settings(false,true,new NonNullableString[]{new NonNullableString("ruby")}.ToImmutableArray()), _mockFileSystem);
        Assert.That(m.CallCount,Is.EqualTo(2));
        
    }
}