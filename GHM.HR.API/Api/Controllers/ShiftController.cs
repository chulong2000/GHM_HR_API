using GHM.HR.API.Domain.IServices;
using GHM.HR.API.Domain.ModelMetas;
using GHM.Infrastructure;
using GHM.Infrastructure.CustomAttributes;
using GHM.Infrastructure.SearchRemote;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GHM.HR.API.Api.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Shifts")]
    [SwaggerTag("Insert, Update, Delete, Get Detail, Search Shifts")]
    public class ShiftController : GhmControllerBase
    {
        private readonly IShiftService _shiftService;
        private readonly ILogger<ShiftController> _logger;

        public ShiftController(IShiftService shiftService, ILogger<ShiftController> logger)
        {
            _shiftService = shiftService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Search information shift", Description = "Requires login verification!", OperationId = "SearchRemoteExpressUser", Tags = new[] { "Shift" })]
        [Route("search/{companyId}"), AcceptVerbs("GET")]
        public async Task<IActionResult> SelectAllByCompanyId(string companyId, DataSourceLoadOptions loadOptions)
        {
            string tentantId = "dae13df6-6720-4bda-a61c-61e5e948017e";
            var data = await _shiftService.SelectAllByCompanyIdAsync(tentantId, companyId);
            return Ok(HandlerSearchResult.SearchResult(data, loadOptions));
        }


        [SwaggerOperation(Summary = "Insert information Title.", Description = "Requires login verification!", OperationId = "InsertTitle", Tags = new[] { "Title" })]
        [AcceptVerbs("POST"), ValidateModel]
        public async Task<IActionResult> InsertAsync(
            string tenantId,
            string companyId,
            string createId,
            [FromBody] ShiftMeta shiftMeta)
        {

            var result = await _shiftService.InsertAsync(tenantId, companyId, createId, CurrentUser.FullName, shiftMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Insert Shift controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Update information Shift.", Description = "Requires login verification!", OperationId = "UpdateShift", Tags = new[] { "Shift" })]
        [Route("{id}"), AcceptVerbs("PUT"), ValidateModel]
        public async Task<IActionResult> UpdateAsync(
             string id, 
             string tenantId,
             string companyId,
            [FromBody] ShiftMeta shiftMeta)
        {
            string updateId = "736c3e00-2df2-4c2a-94a9-89cb0f4b55ba";
            string updateName = "Admin";
            var result = await _shiftService.UpdateAsync(updateId, updateName, shiftMeta, id, companyId, tenantId);
            if (result.Code <= 0)
            {
                _logger.LogError("Update Shift controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Delete information Shift.", Description = "Requires login verification!", OperationId = "DeleteShift", Tags = new[] { "Shift" })]
        [Route("{id}"), AcceptVerbs("DELETE")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _shiftService.DeleteAsync(id);
            if (result.Code <= 0)
            {
                _logger.LogError("Delete Shift controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get detail information position.", Description = "Requires login verification!", OperationId = "GetDetailPosition", Tags = new[] { "Position" })]
        [Route("{id}"), AcceptVerbs("GET")]
        public async Task<IActionResult> DetailAsync(string id)
        {
            var result = await _shiftService.GetDetailAsync(id);
            if (result.Code <= 0)
            {
                _logger.LogError("Get detail shift controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
