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
    public class ValuationDB
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CustomerEmail { get; set; }
        public double ValuationPrice { get; set; }
        public string ValuationReason { get; set; }
        public string Status { get; set; }
        public List<string>? ImagePaths { get; set; }

       public ValuationDB(string title, string description, string customerEmail, double valuationPrice, string valuationReason, string status,List<string> imagePaths)
       {
        this.Title = title;
        this.Description = description;
        this.CustomerEmail = customerEmail;
        this.ValuationPrice = valuationPrice;
        this.ValuationReason = valuationReason;
        this.Status = status;
        this.ImagePaths = imagePaths;
       }
       public ValuationDB(string title, string description, string customerEmail,double valuationPrice, string valuationReason, string status)
       {
        this.Title = title;
        this.Description = description;
        this.CustomerEmail = customerEmail;
        this.ValuationPrice = valuationPrice;
        this.ValuationReason = valuationReason;
        this.Status = status;
       }
       public Valuation Convert()
       {
        return new Valuation(this.Id,this.Title,this.Description,this.CustomerEmail,this.ValuationPrice,this.ValuationReason, this.Status);
       }
    }
}