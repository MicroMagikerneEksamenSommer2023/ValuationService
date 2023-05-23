using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection;
using ThirdParty.Json.LitJson;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;
using MongoDB.Bson.Serialization.Attributes;

namespace ValuationService.Models
{
    public class ValuationData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Newtonsoft.Json.JsonProperty("id")]
        public string? Id { get; set; }

        [Newtonsoft.Json.JsonProperty("valuationPrice")]
        public double ValuationPrice { get; set; }

        [Newtonsoft.Json.JsonProperty("valuationReason")]
        public string ValuationReason { get; set; }

        [Newtonsoft.Json.JsonProperty("status")]
        public string Status { get; set; }

        [JsonConstructor]
       public ValuationData(string id, double valuationPrice, string valuationReason, string status)
       {
        this.Id = id;
        this.ValuationPrice = valuationPrice;
        this.ValuationReason = valuationReason;
        this.Status = status;
       }
      
    }
}