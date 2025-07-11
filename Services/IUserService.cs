using Assiginment.DTO;

namespace Assiginment.Services
{
    public interface IUserService
    {
        Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto loginRequest);
    }

}
