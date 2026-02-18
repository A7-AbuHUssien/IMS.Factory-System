using IMS.Domain.Entities;

namespace IMS.Application.Common.Interfaces;

public interface IJwtProvider
{
    string GenerateAccesToken(User user, IEnumerable<Role> roles);
    string GenerateRefreshToken();
}