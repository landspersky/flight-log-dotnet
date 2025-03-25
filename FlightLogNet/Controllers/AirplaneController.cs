namespace FlightLogNet.Controllers
{
    using System.Collections;
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
            return airplaneFacade.GetClubAirplanes();
        }

        // TODO 3.1: Vystavte REST HTTPGet metodu vracející seznam klubových letadel
        // Letadla získáte voláním airplaneFacade
        // dotazované URL je /airplane
        // Odpověď by měla být kolekce AirplaneModel
    }
}
