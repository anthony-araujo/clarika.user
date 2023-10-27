
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
    [Route("api/states")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private const string EntityName = "state";
        private readonly ILogger<StatesController> _log;
        private readonly IMapper _mapper;
        private readonly IStateService _stateService;

        public StatesController(ILogger<StatesController> log,
        IMapper mapper,
        IStateService stateService)
        {
            _log = log;
            _mapper = mapper;
            _stateService = stateService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<StateDto>> CreateState([FromBody] StateDto stateDto)
        {
            _log.LogDebug($"REST request to save State : {stateDto}");
            if (stateDto.Id != 0)
                throw new BadRequestAlertException("A new state cannot already have an ID", EntityName, "idexists");

            State state = _mapper.Map<State>(stateDto);
            await _stateService.Save(state);
            return CreatedAtAction(nameof(GetState), new { id = state.Id }, state)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, state.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateState(long id, [FromBody] StateDto stateDto)
        {
            _log.LogDebug($"REST request to update State : {stateDto}");
            if (stateDto.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != stateDto.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            State state = _mapper.Map<State>(stateDto);
            await _stateService.Save(state);
            return Ok(state)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, state.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateDto>>> GetAllStates(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of States");
            var result = await _stateService.FindAll(pageable);
            var page = new Page<StateDto>(result.Content.Select(entity => _mapper.Map<StateDto>(entity)).ToList(), pageable, result.TotalElements);
            return Ok(((IPage<StateDto>)page).Content).WithHeaders(page.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetState([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get State : {id}");
            var result = await _stateService.FindOne(id);
            StateDto stateDto = _mapper.Map<StateDto>(result);
            return ActionResultUtil.WrapOrNotFound(stateDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete State : {id}");
            await _stateService.Delete(id);
            return NoContent().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
