using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Responses
{
    public class RelationshipStatsResponse : BaseResponse
    {
        public IDictionary<string,int> RelationShipStats { get; set; }

        private RelationshipStatsResponse(bool success, string message, IDictionary<string, int> relationShipStats) : base(success, message)
        {
            RelationShipStats = relationShipStats;
        }

        public RelationshipStatsResponse(IDictionary<string, int> relationShipStats) : this(true, string.Empty, relationShipStats)
        {

        }

        public RelationshipStatsResponse(string message) : this(false, message, null)
        {

        }
    }
}
