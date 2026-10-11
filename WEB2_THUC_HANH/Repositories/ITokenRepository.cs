using Microsoft.AspNetCore.Identity;

namespace WEB2_THUC_HANH.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(
            IdentityUser user,
            List<string> roles);
    }
}