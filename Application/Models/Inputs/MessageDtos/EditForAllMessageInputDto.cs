using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Inputs.MessageDtos
{
    public class EditForAllMessageInputDto
    {
        internal int MessageId { get; set; }
        public void SetMessageId(int id)
        { MessageId = id; }
        public HashSet<AccessLevel> AccessLevel { get; set; }
    }
}
