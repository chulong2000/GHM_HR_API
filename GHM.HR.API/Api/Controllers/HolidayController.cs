using GHM.HR.API.Domain.IServices;
using GHM.HR.API.Domain.ModelMetas;
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


    }
}
