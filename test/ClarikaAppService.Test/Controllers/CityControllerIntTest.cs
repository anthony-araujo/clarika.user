
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
    public class CitiesControllerIntTest
    {
        public CitiesControllerIntTest()
        {
            _factory = new AppWebApplicationFactory<TestStartup>().WithMockUser();
            _client = _factory.CreateClient();

            _cityRepository = _factory.GetRequiredService<ICityRepository>();

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
        private readonly ICityRepository _cityRepository;

        private City _city;

        private readonly IMapper _mapper;

        private City CreateEntity()
        {
            return new City
            {
                Name = DefaultName,
            };
        }

        private void InitTest()
        {
            _city = CreateEntity();
        }

        //[Fact]
        //public async Task CreateCity()
        //{
        //    var databaseSizeBeforeCreate = await _cityRepository.CountAsync();

        //    // Create the City
        //    CityDto _cityDto = _mapper.Map<CityDto>(_city);
        //    var response = await _client.PostAsync("/api/cities", TestUtil.ToJsonContent(_cityDto));
        //    response.StatusCode.Should().Be(HttpStatusCode.Created);

        //    // Validate the City in the database
        //    var cityList = await _cityRepository.GetAllAsync();
        //    cityList.Count().Should().Be(databaseSizeBeforeCreate + 1);
        //    var testCity = cityList.Last();
        //    testCity.Name.Should().Be(DefaultName);
        //}

        [Fact]
        public async Task CreateCityWithExistingId()
        {
            var databaseSizeBeforeCreate = await _cityRepository.CountAsync();
            // Create the City with an existing ID
            _city.Id = 1L;

            // An entity with an existing ID cannot be created, so this API call must fail
            CityDto _cityDto = _mapper.Map<CityDto>(_city);
            var response = await _client.PostAsync("/api/cities", TestUtil.ToJsonContent(_cityDto));

            // Validate the City in the database
            var cityList = await _cityRepository.GetAllAsync();
            cityList.Count().Should().Be(databaseSizeBeforeCreate);
        }

        [Fact]
        public async Task CheckNameIsRequired()
        {
            var databaseSizeBeforeTest = await _cityRepository.CountAsync();

            // Set the field to null
            _city.Name = null;

            // Create the City, which fails.
            CityDto _cityDto = _mapper.Map<CityDto>(_city);
            var response = await _client.PostAsync("/api/cities", TestUtil.ToJsonContent(_cityDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var cityList = await _cityRepository.GetAllAsync();
            cityList.Count().Should().Be(databaseSizeBeforeTest);
        }

        //[Fact]
        //public async Task GetAllCities()
        //{
        //    // Initialize the database
        //    await _cityRepository.CreateOrUpdateAsync(_city);
        //    await _cityRepository.SaveChangesAsync();

        //    // Get all the cityList
        //    var response = await _client.GetAsync("/api/cities?sort=id,desc");
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);

        //    var json = JToken.Parse(await response.Content.ReadAsStringAsync());
        //    json.SelectTokens("$.[*].id").Should().Contain(_city.Id);
        //    json.SelectTokens("$.[*].name").Should().Contain(DefaultName);
        //}

        //[Fact]
        //public async Task GetCity()
        //{
        //    // Initialize the database
        //    await _cityRepository.CreateOrUpdateAsync(_city);
        //    await _cityRepository.SaveChangesAsync();

        //    // Get the city
        //    var response = await _client.GetAsync($"/api/cities/{_city.Id}");
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);

        //    var json = JToken.Parse(await response.Content.ReadAsStringAsync());
        //    json.SelectTokens("$.id").Should().Contain(_city.Id);
        //    json.SelectTokens("$.name").Should().Contain(DefaultName);
        //}

        //[Fact]
        //public async Task GetNonExistingCity()
        //{
        //    var maxValue = long.MaxValue;
        //    var response = await _client.GetAsync("/api/cities/" + maxValue);
        //    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        //}

        //[Fact]
        //public async Task UpdateCity()
        //{
        //    // Initialize the database
        //    await _cityRepository.CreateOrUpdateAsync(_city);
        //    await _cityRepository.SaveChangesAsync();
        //    var databaseSizeBeforeUpdate = await _cityRepository.CountAsync();

        //    // Update the city
        //    var updatedCity = await _cityRepository.QueryHelper().GetOneAsync(it => it.Id == _city.Id);
        //    // Disconnect from session so that the updates on updatedCity are not directly saved in db
        //    //TODO detach
        //    updatedCity.Name = UpdatedName;

        //    CityDto updatedCityDto = _mapper.Map<CityDto>(updatedCity);
        //    var response = await _client.PutAsync($"/api/cities/{_city.Id}", TestUtil.ToJsonContent(updatedCityDto));
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);

        //    // Validate the City in the database
        //    var cityList = await _cityRepository.GetAllAsync();
        //    cityList.Count().Should().Be(databaseSizeBeforeUpdate);
        //    var testCity = cityList.Last();
        //    testCity.Name.Should().Be(UpdatedName);
        //}

        [Fact]
        public async Task UpdateNonExistingCity()
        {
            var databaseSizeBeforeUpdate = await _cityRepository.CountAsync();

            // If the entity doesn't have an ID, it will throw BadRequestAlertException
            CityDto _cityDto = _mapper.Map<CityDto>(_city);
            var response = await _client.PutAsync("/api/cities/1", TestUtil.ToJsonContent(_cityDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the City in the database
            var cityList = await _cityRepository.GetAllAsync();
            cityList.Count().Should().Be(databaseSizeBeforeUpdate);
        }

        //[Fact]
        //public async Task DeleteCity()
        //{
        //    // Initialize the database
        //    await _cityRepository.CreateOrUpdateAsync(_city);
        //    await _cityRepository.SaveChangesAsync();
        //    var databaseSizeBeforeDelete = await _cityRepository.CountAsync();

        //    var response = await _client.DeleteAsync($"/api/cities/{_city.Id}");
        //    response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        //    // Validate the database is empty
        //    var cityList = await _cityRepository.GetAllAsync();
        //    cityList.Count().Should().Be(databaseSizeBeforeDelete - 1);
        //}

        [Fact]
        public void EqualsVerifier()
        {
            TestUtil.EqualsVerifier(typeof(City));
            var city1 = new City
            {
                Id = 1L
            };
            var city2 = new City
            {
                Id = city1.Id
            };
            city1.Should().Be(city2);
            city2.Id = 2L;
            city1.Should().NotBe(city2);
            city1.Id = 0;
            city1.Should().NotBe(city2);
        }
    }
}
