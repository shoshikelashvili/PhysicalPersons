using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs.DeletionDtos
{
    public class RelatedPersonForDeletionDto
    {
        public int RelatedFromId { get; set; }
        public int RelatedToId { get; set; }
    }
}
