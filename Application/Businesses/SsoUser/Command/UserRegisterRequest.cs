using Application.Models;
using Domain.Repositories;
using MediatR;
using Domain.Entities;
using Domain.Repositories.Base;
using System.Net.Http.Headers;
using System.Data;
using System;

namespace Application.Businesses.SsoUser.Command
{
    public class UserRegisterRequest: IRequest<UserRegisterViewModel>
    {
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public string Username { get;  set; }
        public string Password { get;  set; }
        public int RoleId { get; set; }

    }
    internal class UserRegisterHandler : IRequestHandler<UserRegisterRequest, UserRegisterViewModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public UserRegisterHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public async Task<UserRegisterViewModel> Handle(UserRegisterRequest request, CancellationToken cancellationToken)
        {
            var finalUser = await Create(request);

            var result = UserRegisterViewModel.MakeNew(finalUser);

            return result;
        }

        #region PrivateMethods

        private async Task<User> Create(UserRegisterRequest request)
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

            return newUser;
        }

        #endregion


    }
}
