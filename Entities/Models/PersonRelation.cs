using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class PersonRelation
    {
        public int RelatedFromId { get; set; }
        public int RelatedToId { get; set; }

        public Person RelatedFrom { get; set; }
        public Person RelatedTo { get; set; }
    }
}
