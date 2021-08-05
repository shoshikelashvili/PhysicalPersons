using Entities.DTOs;
using Entities.DTOs.CreationDtos;
using Entities.DTOs.DeletionDtos;
using Entities.DTOs.UpdateDtos;
using Entities.Responses;
using Entities.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IPersonsService
    {
        public PersonResponse GetPerson(int id);
        public PersonResponse CreatePerson(PersonForCreationDto person);

        public BaseResponse DeletePerson(int id);
        public BaseResponse UpdatePerson(int personId, PersonForUpdateDto person);
        public BaseResponse SetImage(int personId, ImageForUpdateDto image);

        public BaseResponse CreateRelationship(int personId, RelatedPersonForCreationDto relation);

        public BaseResponse DeleteRelationship(int personId, RelatedPersonForDeletionDto relation);
        public PersonCollectionResponse QuickSearch(string term);
        public PersonCollectionResponse Search(PersonParameters personParameters);

        public RelationshipStatsResponse GetRelationshipStats(int personId);
    }
}
