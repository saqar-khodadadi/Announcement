using Domain.Repositories;
using MediatR;
using Domain.Entities;
using Application.Models.Inputs.UserDtos;
using Application.Models.Outputs.UserDtos;

namespace Application.Businesses.SsoUser.Command
{
    public class UserRegisterRequest: IRequest<UserRegisterResultDto>
    {

        private UserRegisterRequest()
        {
            
        }
        public UserRegisterRequest(UserRegisterInputDto userRegister)
        {
            UserRegister = userRegister;
        }
        public UserRegisterInputDto UserRegister { get; private set; }
    }
    internal class UserRegisterHandler : IRequestHandler<UserRegisterRequest, UserRegisterResultDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public UserRegisterHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public async Task<UserRegisterResultDto> Handle(UserRegisterRequest request, CancellationToken cancellationToken)
        {
            var finalUser = await Create(request.UserRegister);

            var result = UserRegisterResultDto.MakeNew(finalUser);

            return result;
        }

        #region PrivateMethods

        private async Task<User> Create(UserRegisterInputDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new Exception("Password is required");

            if (await _userRepository.AnyAsync(x=>x.Username == request.Username))
                throw new Exception("Username \"" + request.Username + "\" is already taken");

            var role = await _roleRepository.GetByIdAsync(request.RoleId);

            if (role == null) { throw new ArgumentNullException(nameof(role)); }

            //var encryptedPassword = CreatePasswordHash(request.Password);

            var newUser = User.New(request.FirstName, request.LastName, request.Username, 
                request.Password, role);

            await _userRepository.AddAsync(newUser);

            return newUser;
        }

        #endregion


    }
}
