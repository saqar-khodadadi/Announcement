using Application.Models.Inputs.MessageDtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Businesses.ReportMessage.Command
{
    public class InsertMessageRequest : IRequest
    {
        private InsertMessageRequest()
        {
            
        }

        public InsertMessageRequest(InsertMessageInputDto insertMessage)
        {
            InsertMessage = insertMessage;
        }
        public InsertMessageInputDto InsertMessage { get; private set; }
    }

    internal class InsertMessageHandler : IRequestHandler<InsertMessageRequest>
    {
        private readonly IMessageRepository _messageRepository;
        public InsertMessageHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<Unit> Handle(InsertMessageRequest request, CancellationToken cancellationToken)
        {
            var newMessage = Message.New(request.InsertMessage.Title, request.InsertMessage.Description, request.InsertMessage.Priority);

            var finalMessage = await _messageRepository.AddAsync(newMessage);

            if (finalMessage == null) throw new Exception("Storage encountered a problem");

            return Unit.Value;
        }
    }
}
