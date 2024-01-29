using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Inputs.MessageDtos
{
    public class EditForEmployeeMessageInputDto
    {
        public int MessageId { get; set; }
        public void SetMessageId(int id)
        { MessageId = id; }
        public bool EmployeeAllowed  { get; set; }
    }
}
