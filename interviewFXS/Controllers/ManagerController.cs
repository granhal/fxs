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
    public class ManagerController : ControllerBase
    {
        private static ManagerServices _services;
        public ManagerController(IConfiguration configuration, MongoClient mongoClient)
        {
            _services = new ManagerServices(configuration, mongoClient);
        }
        /// <summary>
        /// Get all type especified model
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<ManagerResponseModel>))]
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
        /// <returns code="201"> Returns the newly id(int) created of the manager</returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(String))]
        [ProducesResponseType(400, Type = typeof(ExceptionModel))]
        [ProducesResponseType(500, Type = typeof(String))]
        public async Task<IActionResult> AddOneAsync(ManagerRequestModel managerRequestModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(await _services.AddOne(managerRequestModel));
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
        [ProducesResponseType(200, Type = typeof(ManagerResponseModel))]
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
        public async Task<IActionResult> UpdateAsync(int id, ManagerRequestModel managerRequestModel)
        {
            try
            {
                return Ok(await _services.UpdateOne(id, managerRequestModel));
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
