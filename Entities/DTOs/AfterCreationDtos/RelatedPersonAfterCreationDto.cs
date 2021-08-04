using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs.AfterCreationDtos
{
    public class RelatedPersonAfterCreationDto
    {
        public int RelatedToId { get; set; }
        public string RelationType { get; set; }
    }
}
