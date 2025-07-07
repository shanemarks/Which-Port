using System.Diagnostics.CodeAnalysis;

public readonly struct NonNullableString : IEquatable<NonNullableString>
{
    private readonly string _value;

    public NonNullableString(string? value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value), "NonNullableString cannot be created from null");
        
        _value = value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator NonNullableString(string? value) => new(value);

    public static implicit operator string(NonNullableString nonNullableString) => nonNullableString._value;

    public override string ToString() => _value;

    public bool Equals(NonNullableString other) => _value == other._value;

    public override bool Equals(object? obj) => obj is NonNullableString other && Equals(other);

    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(NonNullableString left, NonNullableString right) => left.Equals(right);

    public static bool operator !=(NonNullableString left, NonNullableString right) => !left.Equals(right);

    public bool IsEmpty => string.IsNullOrEmpty(_value);

    public int Length => _value.Length;

    public bool StartsWith(string value) => _value.StartsWith(value);

    public bool Contains(string value) => _value.Contains(value);

    public string Substring(int startIndex) => _value.Substring(startIndex);

    public NonNullableString[] Split(char separator) => _value.Split(separator).Select(s => new NonNullableString(s)).ToArray();

    // Factory method for cases where you want to handle null gracefully
    public static NonNullableString CreateOrEmpty(string? value) => new(value ?? string.Empty);

    // Factory method for cases where you want a default value
    public static NonNullableString CreateOrDefault(string? value, string defaultValue) => new(value ?? defaultValue);
}