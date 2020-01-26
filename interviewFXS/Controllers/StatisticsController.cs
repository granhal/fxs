using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using interviewFXS.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using interviewFXS.Services;
using interviewFXS.Exceptions;

namespace interviewFXS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private static StatiticsServices _services;
        public StatisticsController(IConfiguration configuration, MongoClient mongoClient)
        {
            _services = new StatiticsServices(configuration, mongoClient);
        }
        /// <summary>
        /// Return statistics to yellow cards
        /// </summary>
        /// <returns></returns>
        [HttpGet("yellowcards")]
        [ProducesResponseType(200, Type = typeof(IList<StatisticsCardsResponseModel>))]
        [ProducesResponseType(400, Type = typeof(ExceptionModel))]
        [ProducesResponseType(500, Type = typeof(String))]
        public IActionResult GetYellowCards()
        {
            try
            { 
                return Ok(_services.GetYellowCards());
            }
            catch (FXSException fxse)
            {
                return StatusCode(Int32.Parse(fxse.Code), new ExceptionModel { Message = fxse.Message, Code = fxse.Code });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Return statistics to red cards
        /// </summary>
        /// <returns></returns>
        [HttpGet("redcards")]
        [ProducesResponseType(200, Type = typeof(IList<StatisticsCardsResponseModel>))]
        [ProducesResponseType(400, Type = typeof(ExceptionModel))]
        [ProducesResponseType(500, Type = typeof(String))]
        public IActionResult GetRedCards()
        {
            try
            { 
                return Ok(_services.GetRedCards());
            }
            catch (FXSException fxse)
            {
                return StatusCode(Int32.Parse(fxse.Code), new ExceptionModel { Message = fxse.Message, Code = fxse.Code });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Return statistics to minutes
        /// </summary>
        /// <returns></returns>
        [HttpGet("minutes")]
        [ProducesResponseType(200, Type = typeof(IList<StatisticsMinutesResponseModel>))]
        [ProducesResponseType(400, Type = typeof(ExceptionModel))]
        [ProducesResponseType(500, Type = typeof(String))]
        public IActionResult GetMinutes()
        {
            try { 
                return Ok(_services.GetMinutes());
            }
            catch (FXSException fxse)
            {
                return StatusCode(Int32.Parse(fxse.Code), new ExceptionModel { Message = fxse.Message, Code = fxse.Code });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
