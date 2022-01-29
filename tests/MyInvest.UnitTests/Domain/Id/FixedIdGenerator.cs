using System;
using MyInvest.Domain.Id;

namespace MyInvest.UnitTests.Domain.Id;

public class FixedIdGenerator<T> : IUniqueIdGenerator<T> where T : UniqueId
{
    private readonly T _fixedId;

    public FixedIdGenerator(T fixedId)
    {
        _fixedId = fixedId;
    }

    public T Generate() => _fixedId;
}