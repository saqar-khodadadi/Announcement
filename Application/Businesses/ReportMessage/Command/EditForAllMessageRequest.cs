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
    }
    internal class EditForAllMessageHandler : IRequestHandler<EditForAllMessageRequest>
    {
        public async Task<Unit> Handle(EditForAllMessageRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
