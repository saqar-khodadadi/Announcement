using Application.Models.Inputs.MessageDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Task<Unit> Handle(EditForEmployeeMessageRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
