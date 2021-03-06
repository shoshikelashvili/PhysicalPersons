using AutoMapper;
using Entities.DTOs;
using Entities.DTOs.CreationDtos;
using Entities.DTOs.UpdateDtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
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

            CreateMap<PhoneNumberForCreationDto, PhoneNumber>();
            CreateMap<RelatedPersonForCreationDto, PersonRelation>();

            CreateMap<PersonRelation, RelatedPersonDto>();

            CreateMap<PersonForUpdateDto, Person>();
            CreateMap<PhoneNumberForUpdateDto, PhoneNumber>();
            CreateMap<CityForUpdateDto, City>();
        }
    }
}
