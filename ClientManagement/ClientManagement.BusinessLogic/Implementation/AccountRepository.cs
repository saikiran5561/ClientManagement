using ClientManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClientManagement.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<string> SignUpAsync(SignUpModel signUpModel)
        {
            var signUp = new IdentityUser { UserName = signUpModel.Email, Email = signUpModel.Email };
            var signUpUser = await _userManager.CreateAsync(signUp, signUpModel.Password);

            if (!signUpUser.Succeeded)
                return string.Join(", ", signUpUser.Errors);

            return "User signup successful";
        }

        public async Task<string> LoginAsync(SignInModel signInModel)
        {
            var login = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

            if (!login.Succeeded)
                return "Login attempt is not valid";

            var loginUser = await _userManager.FindByEmailAsync(signInModel.Email);
            return JwtToken(loginUser);
        }

        private string JwtToken(IdentityUser user)
        {
            var tokenContext = _configuration.GetSection("JWT");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenContext["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: tokenContext["Issuer"],
                audience: tokenContext["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
