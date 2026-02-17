using IMS.Domain.Entities;

namespace IMS.Application.Common.Interfaces;

public interface IJwtProvider
{
    string Generate(User user, IEnumerable<Role> roles);
}