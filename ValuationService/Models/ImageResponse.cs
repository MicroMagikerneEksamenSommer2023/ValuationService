using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace ValuationService.Models
{

public class ImageResponse
{
    public List<byte[]> FileBytes { get; set; }
    public Valuation AdditionalData { get; set; }

    public ImageResponse(List<byte[]> img, Valuation data)
    {
        this.FileBytes = img;
        this.AdditionalData = data;

    }
  
}
}