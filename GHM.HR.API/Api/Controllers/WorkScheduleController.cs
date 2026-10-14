using GHM.HR.API.Domain.IServices;
using GHM.HR.API.Domain.ModelMetas;
using GHM.HR.API.Infrastructure.SearchRemote;
using GHM.Infrastructure.CustomAttributes;
using GHM.Infrastructure.SearchRemote;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Xml.Linq;
using DataSourceLoadOptions = GHM.Infrastructure.SearchRemote.DataSourceLoadOptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GHM.HR.API.Api.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/WorkSchedules")]
    [SwaggerTag("Danh sach lịch làm việc")]
    public class WorkScheduleController : ControllerBase
    {
        private readonly IWorkScheduleService _workScheduleService;
        private readonly ILogger<WorkScheduleController> _logger;

        public WorkScheduleController(IWorkScheduleService workScheduleService, ILogger<WorkScheduleController> logger)
        {
            _workScheduleService = workScheduleService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "select all information department active", Description = "Requires login verification!", OperationId = "GetAllDepartmentsActive", Tags = new[] { "Department" })]
        [AcceptVerbs("GET"), Route("work-scheduels/{userId:Guid}/monthly")]
        public async Task<IActionResult> GetAllScheduleInMonthOfUser(
                                                                     string userId,
                                                                     [FromQuery] string companyId,
                                                                     [FromQuery] int year,
                                                                     [FromQuery] int month,
                                                                     DataSourceLoadOptions dataLoadOptions)
        {
            string tenantId = "dae13df6-6720-4bda-a61c-61e5e948017e";
            var data = await _workScheduleService.GetAllScheduleInMonthOfUser(tenantId, companyId, userId, year, month);
            return Ok(HandlerSearchResult.SearchResult(data, dataLoadOptions));
        }


        [SwaggerOperation(Summary = "Insert information department.", Description = "Requires login verification!", OperationId = "InsertDepartment", Tags = new[] { "Department" })]
        [AcceptVerbs("POST"), ValidateModel]
        public async Task<IActionResult> InsertAsync(
            string tenantId,
            string companyId,
            string userId,
            [FromBody] WorkScheduleMeta workScheduleMeta)
        {
            string currentUserId = "da87490a-5e89-4b79-bc47-54a51d8d5b7e";
            string currentUserFullName = "Nguyen Van A";
            var result = await _workScheduleService.InsertAsync(tenantId, companyId, currentUserId, currentUserFullName, workScheduleMeta, userId);
            if (result.Code <= 0)
            {
                _logger.LogError("Insert WorkSchedule controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }


        [SwaggerOperation(Summary = "Delete information department.", Description = "Requires login verification!", OperationId = "DeleteDepartment", Tags = new[] { "Department" })]
        [Route("work-schedule/{id:Guid}"), AcceptVerbs("DELETE")]
        public async Task<IActionResult> DeleteAsync([FromQuery]string tenantId,
                                                     [FromQuery]string companyId,
                                                     string id)
        {
            var result = await _workScheduleService.DeleteAsync(tenantId, companyId, id);
            if (result.Code <= 0)
            {
                _logger.LogError("Delete department controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
