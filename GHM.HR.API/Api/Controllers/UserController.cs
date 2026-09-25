using DevExtreme.AspNet.Data;
using GHM.HR.Domain.IServices;
using GHM.HR.Domain.ModelMetas;
using GHM.HR.Infrastructure.Services;
using GHM.Infrastructure;
using GHM.Infrastructure.CustomAttributes;
using GHM.Infrastructure.SearchRemote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using GHM.HR.Domain;
using GHM.Infrastructure.Models;

namespace GHM.HR.Api.Controllers
{
  
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Users")]
    [SwaggerTag("Danh sach nhan vien")]
    public class UserController : GhmControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("count-by-relationship")]
        public async Task<IActionResult> CountByRelationship(string companyId, CancellationToken cancellationToken)
        {
            var result = await _userService.CountByRelationshipAsync(companyId, CurrentUser, cancellationToken);
            if (result.Code <= 0)
            {
                _logger.LogError("CountByRelationshipAsync");
                return BadRequest(result);
            }
            return Ok(result);
        }

        
        [SwaggerOperation(Summary = "Insert information user.", Description = "Requires login verification!", OperationId = "InsertUser", Tags = new[] { "User" })]
        [AcceptVerbs("POST"), ValidateModel]
        public async Task<IActionResult> InsertAsync([FromBody] UserMeta userMeta)
        {
            var result = await _userService.InsertAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, userMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Insert user controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Update information user.", Description = "Requires login verification!", OperationId = "UpdateUser", Tags = new[] { "User" })]
        [Route("{id}"), AcceptVerbs("PUT"), ValidateModel]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] UserMeta userMeta)
        {
            var result = await _userService.UpdateAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, id, userMeta);
            if (result.Code <= 0)
            {
                _logger.LogError("Update user controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Delete information user.", Description = "Requires login verification!", OperationId = "DeleteUser", Tags = new[] { "User" })]
        [Route("{id}"), AcceptVerbs("DELETE")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _userService.DeleteAsync(CurrentUser.TenantId, CurrentUser.Id, CurrentUser.FullName, CurrentUser.Avatar, id);
            if (result.Code <= 0)
            {
                _logger.LogError("Delete user controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get detail information user.", Description = "Requires login verification!", OperationId = "GetDetailUser", Tags = new[] { "User" })]
        [Route("{id}"), AcceptVerbs("GET")]
        public async Task<IActionResult> DetailAsync(string id)
        {
            var result = await _userService.GetDetailAsync(CurrentUser.TenantId, id);
            if (result.Code <= 0)
            {
                _logger.LogError("Get detail user controller code");
                return BadRequest(result);
            }
            return Ok(result);
        }

        

        [SwaggerOperation(Summary = "Select All information user.", Description = "Requires login verification!", OperationId = "SelectAll", Tags = new[] { "User" })]
        [Route("get-all"), AcceptVerbs("GET")]
        public async Task<IActionResult> SelectAllAsync(string companyId, DateTime? contractExpirationDate)
        {
            var result = await _userService.SelectAllAsync(CurrentUser.TenantId, companyId, contractExpirationDate);
            return Ok(result);
        }

       
        [SwaggerOperation(Summary = "Get all user by Company and Department.", Description = "Requires login verification!", OperationId = "GetAllUsers", Tags = new[] { "User" })]
        [Route("get-all-users"), AcceptVerbs("GET")]
        public async Task<IActionResult> GetAllUsersByCompanyAndDepartmentAsync(string companyId, int departmentId)
        {
            var result = await _userService.SelectAllUsersByCompanyDepartmentAsync(CurrentUser.TenantId, companyId, departmentId);
            return Ok(result);
        }

    }
}
