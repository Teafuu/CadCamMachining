using CadCamMachining.Client.Services.Contracts;
using CadCamMachining.Shared.Models;
using CadCamMachining.Shared.Parameters;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CadCamMachining.Client.States;

public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
{
    private UserInfoDto _userInfoDtoCache;
    private readonly IAuthorizeApi _authorizeApi;

    public IdentityAuthenticationStateProvider(IAuthorizeApi authorizeApi)
    {
        this._authorizeApi = authorizeApi;
    }

    public async Task Login(LoginParameters loginParameters)
    {
        await _authorizeApi.Login(loginParameters);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Register(RegisterParameters registerParameters)
    {
        await _authorizeApi.Register(registerParameters);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Logout()
    {
        await _authorizeApi.Logout();
        _userInfoDtoCache = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async Task<UserInfoDto> GetUserInfo()
    {
        if (_userInfoDtoCache != null && _userInfoDtoCache.IsAuthenticated) return _userInfoDtoCache;
        _userInfoDtoCache = await _authorizeApi.GetUserInfo();
        return _userInfoDtoCache;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();
        try
        {
            var userInfo = await GetUserInfo();
            if (userInfo.IsAuthenticated)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, userInfo.UserName) }.Concat(userInfo.ExposedClaims.Select(c => new Claim(c.Key, c.Value)));
                identity = new ClaimsIdentity(claims, "Server authentication");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Request failed:" + ex.ToString());
        }

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}