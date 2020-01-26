using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using interviewFXS.Models;
using interviewFXS.Exceptions;
using interviewFXS.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace interviewFXS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private static MatchServices _services;
        public MatchController(IConfiguration configuration, MongoClient mongoClient)
        {
            _services = new MatchServices(configuration, mongoClient);
        }
        /// <summary>
        /// Get all type especified model
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<MatchResponseModel>))]
        [ProducesResponseType(400, Type = typeof(ExceptionModel))]
        [ProducesResponseType(500, Type = typeof(String))]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                return Ok(await _services.GetAll());
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
        /// Add one type especified model
        /// </summary>
        /// <returns code="201"> Returns the newly id(string) created of the manager</returns>
        /// <returns code="400"> If the players array is not equal to 11</returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(String))]
        [ProducesResponseType(400, Type = typeof(ExceptionModel))]
        [ProducesResponseType(500, Type = typeof(String))]
        public async Task<IActionResult> AddOneAsync(MatchRequestModel matchRequestModel)
        {
            try
            {
                if (matchRequestModel.HouseTeam.Count.Equals(2) && matchRequestModel.AwayTeam.Count.Equals(2))
                {
                    if (ModelState.IsValid)
                    {
                        return StatusCode(201, await _services.AddOne(matchRequestModel));
                    }
                }
                else
                {
                    throw new FXSException("The players array is not equal to 11", "400");
                }
                throw new FXSException("Model is invalid", "401");
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
        /// Get by id type especified model
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(MatchResponseModel))]
        [ProducesResponseType(400, Type = typeof(ExceptionModel))]
        [ProducesResponseType(500, Type = typeof(String))]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                return Ok(await _services.GetOne(id));
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
        /// Delete model by id type especified model
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(ExceptionModel))]
        [ProducesResponseType(500, Type = typeof(String))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                return Ok(await _services.DeleteOne(id));
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
        /// Update model by id type especified model
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(ExceptionModel))]
        [ProducesResponseType(500, Type = typeof(String))]
        public async Task<IActionResult> UpdateAsync(int id, MatchRequestModel matchRequestModel)
        {
            try
            {
                return Ok(await _services.UpdateOne(id, matchRequestModel));
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
