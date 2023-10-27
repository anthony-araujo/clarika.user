
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
    [Route("api/user-locations")]
    [ApiController]
    public class UserLocationsController : ControllerBase
    {
        private const string EntityName = "userLocation";
        private readonly ILogger<UserLocationsController> _log;
        private readonly IMapper _mapper;
        private readonly IUserLocationService _userLocationService;

        public UserLocationsController(ILogger<UserLocationsController> log,
        IMapper mapper,
        IUserLocationService userLocationService)
        {
            _log = log;
            _mapper = mapper;
            _userLocationService = userLocationService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<UserLocationDto>> CreateUserLocation([FromBody] UserLocationDto userLocationDto)
        {
            _log.LogDebug($"REST request to save UserLocation : {userLocationDto}");
            if (userLocationDto.Id != 0)
                throw new BadRequestAlertException("A new userLocation cannot already have an ID", EntityName, "idexists");

            UserLocation userLocation = _mapper.Map<UserLocation>(userLocationDto);
            await _userLocationService.Save(userLocation);
            return CreatedAtAction(nameof(GetUserLocation), new { id = userLocation.Id }, userLocation)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, userLocation.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateUserLocation(long id, [FromBody] UserLocationDto userLocationDto)
        {
            _log.LogDebug($"REST request to update UserLocation : {userLocationDto}");
            if (userLocationDto.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != userLocationDto.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            UserLocation userLocation = _mapper.Map<UserLocation>(userLocationDto);
            await _userLocationService.Save(userLocation);
            return Ok(userLocation)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, userLocation.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLocationDto>>> GetAllUserLocations(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of UserLocations");
            var result = await _userLocationService.FindAll(pageable);
            var page = new Page<UserLocationDto>(result.Content.Select(entity => _mapper.Map<UserLocationDto>(entity)).ToList(), pageable, result.TotalElements);
            return Ok(((IPage<UserLocationDto>)page).Content).WithHeaders(page.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserLocation([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get UserLocation : {id}");
            var result = await _userLocationService.FindOne(id);
            UserLocationDto userLocationDto = _mapper.Map<UserLocationDto>(result);
            return ActionResultUtil.WrapOrNotFound(userLocationDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserLocation([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete UserLocation : {id}");
            await _userLocationService.Delete(id);
            return NoContent().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
