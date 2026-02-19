namespace IMS.Application.Modules.Auth.DTOs;

public class ResetPasswordRequestDto
{
    public string ResetToken { get; set; }
    public string NewPassword { get; set; }
}