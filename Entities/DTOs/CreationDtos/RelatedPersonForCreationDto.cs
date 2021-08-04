using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs.CreationDtos
{
    public class RelatedPersonForCreationDto
    {
        public int RelatedToId { get; set; }
        public string RelationType { get; set; }
    }
}
