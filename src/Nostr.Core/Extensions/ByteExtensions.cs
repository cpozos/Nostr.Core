namespace Nostr.Core.Extensions;

public static class ByteExtensions
{
    public static string ToHex(this Span<byte> bytes)
    {
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}