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

}
}