using Entities.DTOs;
using Entities.DTOs.CreationDtos;
using Entities.DTOs.DeletionDtos;
using Entities.DTOs.UpdateDtos;
using Entities.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IPersonsService
    {
        public PersonDto GetPerson(int id);
        public PersonDto CreatePerson(PersonForCreationDto person);

        public bool DeletePerson(int id);
        public bool UpdatePerson(int personId, PersonForUpdateDto person);
        public bool SetImage(int personId, ImageForUpdateDto image);

        public bool CreateRelationship(int personId, RelatedPersonForCreationDto relation);

        public bool DeleteRelationship(int personId, RelatedPersonForDeletionDto relation);
        public IEnumerable<PersonDto> QuickSearch(string term);
        public IEnumerable<PersonDto> Search(PersonParameters personParameters);

        public IDictionary<string, int> GetRelationshipStats(int personId);
    }
}
