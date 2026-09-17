using System;
using Shouldly;
using Xunit;

namespace Volo.Abp;

public class KeyedObjectHelper_Tests
{
    [Fact]
    public void EncodeCompositeKey()
    {
        var encoded = KeyedObjectHelper.EncodeCompositeKey("Book", "123");
        encoded.ShouldBe("Qm9va3x8MTIz");
    }

    [Fact]
    public void DecodeCompositeKey()
    {
        var decoded = KeyedObjectHelper.DecodeCompositeKey("Qm9va3x8MTIz");
        decoded.ShouldBe("Book||123");
    }

    [Fact]
    public void Encode_Decode_CompositeKey()
    {
        var encoded = KeyedObjectHelper.EncodeCompositeKey("User", 42, Guid.Empty);
        var decoded = KeyedObjectHelper.DecodeCompositeKey(encoded);
        
        decoded.ShouldBe($"User||42||{Guid.Empty}");
    }
}