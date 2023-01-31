using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YouYou.Api.Extensions;
using YouYou.Api.ViewModels.Users;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.Users;
using YouYou.Business.Models;

namespace YouYou.Api.Controllers
{
    [Route("{culture:culture}/api/Account")]
    public class AuthController : MainController<AuthController>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;
        private readonly IDistributedCache _cache;

        public AuthController(IErrorNotifier errorNotifier,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IOptions<AppSettings> appSettings,
            IUser user, IStringLocalizer<AuthController> localizer,
            IUserService userService, IDistributedCache cache) : base(errorNotifier, user, localizer)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _userService = userService;
            _cache = cache;
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return CustomResponse();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.UserName, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(loginUser.UserName);
                if (user.Disabled || user.IsDeleted)
                {
                    NotifyError("Usuário ou Senha incorretos");
                    return CustomResponse(loginUser);
                }

                return CustomResponse(await GenerateJwt(user));
            }
            if (result.IsLockedOut)
            {
                NotifyError("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse(loginUser);
            }

            NotifyError("Usuário ou Senha incorretos");
            return CustomResponse(loginUser);
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult> RefreshToken(RefreshTokenDataViewModel requestRefreshToken)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            string strTokenArmazenado = _cache.GetString(requestRefreshToken.RefreshToken);

            if (!RefreshTokenIsValid(requestRefreshToken, strTokenArmazenado)) return CustomResponse(requestRefreshToken);

            _cache.Remove(requestRefreshToken.RefreshToken);

            var user = await _userManager.FindByIdAsync(requestRefreshToken.UserId.ToString());
            if (user.Disabled)
            {
                NotifyError("Usuário desabilitado.");
                return CustomResponse(requestRefreshToken);
            }

            return CustomResponse(await GenerateJwt(user));
        }

        private bool RefreshTokenIsValid(RefreshTokenDataViewModel requestRefreshToken, string strTokenArmazenado)
        {
            if (string.IsNullOrWhiteSpace(strTokenArmazenado))
            {
                NotifyError("Falha ao Autenticar");
                return false;
            }

            var refreshTokenBase = JsonConvert
                .DeserializeObject<RefreshTokenDataViewModel>(strTokenArmazenado);

            if (refreshTokenBase.UserId != requestRefreshToken.UserId ||
               refreshTokenBase.RefreshToken != requestRefreshToken.RefreshToken)
            {
                NotifyError("Falha ao Autenticar");
                return false;
            }

            return true;
        }

        private async Task<LoginResponseViewModel> GenerateJwt(ApplicationUser user)
        {
            var claims = await GetClaims(user);

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            UserTokenViewModel userData = await GetUserData(user, claims);

            string refreshToken = GenerateRefreshToken(user.Id);

            return new LoginResponseViewModel
            {
                AccessToken = CreateToken(identityClaims),
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiryHours).TotalSeconds,
                RefreshToken = refreshToken,
                UserToken = userData
            };
        }

        private string GenerateRefreshToken(Guid userId)
        {
            string refreshToken = Guid.NewGuid().ToString().Replace("-", String.Empty);

            var refreshTokenData = new RefreshTokenDataViewModel();
            refreshTokenData.RefreshToken = refreshToken;
            refreshTokenData.UserId = userId;

            DistributedCacheEntryOptions optionsCache = new DistributedCacheEntryOptions();
            optionsCache.SetAbsoluteExpiration(TimeSpan.FromHours(_appSettings.FinalExpiration));
            _cache.SetString(refreshToken, JsonConvert.SerializeObject(refreshTokenData), optionsCache);

            return refreshToken;
        }

        private async Task<IList<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email != null ? user.Email : string.Empty));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            return claims;
        }

        private async Task<UserTokenViewModel> GetUserData(ApplicationUser user, IList<Claim> claims)
        {
            var notTypeResults = new List<string>(){
                JwtRegisteredClaimNames.Sub,
                JwtRegisteredClaimNames.Email,
                JwtRegisteredClaimNames.Jti,
                JwtRegisteredClaimNames.Nbf,
                JwtRegisteredClaimNames.Iat
            };

            var userWithPerson = await _userService.GetByIdWithPerson(user.Id);

            UserTokenViewModel userData = new UserTokenViewModel()
            {
                Id = user.Id,
                Name = userWithPerson.IsCompany ? userWithPerson.JuridicalPerson.CompanyName : userWithPerson.PhysicalPerson.Name,
                Email = user.Email,
                Claims = claims.Where(c => !notTypeResults.Contains(c.Type))
                            .Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value }),
                ReadTermsOfUse = user.TermsOfUse == null ? false : true
            };

            return userData;
        }

        private string CreateToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emitter,
                Audience = _appSettings.ValidIn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiryHours),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        //[Authorize(Roles = "Entregador Direto, Transbordo, Entregador do Transbordo")]
        //[HttpPut("ConfirmTermsOfUse")]
        //public async Task<ActionResult> ConfirmTermsOfUse([FromBody]string androidInfo)
        //{
        //    string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        //    TermsOfUseDto termsOfUse = new TermsOfUseDto(DateTime.Now, ip, androidInfo);
        //    await _userService.ConfirmTermsOfUse(termsOfUse);

        //    return CustomResponse();
        //}
    }
}
