using GHM.HR.Domain.IServices;
using GHM.HR.Domain.ModelMetas;
using GHM.Infrastructure;
using GHM.Infrastructure.CustomAttributes;
using GHM.Infrastructure.SearchRemote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace GHM.HR.Api.Controllers
{
    
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Companys")]
    [SwaggerTag("Get Detail Company")]
    public class CompanyController : GhmControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Get detail company.", Description = "Requires login verification!", OperationId = "GetDetailCompany", Tags = new[] { "Company" })]
        [Route("get-detail/{companyId}"), AcceptVerbs("GET")]
        public async Task<IActionResult> GetDetailCompanyAsync(string companyId)
        {
            var result = await _companyService.GetDetailCompanyAsync(CurrentUser.TenantId, companyId);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get all information companys.", Description = "Requires login verification!", OperationId = "GetAllCompanys", Tags = new[] { "Company" })]
        [Route("get-all"), AcceptVerbs("GET")]
        public async Task<IActionResult> SelectAllAsync()
        {
            var result = await _companyService.SelectAllAsync(CurrentUser.TenantId);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Insert information companys.", Description = "Requires login verification!", OperationId = "InsertCompanys", Tags = new[] { "Company" })]
        [AcceptVerbs("POST"), ValidateModel]
        public async Task<IActionResult> InsertAsync([FromBody] CompanyMeta companyMeta)
        {
            var result = await _companyService.InsertAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, companyMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Insert company controller error.");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Update information companys.", Description = "Requires login verification!", OperationId = "UpdateCompanys", Tags = new[] { "Company" })]
        [Route("{id}"), AcceptVerbs("PUT"), ValidateModel]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] CompanyMeta companyMeta)
        {
            var result = await _companyService.UpdateAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, id, companyMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Update company controller error.");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Delete information companys.", Description = "Requires login verification!", OperationId = "DeleteCompanys", Tags = new[] { "Company" })]
        [Route("{id}"), AcceptVerbs("DELETE")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _companyService.DeleteAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, id);
            if (result.Code <= 0)
            {
                _logger.LogError("Delete company controller error.");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get detail information companys.", Description = "Requires login verification!", OperationId = "GetDetailCompanys", Tags = new[] { "Company" })]
        [Route("{id}"), AcceptVerbs("GET")]
        public async Task<IActionResult> DetailAsync(string id)
        {
            var result = await _companyService.GetDetailAsync(CurrentUser.TenantId, id);
            if (result.Code <= 0)
            {
                _logger.LogError("Get detail company controller error.");
                return BadRequest(result);
            }
            return Ok(result);
        }

        
        [SwaggerOperation(Summary = "Get logo company.", Description = "Requires login verification!", OperationId = "GetLogoCompany", Tags = new[] { "Company" })]
        [Route("get-logo/{companyId}"), AcceptVerbs("GET")]
        public async Task<IActionResult> SelectLogoAsync(string companyId)
        {
            var result = await _companyService.GetLogoAsync(CurrentUser.TenantId, companyId);
            return Ok(result);
        }
    }
}
