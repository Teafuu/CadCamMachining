using CadCamMachining.Shared.Models;
using CadCamMachining.Shared.Parameters;

namespace CadCamMachining.Client.Services.Contracts;

public interface IAuthorizeApi
{
    Task Login(LoginParameters loginParameters);
    Task Register(RegisterParameters registerParameters);
    Task Logout();
    Task<UserInfoDto> GetUserInfo();
}