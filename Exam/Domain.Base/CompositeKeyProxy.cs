namespace Domain.Base;

public class CompositeKeyProxy<TFirst, TSecond> : IEquatable<CompositeKeyProxy<TFirst, TSecond>>
    where TFirst : IEquatable<TFirst>
    where TSecond : IEquatable<TSecond>
{
    public TFirst First { get; init; } = default!;
    public TSecond Second { get; init; } = default!;

    public bool Equals(CompositeKeyProxy<TFirst, TSecond>? other)
    {
        if (other == null)
            return false;
        return First.Equals(other.First) && Second.Equals(other.Second);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        return Equals((CompositeKeyProxy<TFirst, TSecond>)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(First, Second);
    }
}
