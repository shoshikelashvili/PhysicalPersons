using AutoMapper;
using Entities.DTOs;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalPersons
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>();
            CreateMap<City, CityDto>();
            CreateMap<PhoneNumber, PhoneNumberDto>();
            CreateMap<Person, RelatedPersonDto>();
            CreateMap<PersonForCreationDto, Person>();
            CreateMap<CityForCreationDto, City>();
        }
    }
}
