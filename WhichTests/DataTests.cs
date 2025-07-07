namespace WhichTests;

public class DataTests
{
    [TestCase("","")]
    [TestCase("foobar","foobar")]

    public void TestNonNullableString(string value, string expected)
    {
        NonNullableString v = new NonNullableString(value);
        Assert.That(v.ToString(), Is.EqualTo(expected));
    }
    
    [Test]
    public void TestNonNullableString_WithNull()
    {
        NonNullableString v = new NonNullableString(null!);
        Assert.That(v.ToString(), Is.EqualTo(""));
    }
}