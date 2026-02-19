namespace IMS.Application.Common.Interfaces;

public interface IOtpService
{
    string Generate(int length = 6);

    string Hash(string otp);

    bool Verify(string providedOtp, string storedHash, DateTime expiresAtUtc);
}