
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
    [Route("api/location-types")]
    [ApiController]
    public class LocationTypesController : ControllerBase
    {
        private const string EntityName = "locationType";
        private readonly ILogger<LocationTypesController> _log;
        private readonly IMapper _mapper;
        private readonly ILocationTypeService _locationTypeService;

        public LocationTypesController(ILogger<LocationTypesController> log,
        IMapper mapper,
        ILocationTypeService locationTypeService)
        {
            _log = log;
            _mapper = mapper;
            _locationTypeService = locationTypeService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<LocationTypeDto>> CreateLocationType([FromBody] LocationTypeDto locationTypeDto)
        {
            _log.LogDebug($"REST request to save LocationType : {locationTypeDto}");
            if (locationTypeDto.Id != 0)
                throw new BadRequestAlertException("A new locationType cannot already have an ID", EntityName, "idexists");

            LocationType locationType = _mapper.Map<LocationType>(locationTypeDto);
            await _locationTypeService.Save(locationType);
            return CreatedAtAction(nameof(GetLocationType), new { id = locationType.Id }, locationType)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, locationType.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateLocationType(long id, [FromBody] LocationTypeDto locationTypeDto)
        {
            _log.LogDebug($"REST request to update LocationType : {locationTypeDto}");
            if (locationTypeDto.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != locationTypeDto.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            LocationType locationType = _mapper.Map<LocationType>(locationTypeDto);
            await _locationTypeService.Save(locationType);
            return Ok(locationType)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, locationType.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationTypeDto>>> GetAllLocationTypes(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of LocationTypes");
            var result = await _locationTypeService.FindAll(pageable);
            var page = new Page<LocationTypeDto>(result.Content.Select(entity => _mapper.Map<LocationTypeDto>(entity)).ToList(), pageable, result.TotalElements);
            return Ok(((IPage<LocationTypeDto>)page).Content).WithHeaders(page.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationType([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get LocationType : {id}");
            var result = await _locationTypeService.FindOne(id);
            LocationTypeDto locationTypeDto = _mapper.Map<LocationTypeDto>(result);
            return ActionResultUtil.WrapOrNotFound(locationTypeDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocationType([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete LocationType : {id}");
            await _locationTypeService.Delete(id);
            return NoContent().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
