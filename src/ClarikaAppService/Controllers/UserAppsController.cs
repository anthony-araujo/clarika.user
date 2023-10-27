
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Crosscutting.Exceptions;
using ClarikaAppService.Dto;
using ClarikaAppService.Web.Extensions;
using ClarikaAppService.Web.Filters;
using ClarikaAppService.Web.Rest.Utilities;
using AutoMapper;
using System.Linq;
using ClarikaAppService.Domain.Repositories.Interfaces;
using ClarikaAppService.Domain.Services.Interfaces;
using ClarikaAppService.Infrastructure.Web.Rest.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace ClarikaAppService.Controllers
{
    [Authorize]
    [Route("api/user-apps")]
    [ApiController]
    public class UserAppsController : ControllerBase
    {
        private const string EntityName = "userApp";
        private readonly ILogger<UserAppsController> _log;
        private readonly IMapper _mapper;
        private readonly IUserAppService _userAppService;

        public UserAppsController(ILogger<UserAppsController> log,
        IMapper mapper,
        IUserAppService userAppService)
        {
            _log = log;
            _mapper = mapper;
            _userAppService = userAppService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<UserAppDto>> CreateUserApp([FromBody] UserAppDto userAppDto)
        {
            _log.LogDebug($"REST request to save UserApp : {userAppDto}");
            if (userAppDto.Id != 0)
                throw new BadRequestAlertException("A new userApp cannot already have an ID", EntityName, "idexists");

            UserApp userApp = _mapper.Map<UserApp>(userAppDto);
            await _userAppService.Save(userApp);
            return CreatedAtAction(nameof(GetUserApp), new { id = userApp.Id }, userApp)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, userApp.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateUserApp(long id, [FromBody] UserAppDto userAppDto)
        {
            _log.LogDebug($"REST request to update UserApp : {userAppDto}");
            if (userAppDto.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != userAppDto.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            UserApp userApp = _mapper.Map<UserApp>(userAppDto);
            await _userAppService.Save(userApp);
            return Ok(userApp)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, userApp.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAppDto>>> GetAllUserApps(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of UserApps");
            var result = await _userAppService.FindAll(pageable);
            var page = new Page<UserAppDto>(result.Content.Select(entity => _mapper.Map<UserAppDto>(entity)).ToList(), pageable, result.TotalElements);
            return Ok(((IPage<UserAppDto>)page).Content).WithHeaders(page.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserApp([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get UserApp : {id}");
            var result = await _userAppService.FindOne(id);
            UserAppDto userAppDto = _mapper.Map<UserAppDto>(result);
            return ActionResultUtil.WrapOrNotFound(userAppDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserApp([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete UserApp : {id}");
            await _userAppService.Delete(id);
            return NoContent().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
