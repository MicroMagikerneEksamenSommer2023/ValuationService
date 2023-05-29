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
    // Attributter
    private readonly ILogger<ValuationController> _logger;

    private readonly IMongoService dBService;

    private readonly PictureService picService;

    // Cuntructor
    public ValuationController(ILogger<ValuationController> logger, IConfiguration configuration, IMongoService dbservice, PictureService picservice)
    {
        _logger = logger;
        dBService = dbservice;
        picService = picservice;
    }

    // Opretter en vurderingsforespørgsel og gemmer den i databasen
    [HttpPost("requestvaluation")]
    public async Task<IActionResult> CreateItem([ModelBinder(BinderType = typeof(JsonModelBinder))] Valuation data, List<IFormFile> images)
    {
        try
        {
            bool insertedStatus = await dBService.CreateValuationRequest(images, data);
            return Ok(new { message = "Valuation requested.", insertedStatus });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to create valuation request." });
        }
    }

    // Henter alle elementer fra databasen
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

    // Henter alle elementer fra databasen baseret på e-mail
    [HttpGet("getbyemail/{email}")]
    public async Task<IActionResult> GetByEmail([FromRoute] string email)
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
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
    }

    // Henter alle elementer fra databasen med status som pending
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
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
    }

    // Foretag vurdering af et element
    [HttpPut("valuate")]
    public async Task<IActionResult> Valuate([FromBody] ValuationData data)
    {
        try
        {
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
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
    }

    // Sletter et element fra databasen ud fra id
    [HttpDelete("deletebyid/{id}")]
    public async Task<IActionResult> DeleteById([FromRoute] string id)
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
            return StatusCode(500, new { error = "An unexpected error occurred." + ex.Message });
        }
    }
}