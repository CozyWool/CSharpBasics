using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes;

public class ReadonlyBytes : IEnumerable<byte>
{
    private readonly byte[] _bytes;
    private readonly int _hashCode;
    private string _toStringCache;
    public int Length { get; }

    public ReadonlyBytes(params byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);

        _bytes = new byte[bytes.Length];
        Length = bytes.Length;
        Array.Copy(bytes, _bytes, Length);

        _hashCode = ComputeHashCode();
    }

    public byte this[int index]
    {
        get
        {
            if (index < 0 || index >= Length)
            {
                throw new IndexOutOfRangeException();
            }

            return _bytes[index];
        }
    }

    public IEnumerator<byte> GetEnumerator()
    {
        for (var i = 0; i < Length; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        _toStringCache ??= $"[{string.Join(", ", _bytes)}]";

        return _toStringCache;
    }

    public override bool Equals(object obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((ReadonlyBytes) obj);
    }

    public bool Equals(ReadonlyBytes other)
    {
        if (other is null || other.GetType() != GetType())
        {
            return false;
        }

        if (other.Length != Length)
        {
            return false;
        }

        for (var i = 0; i < Length; i++)
        {
            if (other[i] != this[i])
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() => _hashCode;

    private int ComputeHashCode()
    {
        unchecked
        {
            // FNV
            var hash = 2166136261;
            foreach (var b in _bytes)
            {
                hash = (hash ^ b) * 16777619;
            }

            return (int) hash;
        }
    }
}