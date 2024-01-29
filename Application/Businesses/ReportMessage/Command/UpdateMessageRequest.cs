using Application.Models.Inputs.MessageDtos;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Businesses.ReportMessage.Command
{
    public class UpdateMessageRequest : IRequest
    {
        public UpdateMessageRequest(UpdateMessageInputDto updateMessage)
        {
            UpdateMessage = updateMessage;
        }
        public UpdateMessageInputDto UpdateMessage { get; private set; }
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
            var updatedMessage = Message.New(request.UpdateMessage.Title, request.UpdateMessage.Description, request.UpdateMessage.Priority);

            var finalMessage = await _messageRepository.UpdateAsync(updatedMessage, request.UpdateMessage.MessageId);

            if (finalMessage == null) throw new Exception("Storage encountered a problem");

            return Unit.Value;
        }
    }
}
