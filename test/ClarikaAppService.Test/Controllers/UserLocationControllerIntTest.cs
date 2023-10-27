
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
    public class UserLocationsControllerIntTest
    {
        public UserLocationsControllerIntTest()
        {
            _factory = new AppWebApplicationFactory<TestStartup>().WithMockUser();
            _client = _factory.CreateClient();

            _userLocationRepository = _factory.GetRequiredService<IUserLocationRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            _mapper = config.CreateMapper();

            InitTest();
        }

        private const string DefaultAddress = "AAAAAAAAAA";
        private const string UpdatedAddress = "BBBBBBBBBB";

        private const string DefaultZipCode = "AAAAAAAAAA";
        private const string UpdatedZipCode = "BBBBBBBBBB";

        private const string DefaultProvince = "AAAAAAAAAA";
        private const string UpdatedProvince = "BBBBBBBBBB";

        private readonly AppWebApplicationFactory<TestStartup> _factory;
        private readonly HttpClient _client;
        private readonly IUserLocationRepository _userLocationRepository;

        private UserLocation _userLocation;

        private readonly IMapper _mapper;

        private UserLocation CreateEntity()
        {
            return new UserLocation
            {
                Address = DefaultAddress,
                ZipCode = DefaultZipCode,
                Province = DefaultProvince,
            };
        }

        private void InitTest()
        {
            _userLocation = CreateEntity();
        }

        [Fact]
        public async Task CreateUserLocation()
        {
            var databaseSizeBeforeCreate = await _userLocationRepository.CountAsync();

            // Create the UserLocation
            UserLocationDto _userLocationDto = _mapper.Map<UserLocationDto>(_userLocation);
            var response = await _client.PostAsync("/api/user-locations", TestUtil.ToJsonContent(_userLocationDto));
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Validate the UserLocation in the database
            var userLocationList = await _userLocationRepository.GetAllAsync();
            userLocationList.Count().Should().Be(databaseSizeBeforeCreate + 1);
            var testUserLocation = userLocationList.Last();
            testUserLocation.Address.Should().Be(DefaultAddress);
            testUserLocation.ZipCode.Should().Be(DefaultZipCode);
            testUserLocation.Province.Should().Be(DefaultProvince);
        }

        [Fact]
        public async Task CreateUserLocationWithExistingId()
        {
            var databaseSizeBeforeCreate = await _userLocationRepository.CountAsync();
            // Create the UserLocation with an existing ID
            _userLocation.Id = 1L;

            // An entity with an existing ID cannot be created, so this API call must fail
            UserLocationDto _userLocationDto = _mapper.Map<UserLocationDto>(_userLocation);
            var response = await _client.PostAsync("/api/user-locations", TestUtil.ToJsonContent(_userLocationDto));

            // Validate the UserLocation in the database
            var userLocationList = await _userLocationRepository.GetAllAsync();
            userLocationList.Count().Should().Be(databaseSizeBeforeCreate);
        }

        [Fact]
        public async Task CheckAddressIsRequired()
        {
            var databaseSizeBeforeTest = await _userLocationRepository.CountAsync();

            // Set the field to null
            _userLocation.Address = null;

            // Create the UserLocation, which fails.
            UserLocationDto _userLocationDto = _mapper.Map<UserLocationDto>(_userLocation);
            var response = await _client.PostAsync("/api/user-locations", TestUtil.ToJsonContent(_userLocationDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var userLocationList = await _userLocationRepository.GetAllAsync();
            userLocationList.Count().Should().Be(databaseSizeBeforeTest);
        }

        [Fact]
        public async Task GetAllUserLocations()
        {
            // Initialize the database
            await _userLocationRepository.CreateOrUpdateAsync(_userLocation);
            await _userLocationRepository.SaveChangesAsync();

            // Get all the userLocationList
            var response = await _client.GetAsync("/api/user-locations?sort=id,desc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.[*].id").Should().Contain(_userLocation.Id);
            json.SelectTokens("$.[*].address").Should().Contain(DefaultAddress);
            json.SelectTokens("$.[*].zipCode").Should().Contain(DefaultZipCode);
            json.SelectTokens("$.[*].province").Should().Contain(DefaultProvince);
        }

        [Fact]
        public async Task GetUserLocation()
        {
            // Initialize the database
            await _userLocationRepository.CreateOrUpdateAsync(_userLocation);
            await _userLocationRepository.SaveChangesAsync();

            // Get the userLocation
            var response = await _client.GetAsync($"/api/user-locations/{_userLocation.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.id").Should().Contain(_userLocation.Id);
            json.SelectTokens("$.address").Should().Contain(DefaultAddress);
            json.SelectTokens("$.zipCode").Should().Contain(DefaultZipCode);
            json.SelectTokens("$.province").Should().Contain(DefaultProvince);
        }

        [Fact]
        public async Task GetNonExistingUserLocation()
        {
            var maxValue = long.MaxValue;
            var response = await _client.GetAsync("/api/user-locations/" + maxValue);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateUserLocation()
        {
            // Initialize the database
            await _userLocationRepository.CreateOrUpdateAsync(_userLocation);
            await _userLocationRepository.SaveChangesAsync();
            var databaseSizeBeforeUpdate = await _userLocationRepository.CountAsync();

            // Update the userLocation
            var updatedUserLocation = await _userLocationRepository.QueryHelper().GetOneAsync(it => it.Id == _userLocation.Id);
            // Disconnect from session so that the updates on updatedUserLocation are not directly saved in db
            //TODO detach
            updatedUserLocation.Address = UpdatedAddress;
            updatedUserLocation.ZipCode = UpdatedZipCode;
            updatedUserLocation.Province = UpdatedProvince;

            UserLocationDto updatedUserLocationDto = _mapper.Map<UserLocationDto>(updatedUserLocation);
            var response = await _client.PutAsync($"/api/user-locations/{_userLocation.Id}", TestUtil.ToJsonContent(updatedUserLocationDto));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the UserLocation in the database
            var userLocationList = await _userLocationRepository.GetAllAsync();
            userLocationList.Count().Should().Be(databaseSizeBeforeUpdate);
            var testUserLocation = userLocationList.Last();
            testUserLocation.Address.Should().Be(UpdatedAddress);
            testUserLocation.ZipCode.Should().Be(UpdatedZipCode);
            testUserLocation.Province.Should().Be(UpdatedProvince);
        }

        [Fact]
        public async Task UpdateNonExistingUserLocation()
        {
            var databaseSizeBeforeUpdate = await _userLocationRepository.CountAsync();

            // If the entity doesn't have an ID, it will throw BadRequestAlertException
            UserLocationDto _userLocationDto = _mapper.Map<UserLocationDto>(_userLocation);
            var response = await _client.PutAsync("/api/user-locations/1", TestUtil.ToJsonContent(_userLocationDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the UserLocation in the database
            var userLocationList = await _userLocationRepository.GetAllAsync();
            userLocationList.Count().Should().Be(databaseSizeBeforeUpdate);
        }

        [Fact]
        public async Task DeleteUserLocation()
        {
            // Initialize the database
            await _userLocationRepository.CreateOrUpdateAsync(_userLocation);
            await _userLocationRepository.SaveChangesAsync();
            var databaseSizeBeforeDelete = await _userLocationRepository.CountAsync();

            var response = await _client.DeleteAsync($"/api/user-locations/{_userLocation.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Validate the database is empty
            var userLocationList = await _userLocationRepository.GetAllAsync();
            userLocationList.Count().Should().Be(databaseSizeBeforeDelete - 1);
        }

        [Fact]
        public void EqualsVerifier()
        {
            TestUtil.EqualsVerifier(typeof(UserLocation));
            var userLocation1 = new UserLocation
            {
                Id = 1L
            };
            var userLocation2 = new UserLocation
            {
                Id = userLocation1.Id
            };
            userLocation1.Should().Be(userLocation2);
            userLocation2.Id = 2L;
            userLocation1.Should().NotBe(userLocation2);
            userLocation1.Id = 0;
            userLocation1.Should().NotBe(userLocation2);
        }
    }
}
