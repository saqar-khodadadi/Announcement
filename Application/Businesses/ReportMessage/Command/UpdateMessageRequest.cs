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
    public class UpdateMessageRequest : IRequest
    {
        internal int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public void SetMessageId(int id) 
        { Id = id; }

    }
    internal class UpdateMessageHandler : IRequestHandler<UpdateMessageRequest>
    {
        private readonly IMessageRepository _messageRepository;
        public UpdateMessageHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Unit> Handle(UpdateMessageRequest request, CancellationToken cancellationToken)
        {
            var updatedMessage = Message.New(request.Title, request.Description, request.Priority);

            var finalMessage = await _messageRepository.UpdateAsync(updatedMessage, request.Id);

            if (finalMessage == null) throw new Exception("Storage encountered a problem");

            return Unit.Value;
        }
    }
}
