using GHM.HR.API.Domain.IServices;
using GHM.HR.API.Domain.ModelMetas;
using GHM.Infrastructure.CustomAttributes;
using GHM.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GHM.HR.API.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Departments")]
    [SwaggerTag("Insert, Update, Delete, Get Detail, Search Departments")]
    public class DepartmentController : GhmControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "select all information department active", Description = "Requires login verification!", OperationId = "GetAllDepartmentsActive", Tags = new[] { "Department" })]
        [Route("get-all/{companyId}"), AcceptVerbs("GET")]
        public async Task<IActionResult> GetAllDepartmentsAsync(string companyId)
        {
            var data = await _departmentService.SelectAllDepartmentsActionAsync(CurrentUser.TenantId, companyId);
            return Ok(data);
        }


        [SwaggerOperation(Summary = "Insert information department.", Description = "Requires login verification!", OperationId = "InsertDepartment", Tags = new[] { "Department" })]
        [AcceptVerbs("POST"), ValidateModel]
        public async Task<IActionResult> InsertAsync([FromBody] DepartmentMeta departmentMeta)
        {
            var result = await _departmentService.InsertAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, departmentMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Insert Department controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Update information department.", Description = "Requires login verification!", OperationId = "UpdateDepartment", Tags = new[] { "Department" })]
        [Route("{id}"), AcceptVerbs("PUT"), ValidateModel]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] DepartmentMeta departmentMeta)
        {
            var result = await _departmentService.UpdateAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, id, departmentMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Update Department controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get detail information department.", Description = "Requires login verification!", OperationId = "GetDetailDepartment", Tags = new[] { "Department" })]
        [Route("{id}"), AcceptVerbs("GET")]
        public async Task<IActionResult> DetailAsync(int id)
        {
            var result = await _departmentService.GetDetailAsync(CurrentUser.TenantId, id);
            if (result.Code <= 0)
            {
                _logger.LogError("Get detail department controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Delete information department.", Description = "Requires login verification!", OperationId = "DeleteDepartment", Tags = new[] { "Department" })]
        [Route("{id}"), AcceptVerbs("DELETE")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _departmentService.DeleteAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, id);
            if (result.Code <= 0)
            {
                _logger.LogError("Delete department controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get department tree information.", Description = "Requires login verification!", OperationId = "GetDepartmentTree", Tags = new[] { "Department" })]
        [Route("trees/{companyId}"), AcceptVerbs("GET")]
        public async Task<IActionResult> GetDepartmentTree(string companyId)
        {
            var result = await _departmentService.GetFullTreeAsync(CurrentUser.TenantId, companyId);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get department tree active information.", Description = "Requires login verification!", OperationId = "GetDepartmentTreeActive", Tags = new[] { "Department" })]
        [Route("trees-active/{companyId}"), AcceptVerbs("GET")]
        public async Task<IActionResult> GetDepartmentTreeACtive(string companyId)
        {
            var result = await _departmentService.GetFullTreeActiveAsync(CurrentUser.TenantId, companyId);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get department tree by user information.", Description = "Requires login verification!", OperationId = "GetDepartmentTreeByUser", Tags = new[] { "Department" })]
        [Route("trees-user/{companyId}"), AcceptVerbs("GET")]
        public async Task<IActionResult> GetDepartmentTreeByUser(string companyId)
        {
            var result = await _departmentService.GetFullTreeByUserAsync(CurrentUser.TenantId, companyId, CurrentUser.Id);
            return Ok(result);
        }
    }
}
