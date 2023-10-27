
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
    public class LocationTypesControllerIntTest
    {
        public LocationTypesControllerIntTest()
        {
            _factory = new AppWebApplicationFactory<TestStartup>().WithMockUser();
            _client = _factory.CreateClient();

            _locationTypeRepository = _factory.GetRequiredService<ILocationTypeRepository>();

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
        private readonly ILocationTypeRepository _locationTypeRepository;

        private LocationType _locationType;

        private readonly IMapper _mapper;

        private LocationType CreateEntity()
        {
            return new LocationType
            {
                Name = DefaultName,
            };
        }

        private void InitTest()
        {
            _locationType = CreateEntity();
        }

        [Fact]
        public async Task CreateLocationType()
        {
            var databaseSizeBeforeCreate = await _locationTypeRepository.CountAsync();

            // Create the LocationType
            LocationTypeDto _locationTypeDto = _mapper.Map<LocationTypeDto>(_locationType);
            var response = await _client.PostAsync("/api/location-types", TestUtil.ToJsonContent(_locationTypeDto));
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Validate the LocationType in the database
            var locationTypeList = await _locationTypeRepository.GetAllAsync();
            locationTypeList.Count().Should().Be(databaseSizeBeforeCreate + 1);
            var testLocationType = locationTypeList.Last();
            testLocationType.Name.Should().Be(DefaultName);
        }

        [Fact]
        public async Task CreateLocationTypeWithExistingId()
        {
            var databaseSizeBeforeCreate = await _locationTypeRepository.CountAsync();
            // Create the LocationType with an existing ID
            _locationType.Id = 1L;

            // An entity with an existing ID cannot be created, so this API call must fail
            LocationTypeDto _locationTypeDto = _mapper.Map<LocationTypeDto>(_locationType);
            var response = await _client.PostAsync("/api/location-types", TestUtil.ToJsonContent(_locationTypeDto));

            // Validate the LocationType in the database
            var locationTypeList = await _locationTypeRepository.GetAllAsync();
            locationTypeList.Count().Should().Be(databaseSizeBeforeCreate);
        }

        [Fact]
        public async Task CheckNameIsRequired()
        {
            var databaseSizeBeforeTest = await _locationTypeRepository.CountAsync();

            // Set the field to null
            _locationType.Name = null;

            // Create the LocationType, which fails.
            LocationTypeDto _locationTypeDto = _mapper.Map<LocationTypeDto>(_locationType);
            var response = await _client.PostAsync("/api/location-types", TestUtil.ToJsonContent(_locationTypeDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var locationTypeList = await _locationTypeRepository.GetAllAsync();
            locationTypeList.Count().Should().Be(databaseSizeBeforeTest);
        }

        [Fact]
        public async Task GetAllLocationTypes()
        {
            // Initialize the database
            await _locationTypeRepository.CreateOrUpdateAsync(_locationType);
            await _locationTypeRepository.SaveChangesAsync();

            // Get all the locationTypeList
            var response = await _client.GetAsync("/api/location-types?sort=id,desc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.[*].id").Should().Contain(_locationType.Id);
            json.SelectTokens("$.[*].name").Should().Contain(DefaultName);
        }

        [Fact]
        public async Task GetLocationType()
        {
            // Initialize the database
            await _locationTypeRepository.CreateOrUpdateAsync(_locationType);
            await _locationTypeRepository.SaveChangesAsync();

            // Get the locationType
            var response = await _client.GetAsync($"/api/location-types/{_locationType.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.id").Should().Contain(_locationType.Id);
            json.SelectTokens("$.name").Should().Contain(DefaultName);
        }

        [Fact]
        public async Task GetNonExistingLocationType()
        {
            var maxValue = long.MaxValue;
            var response = await _client.GetAsync("/api/location-types/" + maxValue);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateLocationType()
        {
            // Initialize the database
            await _locationTypeRepository.CreateOrUpdateAsync(_locationType);
            await _locationTypeRepository.SaveChangesAsync();
            var databaseSizeBeforeUpdate = await _locationTypeRepository.CountAsync();

            // Update the locationType
            var updatedLocationType = await _locationTypeRepository.QueryHelper().GetOneAsync(it => it.Id == _locationType.Id);
            // Disconnect from session so that the updates on updatedLocationType are not directly saved in db
            //TODO detach
            updatedLocationType.Name = UpdatedName;

            LocationTypeDto updatedLocationTypeDto = _mapper.Map<LocationTypeDto>(updatedLocationType);
            var response = await _client.PutAsync($"/api/location-types/{_locationType.Id}", TestUtil.ToJsonContent(updatedLocationTypeDto));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the LocationType in the database
            var locationTypeList = await _locationTypeRepository.GetAllAsync();
            locationTypeList.Count().Should().Be(databaseSizeBeforeUpdate);
            var testLocationType = locationTypeList.Last();
            testLocationType.Name.Should().Be(UpdatedName);
        }

        [Fact]
        public async Task UpdateNonExistingLocationType()
        {
            var databaseSizeBeforeUpdate = await _locationTypeRepository.CountAsync();

            // If the entity doesn't have an ID, it will throw BadRequestAlertException
            LocationTypeDto _locationTypeDto = _mapper.Map<LocationTypeDto>(_locationType);
            var response = await _client.PutAsync("/api/location-types/1", TestUtil.ToJsonContent(_locationTypeDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the LocationType in the database
            var locationTypeList = await _locationTypeRepository.GetAllAsync();
            locationTypeList.Count().Should().Be(databaseSizeBeforeUpdate);
        }

        [Fact]
        public async Task DeleteLocationType()
        {
            // Initialize the database
            await _locationTypeRepository.CreateOrUpdateAsync(_locationType);
            await _locationTypeRepository.SaveChangesAsync();
            var databaseSizeBeforeDelete = await _locationTypeRepository.CountAsync();

            var response = await _client.DeleteAsync($"/api/location-types/{_locationType.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Validate the database is empty
            var locationTypeList = await _locationTypeRepository.GetAllAsync();
            locationTypeList.Count().Should().Be(databaseSizeBeforeDelete - 1);
        }

        [Fact]
        public void EqualsVerifier()
        {
            TestUtil.EqualsVerifier(typeof(LocationType));
            var locationType1 = new LocationType
            {
                Id = 1L
            };
            var locationType2 = new LocationType
            {
                Id = locationType1.Id
            };
            locationType1.Should().Be(locationType2);
            locationType2.Id = 2L;
            locationType1.Should().NotBe(locationType2);
            locationType1.Id = 0;
            locationType1.Should().NotBe(locationType2);
        }
    }
}
