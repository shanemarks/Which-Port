public readonly  struct NonNullableString
{
    private readonly string _string;

    public NonNullableString(string value)
    {
        _string = (value ?? string.Empty);
    }

    public override string ToString() => _string;
}