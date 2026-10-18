using GHM.HR.API.Domain.IServices;
using GHM.HR.API.Domain.ModelMetas;
using GHM.HR.API.Infrastructure.Service;
using GHM.Infrastructure;
using GHM.Infrastructure.CustomAttributes;
using GHM.Infrastructure.SearchRemote;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GHM.HR.API.Api.Controllers
{

    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Holidays")]
    [SwaggerTag("Insert, Update, Delete, Get Detail, Search Holidays")]
    public class HolidayController : GhmControllerBase
    {
        private readonly IHolidayService _holidayService;
        private readonly ILogger<HolidayController> _logger;

        public HolidayController(IHolidayService holidayService, ILogger<HolidayController> logger)
        {
            _holidayService = holidayService;
            _logger = logger;
        }


        [SwaggerOperation(Summary = "Get all information companys.", Description = "Requires login verification!", OperationId = "GetAllCompanys", Tags = new[] { "Company" })]
        [Route("get-all/{companyId}/{tenantId}"), AcceptVerbs("GET")]
        public async Task<IActionResult> SelectAllAsync(string companyId, string tenantId)
        {
            var result = await _holidayService.SelectAllByCompanyIdAsync(tenantId, companyId);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Insert information department.", Description = "Requires login verification!", OperationId = "InsertDepartment", Tags = new[] { "Department" })]
        [AcceptVerbs("POST"), ValidateModel]
        public async Task<IActionResult> InsertAsync(
            string tenantId,
            string companyId,
            [FromBody] HolidayMeta holidayMeta)
        {
            string currentUserId = "da87490a-5e89-4b79-bc47-54a51d8d5b7e";
            string currentUserFullName = "Nguyen Van A";
            var result = await _holidayService.InsertAsync(tenantId, companyId, currentUserId, currentUserFullName, holidayMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Insert Holiday controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }


        [SwaggerOperation(Summary = "Update information Holiday.", Description = "Requires login verification!", OperationId = "UpdateHoliday", Tags = new[] { "Holiday" })]
        [Route("{id}"), AcceptVerbs("PUT"), ValidateModel]
        public async Task<IActionResult> UpdateAsync(
             string id,
             string tenantId,
             string companyId,
            [FromBody] HolidayMeta holidayMeta)
        {
            string updateId = "736c3e00-2df2-4c2a-94a9-89cb0f4b55ba";
            string updateName = "Admin";
            var result = await _holidayService.UpdateAsync(updateId, updateName, holidayMeta, id, companyId, tenantId);
            if (result.Code <= 0)
            {
                _logger.LogError("Update Holiday controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Delete information Holiday.", Description = "Requires login verification!", OperationId = "DeleteHoliday", Tags = new[] { "Holiday" })]
        [Route("{id}"), AcceptVerbs("DELETE")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            string deleteUserId = "da87490a-5e89-4b79-bc47-54a51d8d5b7e";
            string deleteFullName = "Nguyen Van A";
            var result = await _holidayService.DeleteAsync(id, deleteUserId, deleteFullName);
            if (result.Code <= 0)
            {
                _logger.LogError("Delete Holiday controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }


    }
}
