using System.Text;

namespace Nostr.Core.Extensions;

public static class StringExtensions
{
    public static byte[] ComputeSha256Hash(this string rawData)
    {
        // Create a SHA256
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        // ComputeHash - returns byte array
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
    }
}