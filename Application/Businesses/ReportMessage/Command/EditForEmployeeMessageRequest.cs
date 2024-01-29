using Application.Models.Inputs.MessageDtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Application.Businesses.ReportMessage.Command
{
    public class EditForEmployeeMessageRequest : IRequest
    {
        private EditForEmployeeMessageRequest()
        {

        }
        public EditForEmployeeMessageRequest(EditForEmployeeMessageInputDto editForEmployeeMessage)
        {
            EditForEmployeeMessage = editForEmployeeMessage;
        }
        public EditForEmployeeMessageInputDto EditForEmployeeMessage { get; private set; }
    }
    internal class EditForEmployeeMessageHandler : IRequestHandler<EditForEmployeeMessageRequest>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IRoleRepository _roleRepository;
        public EditForEmployeeMessageHandler(IMessageRepository messageRepository, IRoleRepository roleRepository)
        {
            _messageRepository = messageRepository;
            _roleRepository = roleRepository;
        }
        public async Task<Unit> Handle(EditForEmployeeMessageRequest request, CancellationToken cancellationToken)
        {
            var message = await _messageRepository
                .GetByIdWithRolesAsync(request.EditForEmployeeMessage.MessageId);

            if (message == null) throw new Exception("پیام وجود ندارد");
            if (!message.Roles.Any(x=>x.Id == 2)) throw new Exception("ما به این پیام دسترسی ندارید");

            var roleIds = request.EditForEmployeeMessage.EmployeeAllowed ?
                new HashSet<AccessLevel>() { AccessLevel.MidLevelManager, AccessLevel.Employee } :
                new HashSet<AccessLevel>() { AccessLevel.MidLevelManager };

            var roles = roleIds
                 .Select(async x => await _roleRepository.GetByIdAsync((int)x))
                 .Select(t => t.Result)
                 .ToHashSet();

            message.SetRolePermission(roles);

            return Unit.Value;
        }
    }
}
