using Application.Models.Outputs;
using Domain.Common.Helpers;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Application.Businesses.SsoUser.Query
{
    public class UserLoginRequest : IRequest<UserLoginViewModel>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class UserLoginHandler : IRequestHandler<UserLoginRequest, UserLoginViewModel>
    {
        private readonly IUserRepository _user;
        private readonly IRoleRepository _role;
        private readonly Alaki _options;
        public UserLoginHandler(IUserRepository user, IRoleRepository role, IOptions<Alaki> options)
        {
            _user = user;
            _role = role;
            _options = options.Value;
        }
        public async Task<UserLoginViewModel> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            var user =await Authenticate(request);

            if (user == null)
            {
                return null;
            }

            var roleId = user.Roles.ToList().FirstOrDefault()?.Id ?? 0;
            var role = await _role.GetByIdAsync(roleId);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Secret);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim(ClaimTypes.Role, role.Name.ToUpper()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
              
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _options.Issuer,
                Audience = _options.Audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return UserLoginViewModel.MakeNew(user.Id, user.Username, user.FirstName, user.LastName, tokenString);
        }
        #region PrivateMethods
        private async Task<User> Authenticate(UserLoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return null;

            var user = await _user.GetUserWithRoleByUsername(request.Username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        #endregion
    }
}
