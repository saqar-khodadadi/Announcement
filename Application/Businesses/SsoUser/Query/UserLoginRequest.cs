using Application.Models.Inputs.UserDtos;
using Application.Models.Outputs.UserDtos;
using Domain.Common.Helpers;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Businesses.SsoUser.Query
{
    public class UserLoginRequest : IRequest<UserLoginResultDto>
    {
        public UserLoginRequest(UserLoginInputDto userLoginInput)
        {
            UserLogin = userLoginInput;
        }
        public UserLoginInputDto UserLogin { get; private set; }
    }
    public class UserLoginHandler : IRequestHandler<UserLoginRequest, UserLoginResultDto>
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
        public async Task<UserLoginResultDto> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            var user =await Authenticate(request.UserLogin);

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

            return UserLoginResultDto.MakeNew(user.Id, user.Username, user.FirstName, user.LastName, tokenString);
        }
        #region PrivateMethods
        private async Task<User> Authenticate(UserLoginInputDto request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return null;

            var user = await _user.GetUserWithRoleByUsername(request.Username);

            if (user == null)
                return null;

            return user;
        }

        #endregion
    }
}
