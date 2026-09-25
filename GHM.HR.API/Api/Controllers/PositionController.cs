using GHM.HR.Domain.IServices;
using GHM.HR.Domain.ModelMetas;
using GHM.HR.Infrastructure.Services;
using GHM.Infrastructure;
using GHM.Infrastructure.CustomAttributes;
using GHM.Infrastructure.SearchRemote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace GHM.HR.Api.Controllers
{
    
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Positions")]
    [SwaggerTag("Insert, Update, Delete, Get Detail, Search Positions")]
    public class PositionController : GhmControllerBase
    {
        private readonly IPositionService _positionService;
        private readonly ILogger<PositionController> _logger;

        public PositionController(IPositionService positionService, ILogger<PositionController> logger)
        {
            _positionService = positionService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Get all information Position active.", Description = "Requires login verification!", OperationId = "GetAllPositionsActive", Tags = new[] { "Position" })]
        [AcceptVerbs("GET"), Route("get-all-active/{companyId}")]
        public async Task<IActionResult> GetAllPositionActiveAsync(string companyId)
        {
            var result = await _positionService.SelectAllPositionActiveAsync(CurrentUser.TenantId, companyId);
            return Ok(result);
        }


        [SwaggerOperation(Summary = "Insert information Position.", Description = "Requires login verification!", OperationId = "InsertPosition", Tags = new[] { "Position" })]
        [AcceptVerbs("POST"), ValidateModel]
        public async Task<IActionResult> InsertAsync([FromBody] PositionMeta positionMeta)
        {
            var result = await _positionService.InsertAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, positionMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Insert Position controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Update information Position.", Description = "Requires login verification!", OperationId = "UpdatePosition", Tags = new[] { "Position" })]
        [Route("{id}"), AcceptVerbs("PUT"), ValidateModel]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] PositionMeta positionMeta)
        {
            var result = await _positionService.UpdateAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, id, positionMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Update Position controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Delete information position.", Description = "Requires login verification!", OperationId = "DeletePosition", Tags = new[] { "Position" })]
        [Route("{id}"), AcceptVerbs("DELETE")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _positionService.DeleteAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, id);
            if (result.Code <= 0)
            {
                _logger.LogError("Delete position controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get detail information position.", Description = "Requires login verification!", OperationId = "GetDetailPosition", Tags = new[] { "Position" })]
        [Route("{id}"), AcceptVerbs("GET")]
        public async Task<IActionResult> DetailAsync(string id)
        {
            var result = await _positionService.GetDetailAsync(CurrentUser.TenantId,id);
            if (result.Code <= 0)
            {
                _logger.LogError("Get detail position controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Update IsActive information position.", Description = "Requires login verification!", OperationId = "UpdateIsActivePositionById", Tags = new[] { "Position" })]
        [Route("update-by-Id/{companyId}/{id}"), AcceptVerbs("PUT")]
        public async Task<IActionResult> UpdateIsActiveAsync(string companyId,string id, bool isActive)
        {
            var result = await _positionService.UpdateIsActive(companyId,id, isActive);
            return Ok(result);
        }
    }
}
