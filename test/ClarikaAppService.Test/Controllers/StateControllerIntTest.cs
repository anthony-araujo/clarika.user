
using AutoMapper;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ClarikaAppService.Infrastructure.Data;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Repositories.Interfaces;
using ClarikaAppService.Dto;
using ClarikaAppService.Configuration.AutoMapper;
using ClarikaAppService.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ClarikaAppService.Test.Controllers
{
    public class StatesControllerIntTest
    {
        public StatesControllerIntTest()
        {
            _factory = new AppWebApplicationFactory<TestStartup>().WithMockUser();
            _client = _factory.CreateClient();

            _stateRepository = _factory.GetRequiredService<IStateRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            _mapper = config.CreateMapper();

            InitTest();
        }

        private const string DefaultName = "AAAAAAAAAA";
        private const string UpdatedName = "BBBBBBBBBB";

        private readonly AppWebApplicationFactory<TestStartup> _factory;
        private readonly HttpClient _client;
        private readonly IStateRepository _stateRepository;

        private State _state;

        private readonly IMapper _mapper;

        private State CreateEntity()
        {
            return new State
            {
                Name = DefaultName,
            };
        }

        private void InitTest()
        {
            _state = CreateEntity();
        }

        //[Fact]
        //public async Task CreateState()
        //{
        //    var databaseSizeBeforeCreate = await _stateRepository.CountAsync();

        //    // Create the State
        //    StateDto _stateDto = _mapper.Map<StateDto>(_state);
        //    var response = await _client.PostAsync("/api/states", TestUtil.ToJsonContent(_stateDto));
        //    response.StatusCode.Should().Be(HttpStatusCode.Created);

        //    // Validate the State in the database
        //    var stateList = await _stateRepository.GetAllAsync();
        //    stateList.Count().Should().Be(databaseSizeBeforeCreate + 1);
        //    var testState = stateList.Last();
        //    testState.Name.Should().Be(DefaultName);
        //}

        [Fact]
        public async Task CreateStateWithExistingId()
        {
            var databaseSizeBeforeCreate = await _stateRepository.CountAsync();
            // Create the State with an existing ID
            _state.Id = 1L;

            // An entity with an existing ID cannot be created, so this API call must fail
            StateDto _stateDto = _mapper.Map<StateDto>(_state);
            var response = await _client.PostAsync("/api/states", TestUtil.ToJsonContent(_stateDto));

            // Validate the State in the database
            var stateList = await _stateRepository.GetAllAsync();
            stateList.Count().Should().Be(databaseSizeBeforeCreate);
        }

        [Fact]
        public async Task CheckNameIsRequired()
        {
            var databaseSizeBeforeTest = await _stateRepository.CountAsync();

            // Set the field to null
            _state.Name = null;

            // Create the State, which fails.
            StateDto _stateDto = _mapper.Map<StateDto>(_state);
            var response = await _client.PostAsync("/api/states", TestUtil.ToJsonContent(_stateDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var stateList = await _stateRepository.GetAllAsync();
            stateList.Count().Should().Be(databaseSizeBeforeTest);
        }

        //[Fact]
        //public async Task GetAllStates()
        //{
        //    // Initialize the database
        //    await _stateRepository.CreateOrUpdateAsync(_state);
        //    await _stateRepository.SaveChangesAsync();

        //    // Get all the stateList
        //    var response = await _client.GetAsync("/api/states?sort=id,desc");
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);

        //    var json = JToken.Parse(await response.Content.ReadAsStringAsync());
        //    json.SelectTokens("$.[*].id").Should().Contain(_state.Id);
        //    json.SelectTokens("$.[*].name").Should().Contain(DefaultName);
        //}

        //[Fact]
        //public async Task GetState()
        //{
        //    // Initialize the database
        //    await _stateRepository.CreateOrUpdateAsync(_state);
        //    await _stateRepository.SaveChangesAsync();

        //    // Get the state
        //    var response = await _client.GetAsync($"/api/states/{_state.Id}");
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);

        //    var json = JToken.Parse(await response.Content.ReadAsStringAsync());
        //    json.SelectTokens("$.id").Should().Contain(_state.Id);
        //    json.SelectTokens("$.name").Should().Contain(DefaultName);
        //}

        //[Fact]
        //public async Task GetNonExistingState()
        //{
        //    var maxValue = long.MaxValue;
        //    var response = await _client.GetAsync("/api/states/" + maxValue);
        //    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        //}

        //[Fact]
        //public async Task UpdateState()
        //{
        //    // Initialize the database
        //    await _stateRepository.CreateOrUpdateAsync(_state);
        //    await _stateRepository.SaveChangesAsync();
        //    var databaseSizeBeforeUpdate = await _stateRepository.CountAsync();

        //    // Update the state
        //    var updatedState = await _stateRepository.QueryHelper().GetOneAsync(it => it.Id == _state.Id);
        //    // Disconnect from session so that the updates on updatedState are not directly saved in db
        //    //TODO detach
        //    updatedState.Name = UpdatedName;

        //    StateDto updatedStateDto = _mapper.Map<StateDto>(updatedState);
        //    var response = await _client.PutAsync($"/api/states/{_state.Id}", TestUtil.ToJsonContent(updatedStateDto));
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);

        //    // Validate the State in the database
        //    var stateList = await _stateRepository.GetAllAsync();
        //    stateList.Count().Should().Be(databaseSizeBeforeUpdate);
        //    var testState = stateList.Last();
        //    testState.Name.Should().Be(UpdatedName);
        //}

        [Fact]
        public async Task UpdateNonExistingState()
        {
            var databaseSizeBeforeUpdate = await _stateRepository.CountAsync();

            // If the entity doesn't have an ID, it will throw BadRequestAlertException
            StateDto _stateDto = _mapper.Map<StateDto>(_state);
            var response = await _client.PutAsync("/api/states/1", TestUtil.ToJsonContent(_stateDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the State in the database
            var stateList = await _stateRepository.GetAllAsync();
            stateList.Count().Should().Be(databaseSizeBeforeUpdate);
        }

        //[Fact]
        //public async Task DeleteState()
        //{
        //    // Initialize the database
        //    await _stateRepository.CreateOrUpdateAsync(_state);
        //    await _stateRepository.SaveChangesAsync();
        //    var databaseSizeBeforeDelete = await _stateRepository.CountAsync();

        //    var response = await _client.DeleteAsync($"/api/states/{_state.Id}");
        //    response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        //    // Validate the database is empty
        //    var stateList = await _stateRepository.GetAllAsync();
        //    stateList.Count().Should().Be(databaseSizeBeforeDelete - 1);
        //}

        [Fact]
        public void EqualsVerifier()
        {
            TestUtil.EqualsVerifier(typeof(State));
            var state1 = new State
            {
                Id = 1L
            };
            var state2 = new State
            {
                Id = state1.Id
            };
            state1.Should().Be(state2);
            state2.Id = 2L;
            state1.Should().NotBe(state2);
            state1.Id = 0;
            state1.Should().NotBe(state2);
        }
    }
}
