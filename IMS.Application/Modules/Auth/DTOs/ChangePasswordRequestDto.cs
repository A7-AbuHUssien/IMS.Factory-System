namespace IMS.Application.Modules.Auth.DTOs;

public class ChangePasswordRequestDto
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}