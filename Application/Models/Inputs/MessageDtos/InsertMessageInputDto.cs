using Domain.Enums;

namespace Application.Models.Inputs.MessageDtos
{
    public class InsertMessageInputDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
    }
}
