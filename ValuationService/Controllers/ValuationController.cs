using ValuationService.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ValuationService.Services;
using MongoDB.Bson;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ValuationService.Controllers;


[ApiController]
[Route("valuationservice/v1")]
public class ValuationController : ControllerBase
{


    private readonly ILogger<ValuationController> _logger;

    private readonly MongoService dBService;

    private readonly PictureService picService;

    public ValuationController(ILogger<ValuationController> logger, MongoService dbservice, PictureService picservice)
    {
        _logger = logger;
        dBService = dbservice;
        picService = picservice;
    }


     [HttpPost("requestvaluation")]
    public async Task<IActionResult> CreateItem([ModelBinder(BinderType = typeof(JsonModelBinder))] Valuation data,List<IFormFile> images)
    {
        //burde return noget, men kan ikke fetche id
         try
    {
        bool insertedStatus = await dBService.CreateCatalogItem(images, data);
        return Ok(new { message = "Catalog item created successfully.", insertedStatus });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { error = "Failed to create catalog item." });
    }
    }
}