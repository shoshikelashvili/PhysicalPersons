using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Parameters
{
    public class PersonParameters
    {
        const int maxPageSize = 10;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        #nullable enable
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? Gender { get; set; }

        public string? PersonalNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public int? CityId { get; set; }
        #nullable disable
    }
}
