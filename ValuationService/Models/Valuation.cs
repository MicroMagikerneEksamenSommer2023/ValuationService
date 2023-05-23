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
    public class Valuation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Newtonsoft.Json.JsonProperty("title")]
        public string Title { get; set; }

        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }

        [Newtonsoft.Json.JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }

        [Newtonsoft.Json.JsonProperty("valuationPrice")]
        public double ValuationPrice { get; set; }

        [Newtonsoft.Json.JsonProperty("valuationReason")]
        public string ValuationReason { get; set; }

        [Newtonsoft.Json.JsonProperty("status")]
        public string Status { get; set; }

        [JsonConstructor]
       public Valuation(string title, string description, string customerEmail)
       {
        this.Title = title;
        this.Description = description;
        this.CustomerEmail = customerEmail;
        this.ValuationPrice = 0.0;
        this.ValuationReason = string.Empty;
        this.Status = "Pending";
       }
       public Valuation(string id,string title, string description, string customerEmail, double valuationPrice, string valuationReason, string status)
       {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.CustomerEmail = customerEmail;
        this.ValuationPrice = valuationPrice;
        this.ValuationReason = valuationReason;
        this.Status = status;
       }
       public ValuationDB Convert(List<string> paths){
        return new ValuationDB(this.Title,this.Description,this.CustomerEmail,this.ValuationPrice,this.ValuationReason,this.Status, paths);
       }

    }
}