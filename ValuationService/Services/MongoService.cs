using ValuationService.Controllers;
using ValuationService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace ValuationService.Services
{ 
    public class MongoService
{
    private readonly ILogger<MongoService> _logger;

    private readonly IMongoCollection<ValuationDB> _valuations;

    private readonly IConfiguration _config;
    private readonly PictureService picService;

    public MongoService(ILogger<MongoService> logger, IConfiguration config, PictureService picservice)
    {
        _logger = logger;
        _config = config;
        picService = picservice;
     
        var mongoClient = new MongoClient(_config["connectionsstring"]);
        var database = mongoClient.GetDatabase(_config["database"]);
        _valuations = database.GetCollection<ValuationDB>(_config["collection"]);
    }

    public async Task<bool> CreateCatalogItem(List<IFormFile> pictures ,Valuation data)
        {
            
            List<string> paths = await picService.SavePicture(pictures);
            ValuationDB item = data.Convert(paths);
            try
            {
                await _valuations.InsertOneAsync(item);
                
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to insert document: " + ex.Message);
            }
            //burde kunne return noget, men det har mongodb driver ikke
        }
        public async Task<List<ImageResponse>> GetAllItems()
        {
            List<ImageResponse> result = new List<ImageResponse>();
            
            var filter = Builders<ValuationDB>.Filter.Empty;
            var dbData = (await _valuations.FindAsync(filter)).ToList();

            if (dbData.Count == 0)
            {
                throw new ItemsNotFoundException("No items were found in the database.");
            }
            foreach(var item in dbData)
            {
                List<byte[]> img = picService.ReadPicture(item.ImagePaths);
                Valuation valuation = item.Convert();
                ImageResponse combined = new ImageResponse(img,valuation);
                result.Add(combined);
            }
            return result;
        }
        public async Task<List<ImageResponse>> GetByEmail(string email)
        {
            List<ImageResponse> result = new List<ImageResponse>();

            var filter = Builders<ValuationDB>.Filter.Eq(v => v.CustomerEmail, email);
            var dbData = (await _valuations.FindAsync(filter)).ToList();
              if (dbData.Count == 0)
            {
                throw new ItemsNotFoundException($"No item with email {email} was found in the database.");
            }
            foreach(var item in dbData)
            {
                List<byte[]> img = picService.ReadPicture(item.ImagePaths);
                Valuation valuation = item.Convert();
                ImageResponse combined = new ImageResponse(img,valuation);
                result.Add(combined);
            }
            return result;
        }
        public async Task<List<ImageResponse>> GetPending()
        {
            List<ImageResponse> result = new List<ImageResponse>();

            var filter = Builders<ValuationDB>.Filter.Eq(v => v.Status, "pending");
            var dbData = (await _valuations.FindAsync(filter)).ToList();
              if (dbData.Count == 0)
            {
                throw new ItemsNotFoundException($"No pending items was found in the database.");
            }
            foreach(var item in dbData)
            {
                List<byte[]> img = picService.ReadPicture(item.ImagePaths);
                Valuation valuation = item.Convert();
                ImageResponse combined = new ImageResponse(img,valuation);
                result.Add(combined);
            }
            return result;
        }

        public async Task<Valuation> Valuate(ValuationData data)
        {
            var filter = Builders<ValuationDB>.Filter.Eq(v => v.Id, data.Id);
            var itemToUpdate = _valuations.Find(filter).FirstOrDefault();
             if (itemToUpdate == null)
            {
                throw new ItemsNotFoundException($"No item with ID {data.Id} was found in the database for the update.");
            }
            var update = Builders<ValuationDB>.Update.Set(v=>v.ValuationPrice,data.ValuationPrice).Set(v=>v.ValuationReason, data.ValuationReason).Set(v=>v.Status, data.Status);
            ValuationDB dbData = await _valuations.FindOneAndUpdateAsync(filter,update, new FindOneAndUpdateOptions<ValuationDB>{ReturnDocument = ReturnDocument.After});
            return new Valuation(dbData.Id,dbData.Title,dbData.Description,dbData.CustomerEmail,dbData.ValuationPrice,dbData.ValuationReason,dbData.Status);
        }
         public async Task<ImageResponse> DeleteById(string id)
        {
            var filter = Builders<ValuationDB>.Filter.Eq(c => c.Id, id);
            var dbData = await _valuations.FindOneAndDeleteAsync(filter);
             if (dbData == null)
            {
                throw new ItemsNotFoundException($"No item with ID {id} was found in the database for deletion.");
            }
            List<byte[]> img = picService.ReadAndDeletePictures(dbData.ImagePaths);
            Valuation valuation = dbData.Convert();
            ImageResponse combined = new ImageResponse(img,valuation);
            return combined;
        }

}
}