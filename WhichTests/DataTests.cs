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
        Assert.Throws<ArgumentNullException>(() => new NonNullableString(null!));
    }

    [Test]
    public void TestNonNullableString_CreateOrEmpty()
    {
        NonNullableString v = NonNullableString.CreateOrEmpty(null);
        Assert.That(v.ToString(), Is.EqualTo(""));
    }

    [Test]
    public void TestNonNullableString_CreateOrDefault()
    {
        NonNullableString v = NonNullableString.CreateOrDefault(null, "default");
        Assert.That(v.ToString(), Is.EqualTo("default"));
    }

    [Test]
    public void TestNonNullableString_Equality()
    {
        NonNullableString v1 = new NonNullableString("test");
        NonNullableString v2 = new NonNullableString("test");
        NonNullableString v3 = new NonNullableString("different");
        
        Assert.That(v1, Is.EqualTo(v2));
        Assert.That(v1, Is.Not.EqualTo(v3));
        Assert.That(v1 == v2, Is.True);
        Assert.That(v1 != v3, Is.True);
    }

    [Test]
    public void TestNonNullableString_ImplicitConversion()
    {
        NonNullableString v = "test string";
        string s = v;
        
        Assert.That(v.ToString(), Is.EqualTo("test string"));
        Assert.That(s, Is.EqualTo("test string"));
    }
}