using Domain.Repositories;
using MediatR;

namespace Application.Businesses.ReportMessage.Command
{
    public class DeleteMessageRequest : IRequest
    {
        internal int Id { get; private set; }
        public void SetMessageId(int id)
        { Id = id; }
    }
    public class DeleteMessageHandler : IRequestHandler<DeleteMessageRequest>
    {
        private readonly IMessageRepository _messageRepository;
        public DeleteMessageHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<Unit> Handle(DeleteMessageRequest request, CancellationToken cancellationToken)
        {

            var saved = await _messageRepository.DeleteByIdAsync(request.Id);

            if (saved <=0 ) throw new Exception("Storage encountered a problem");

            return Unit.Value;
        }
    }
}
