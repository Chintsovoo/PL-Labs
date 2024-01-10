using Lab4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers;

[ApiController]
[Route("/api/calculator")]
public class AppController(ILogger<AppController> logger, CalculatorService service) : ControllerBase
{
    private readonly ILogger<AppController> _logger = logger;

    [HttpGet]
    public ActionResult<Dictionary<int, double>> GetHistory()
    {
        logger.LogInformation("Got request to fetch history");
        return Ok(service.GetAllValues());
    }

    [HttpPost]
    public ActionResult AddValue([FromBody] double value)
    {
        logger.LogInformation("Got request to add value {Value}", value);
        service.AddValue(value);
        return Ok();
    }
    
    [HttpPost("evaluate/{operation}")]
    public ActionResult<double> Evaluate([FromBody] double value, char operation)
    {
        logger.LogInformation("Got request to evaluate value {Value} by {Operation} ", value, operation);
        double result = 0;
        try
        {
            result = service.Evaluate(value, operation);
        }
        catch (OperationNotFound e)
        {
            logger.LogInformation("Operation not valid {Operation}", operation);
            return BadRequest("Operation's are +,-,*,/ ");
        }
        return Ok(result);
    }

    [HttpPost("history")]
    public ActionResult TakeValueFromHistory([FromBody] int step)
    {
        logger.LogInformation("Got request to take value from history step = {Step} ", step);
        try
        {
            var value = service.GetValueAtStep(step);
            service.AddValue(value);
        }
        catch (KeyNotFoundException)
        {
            return BadRequest("There is no step " + step);
        }

        return Ok();
    }
    
}
