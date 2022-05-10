using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using MisteryBlazor.StringUtils;
using MongoDB.Driver;

namespace MisteryBlazor.Services
{
    public class AuthorizationManager
    {
        private ILogger _Logger;
        private string userId { get; set; }
        private string userName { get; set; }
        public string UserId => userId;
        public string UserName => userName;
        private AuthenticationState authState;
        private ClaimsPrincipal currectUser;
        private readonly AuthenticationStateProvider _AuthenticationStateProvider;
        public AuthorizationManager(ILogger<AuthorizationManager> logger, AuthenticationStateProvider provider)
        {
            _Logger = logger;
            _AuthenticationStateProvider = provider;
            _ = InitAsync();
        }

        public async Task InitAsync()
        {
            authState = await _AuthenticationStateProvider.GetAuthenticationStateAsync();
            currectUser = authState.User;
            userId = currectUser.FindFirstValue(ClaimTypes.NameIdentifier);
            userName = currectUser.FindFirstValue(ClaimTypes.Name).ToStringFromASCIIByte();
        }
    }
}
