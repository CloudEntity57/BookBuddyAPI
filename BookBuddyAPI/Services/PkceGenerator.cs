using System;
using System.Security.Cryptography;
using System.Text;

namespace BookbuddyAPI.Services
{
public class PkceGenerator
{
    public static (string CodeVerifier, string CodeChallenge) Generate()
    {
        // 1. Generate cryptographically secure random bytes
        byte[] randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        // 2. Convert to Base64Url string to get the Code Verifier
        string codeVerifier = Base64UrlEncode(randomBytes);

        // 3. Hash the verifier using SHA-256
        byte[] verifierBytes = Encoding.UTF8.GetBytes(codeVerifier);
        byte[] hashBytes = SHA256.HashData(verifierBytes);

        // 4. Convert the hash to Base64Url string to get the Code Challenge
        string codeChallenge = Base64UrlEncode(hashBytes);

        return (codeVerifier, codeChallenge);
    }

    private static string Base64UrlEncode(byte[] input)
    {
        // Convert to standard Base64 string
        string base64 = Convert.ToBase64String(input);

        // Convert standard Base64 to Base64Url (RFC 7636 compliant)
        return base64
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }
}
}
