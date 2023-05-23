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
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        
        try
        {
            var response = await dBService.GetAllItems();
            return Ok(response);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
         catch (Exception ex)
        {
            
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
       
    }
    [HttpGet("getbyemail/{email}")]
    public async Task<IActionResult> GetById([FromRoute]string email)
    {
        try
        {
            var item = await dBService.GetByEmail(email);
            return Ok(item);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Handle other exceptions or unexpected errors
            return StatusCode(500, new { error = "An unexpected error occurred."+ ex.Message });
        }
    }
    [HttpGet("getpending")]
    public async Task<IActionResult> GetPending()
    {
        try
        {
            var item = await dBService.GetPending();
            return Ok(item);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Handle other exceptions or unexpected errors
            return StatusCode(500, new { error = "An unexpected error occurred."+ ex.Message });
        }
    }
    [HttpPut("valuate")]
    public async Task<IActionResult> Valuate([FromBody] ValuationData data)
    {
        try{
        var item = await dBService.Valuate(data);
        return Ok(item);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Handle other exceptions or unexpected errors
            return StatusCode(500, new { error = "An unexpected error occurred."+ ex.Message });
        }

    }
    [HttpDelete("deletebyid/{id}")]
    public async Task<IActionResult> DeleteById([FromRoute]string id)
    {
        try
        {
            var item = await dBService.DeleteById(id);
            return Ok(item);
        }
        catch (ItemsNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Handle other exceptions or unexpected errors
            return StatusCode(500, new { error = "An unexpected error occurred."+ ex.Message });
        }
    }


}