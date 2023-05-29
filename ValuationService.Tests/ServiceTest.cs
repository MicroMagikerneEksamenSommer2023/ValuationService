using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using ValuationService.Services;
using Microsoft.AspNetCore.Mvc;
using ValuationService.Models;
using ValuationService.Controllers;
using Microsoft.AspNetCore.Http;

namespace ValuationService.Tests;

public class Tests
{
    // Attributter til ILogger og IConfuguration
    private ILogger<ValuationController> _logger = null;
    private IConfiguration _configuration = null!;

    // Opsætter testmiljøet ved at initialisere _logger og _configuration
    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<ValuationController>>().Object;

        var myConfiguration = new Dictionary<string, string?>
        {
            {"ValuationServiceBrokerHost", "http://testhost.local"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
    }

    // Tester oprettelse af en vurdering med succes
    [Test]
    public async Task CreateValuationTest_Succes()
    {
        //Arrange
        var valuation = CreateValuation("Y-stol", "Flot moderne stol", "mail@mail.dk");
        List<IFormFile> images = new List<IFormFile>();
        bool succes = true;

        var stubServiceCatalog = new Mock<IMongoService>();
        var stubServicePicture = new Mock<PictureService>();

        stubServiceCatalog.Setup(svc => svc.CreateValuationRequest(images, valuation))
            .Returns(Task.FromResult<bool>(succes));

        var controller = new ValuationController(_logger,_configuration, stubServiceCatalog.Object, stubServicePicture.Object);

        //Act
        var result = await controller.CreateItem(valuation, images);

        //Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>()); 
    }

    // Tester oprettelse af en vurdering med fejl
    [Test]
    public async Task CreateValuationTest_Failure()
    {
        //Arrange
         var valuation = CreateValuation("Y-stol", "Flot moderne stol", "mail@mail.dk");
        List<IFormFile> images = new List<IFormFile>();

        var stubServiceCatalog = new Mock<IMongoService>();
        var stubServicePicture = new Mock<PictureService>();

        stubServiceCatalog.Setup(svc => svc.CreateValuationRequest(images, valuation))
            .ThrowsAsync(new Exception());

        var controller = new ValuationController(_logger,_configuration, stubServiceCatalog.Object, stubServicePicture.Object);

        //Act
        var result = await controller.CreateItem(valuation, images);

        //Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = (ObjectResult)result;
        Assert.AreEqual(500, objectResult.StatusCode);
    }

    // Tester en vurdering med succes
    [Test]
    public async Task ValuateTest_Succes()
    {
        //Arrange
        var valuation = CreateValuation("Y-stol", "Flot moderne stol", "mail@mail.dk");
        var valuate = Valuate("3", 4000, "Indsendt", "Klar");

        var stubServiceCatalog = new Mock<IMongoService>();
        var stubServicePicture = new Mock<PictureService>();

        stubServiceCatalog.Setup(svc => svc.Valuate(valuate))
            .Returns(Task.FromResult<Valuation>(valuation));

        var controller = new ValuationController(_logger,_configuration, stubServiceCatalog.Object, stubServicePicture.Object);

        //Act
        var result = await controller.Valuate(valuate);

        //Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>()); 
    }

    // Tester en vurdering med succes
    [Test]
    public async Task ValuateTest_Failure()
    {
        //Arrange
        var valuate = Valuate("3", 4000, "Indsendt", "Klar");

        var stubServiceCatalog = new Mock<IMongoService>();
        var stubServicePicture = new Mock<PictureService>();

        stubServiceCatalog.Setup(svc => svc.Valuate(valuate))
            .ThrowsAsync(new ItemsNotFoundException());

        var controller = new ValuationController(_logger,_configuration, stubServiceCatalog.Object, stubServicePicture.Object);

        //Act
        var result = await controller.Valuate(valuate);

        //Assert
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>()); 
    }

    /// <summary>
    /// Helper method for creating Valuation instance.
    /// </summary>
    /// <returns></returns>
    private Valuation CreateValuation(string title, string description, string customerEmail)
    {
        var valuation = new Valuation(title, description, customerEmail);

        return valuation;
    }

    /// <summary>
    /// Helper method for creating ValuationData instance.
    /// </summary>
    /// <returns></returns>
    private ValuationData Valuate(string id, double valuationPrice, string valuationReason, string status)
    {
        var valuate = new ValuationData(id, valuationPrice, valuationReason, status);

        return valuate;
    }
}