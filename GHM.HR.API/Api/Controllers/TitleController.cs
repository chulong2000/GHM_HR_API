using GHM.HR.API.Domain.IServices;
using GHM.Infrastructure.CustomAttributes;
using GHM.Infrastructure.SearchRemote;
using GHM.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using GHM.HR.API.Domain.ModelMetas;

namespace GHM.HR.API.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Titles")]
    [SwaggerTag("Insert, Update, Delete, Get Detail, Search Titles")]
    public class TitleController : GhmControllerBase
    {
        private readonly ITitleService _titleService;
        private readonly ILogger<TitleController> _logger;

        public TitleController(ITitleService titleService, ILogger<TitleController> logger)
        {
            _titleService = titleService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Search information title", Description = "Requires login verification!", OperationId = "SearchRemoteExpressUser", Tags = new[] { "Title" })]
        [Route("search/{companyId}"), AcceptVerbs("GET")]
        public async Task<IActionResult> SearchRemoteAsync(string companyId, DataSourceLoadOptions loadOptions)
        {
            var data = await _titleService.SelectAllAsync(CurrentUser.TenantId, companyId);
            return Ok(HandlerSearchResult.SearchResult(data, loadOptions));
        }


        [SwaggerOperation(Summary = "Get all information title active.", Description = "Requires login verification!", OperationId = "GetAllTitlesActive", Tags = new[] { "Title" })]
        [AcceptVerbs("GET"), Route("get-all-active/{companyId}")]
        public async Task<IActionResult> GetAllTitleActiveAsync(string companyId)
        {
            var result = await _titleService.SelectAllTitleActiveAsync(CurrentUser.TenantId, companyId);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Insert information Title.", Description = "Requires login verification!", OperationId = "InsertTitle", Tags = new[] { "Title" })]
        [AcceptVerbs("POST"), ValidateModel]
        public async Task<IActionResult> InsertAsync([FromBody] TitleMeta titleMeta)
        {
            var result = await _titleService.InsertAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, titleMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Insert Title controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Update information Title.", Description = "Requires login verification!", OperationId = "UpdateTitle", Tags = new[] { "Title" })]
        [Route("{id}"), AcceptVerbs("PUT"), ValidateModel]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] TitleMeta TitleMeta)
        {
            var result = await _titleService.UpdateAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, id, TitleMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Update Title controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Delete information Title.", Description = "Requires login verification!", OperationId = "DeleteTitle", Tags = new[] { "Title" })]
        [Route("{id}"), AcceptVerbs("DELETE")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _titleService.DeleteAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, id);
            if (result.Code <= 0)
            {
                _logger.LogError("Delete Title controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get detail information Title.", Description = "Requires login verification!", OperationId = "GetDetailTitle", Tags = new[] { "Title" })]
        [Route("{id}"), AcceptVerbs("GET")]
        public async Task<IActionResult> DetailAsync(string id)
        {
            var result = await _titleService.GetDetailAsync(CurrentUser.TenantId, id);
            if (result.Code <= 0)
            {
                _logger.LogError("Get detail Title controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Update IsActive information Title.", Description = "Requires login verification!", OperationId = "UpdateIsActiveTitleById", Tags = new[] { "Title" })]
        [Route("update-by-Id/{companyId}/{id}"), AcceptVerbs("PUT")]
        public async Task<IActionResult> UpdateIsActiveAsync(string companyId, string id, bool isActive)
        {
            var result = await _titleService.UpdateIsActive(companyId, id, isActive);
            return Ok(result);
        }
    }
}
