using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class RelatedPersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        
        public string RelationType { get; set; }
    }
}
