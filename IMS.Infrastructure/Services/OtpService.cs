 
using System.Security.Cryptography;
using System.Text;
using IMS.Application.Common.Interfaces;


public sealed class OtpService : IOtpService
{
    public string Generate(int length = 6)
    {
        Span<byte> buffer = stackalloc byte[length];
        RandomNumberGenerator.Fill(buffer);

        var sb = new StringBuilder(length);

        foreach (var b in buffer)
            sb.Append(b % 10);

        return sb.ToString();
    }

    public string Hash(string otp)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(otp));
        return Convert.ToHexString(bytes);
    }

    public bool Verify(string providedOtp, string storedHash, DateTime expiresAtUtc)
    {
        if (DateTime.UtcNow > expiresAtUtc)
            return false;

        var providedHash = Hash(providedOtp);

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(providedHash),
            Encoding.UTF8.GetBytes(storedHash)
        );
    }
}