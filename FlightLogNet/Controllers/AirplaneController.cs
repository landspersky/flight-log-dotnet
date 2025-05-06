namespace FlightLogNet.Controllers
{
    using System.Collections.Generic;
    using Facades;
    using FlightLogNet.Models;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [EnableCors]
    [ApiController]
    [Route("[controller]")]
    public class AirplaneController(ILogger<AirplaneController> logger, AirplaneFacade airplaneFacade)
        : ControllerBase
    {
        [HttpGet]
        public IEnumerable<AirplaneModel> Get()
        {
            logger.LogDebug("Get airplanes.");
            return airplaneFacade.GetClubAirplanes();
        }
    }
}
