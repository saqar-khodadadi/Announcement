using Application.Models.Inputs.MessageDtos;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Businesses.ReportMessage.Command
{
    public class EditForAllMessageRequest : IRequest
    {
        private EditForAllMessageRequest()
        {
            
        }
        public EditForAllMessageRequest(EditForAllMessageInputDto editForAllMessage)
        {
            EditForAllMessage = editForAllMessage;
        }
        public EditForAllMessageInputDto EditForAllMessage { get; private set; }
    }
    internal class EditForAllMessageHandler : IRequestHandler<EditForAllMessageRequest>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IRoleRepository _roleRepository;
        public EditForAllMessageHandler(IMessageRepository messageRepository, IRoleRepository roleRepository)
        {
            _messageRepository = messageRepository;
            _roleRepository = roleRepository;
        }
        public async Task<Unit> Handle(EditForAllMessageRequest request, CancellationToken cancellationToken)
        {
            var message = await _messageRepository.GetByIdAsync(request.EditForAllMessage.MessageId);

            if (message == null) throw new Exception("پیام وجود ندارد");

            var roles = request
                .EditForAllMessage
                .AccessLevel
                .Select(async x => await _roleRepository.GetByIdAsync((int)x))
                .Select(t=>t.Result)
                .ToHashSet();

            message.SetRolePermission(roles);

            return Unit.Value;
        }
    }
}
