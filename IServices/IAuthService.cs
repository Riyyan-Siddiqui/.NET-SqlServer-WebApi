using WebAPI_tutorial.Dto;

namespace WebAPI_tutorial.IServices
{
    public interface IAuthService
    {
        Task<Tuple<int, TokenDto>> LoginUser(UserDto dto);
        Task<Tuple<int, string>> RegisterUser(UserDto dto);
    }
}
