using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Inputs.MessageDtos
{
    public class UpdateMessageInputDto
    {
        internal int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public void SetMessageId(int id)
        { Id = id; }
    }
}
