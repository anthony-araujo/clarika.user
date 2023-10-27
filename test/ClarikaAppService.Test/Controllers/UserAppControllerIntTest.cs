using System;

using AutoMapper;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
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
    public class UserAppsControllerIntTest
    {
        public UserAppsControllerIntTest()
        {
            _factory = new AppWebApplicationFactory<TestStartup>().WithMockUser();
            _client = _factory.CreateClient();

            _userAppRepository = _factory.GetRequiredService<IUserAppRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            _mapper = config.CreateMapper();

            InitTest();
        }

        private const string DefaultFirstName = "AAAAAAAAAA";
        private const string UpdatedFirstName = "BBBBBBBBBB";

        private const string DefaultLastName = "AAAAAAAAAA";
        private const string UpdatedLastName = "BBBBBBBBBB";

        private const string DefaultEmail = "AAAAAAAAAA";
        private const string UpdatedEmail = "BBBBBBBBBB";

        private static readonly DateTime? DefaultDateBirth = DateTime.UnixEpoch;
        private static readonly DateTime? UpdatedDateBirth = DateTime.UtcNow;

        private static readonly int? DefaultAge = 1;
        private static readonly int? UpdatedAge = 2;

        private const string DefaultPasswordHash = "AAAAAAAAAA";
        private const string UpdatedPasswordHash = "BBBBBBBBBB";

        private const string DefaultSecurityStamp = "AAAAAAAAAA";
        private const string UpdatedSecurityStamp = "BBBBBBBBBB";

        private const string DefaultConcurrencyStamp = "AAAAAAAAAA";
        private const string UpdatedConcurrencyStamp = "BBBBBBBBBB";

        private readonly AppWebApplicationFactory<TestStartup> _factory;
        private readonly HttpClient _client;
        private readonly IUserAppRepository _userAppRepository;

        private UserApp _userApp;

        private readonly IMapper _mapper;

        private UserApp CreateEntity()
        {
            return new UserApp
            {
                FirstName = DefaultFirstName,
                LastName = DefaultLastName,
                Email = DefaultEmail,
                DateBirth = DefaultDateBirth,
                Age = DefaultAge,
                PasswordHash = DefaultPasswordHash,
                SecurityStamp = DefaultSecurityStamp,
                ConcurrencyStamp = DefaultConcurrencyStamp,
            };
        }

        private void InitTest()
        {
            _userApp = CreateEntity();
        }

        [Fact]
        public async Task CreateUserApp()
        {
            var databaseSizeBeforeCreate = await _userAppRepository.CountAsync();

            // Create the UserApp
            UserAppDto _userAppDto = _mapper.Map<UserAppDto>(_userApp);
            var response = await _client.PostAsync("/api/user-apps", TestUtil.ToJsonContent(_userAppDto));
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Validate the UserApp in the database
            var userAppList = await _userAppRepository.GetAllAsync();
            userAppList.Count().Should().Be(databaseSizeBeforeCreate + 1);
            var testUserApp = userAppList.Last();
            testUserApp.FirstName.Should().Be(DefaultFirstName);
            testUserApp.LastName.Should().Be(DefaultLastName);
            testUserApp.Email.Should().Be(DefaultEmail);
            testUserApp.DateBirth.Should().Be(DefaultDateBirth);
            testUserApp.Age.Should().Be(DefaultAge);
            testUserApp.PasswordHash.Should().Be(DefaultPasswordHash);
            testUserApp.SecurityStamp.Should().Be(DefaultSecurityStamp);
            testUserApp.ConcurrencyStamp.Should().Be(DefaultConcurrencyStamp);
        }

        [Fact]
        public async Task CreateUserAppWithExistingId()
        {
            var databaseSizeBeforeCreate = await _userAppRepository.CountAsync();
            // Create the UserApp with an existing ID
            _userApp.Id = 1L;

            // An entity with an existing ID cannot be created, so this API call must fail
            UserAppDto _userAppDto = _mapper.Map<UserAppDto>(_userApp);
            var response = await _client.PostAsync("/api/user-apps", TestUtil.ToJsonContent(_userAppDto));

            // Validate the UserApp in the database
            var userAppList = await _userAppRepository.GetAllAsync();
            userAppList.Count().Should().Be(databaseSizeBeforeCreate);
        }

        [Fact]
        public async Task CheckFirstNameIsRequired()
        {
            var databaseSizeBeforeTest = await _userAppRepository.CountAsync();

            // Set the field to null
            _userApp.FirstName = null;

            // Create the UserApp, which fails.
            UserAppDto _userAppDto = _mapper.Map<UserAppDto>(_userApp);
            var response = await _client.PostAsync("/api/user-apps", TestUtil.ToJsonContent(_userAppDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var userAppList = await _userAppRepository.GetAllAsync();
            userAppList.Count().Should().Be(databaseSizeBeforeTest);
        }

        [Fact]
        public async Task CheckLastNameIsRequired()
        {
            var databaseSizeBeforeTest = await _userAppRepository.CountAsync();

            // Set the field to null
            _userApp.LastName = null;

            // Create the UserApp, which fails.
            UserAppDto _userAppDto = _mapper.Map<UserAppDto>(_userApp);
            var response = await _client.PostAsync("/api/user-apps", TestUtil.ToJsonContent(_userAppDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var userAppList = await _userAppRepository.GetAllAsync();
            userAppList.Count().Should().Be(databaseSizeBeforeTest);
        }

        [Fact]
        public async Task GetAllUserApps()
        {
            // Initialize the database
            await _userAppRepository.CreateOrUpdateAsync(_userApp);
            await _userAppRepository.SaveChangesAsync();

            // Get all the userAppList
            var response = await _client.GetAsync("/api/user-apps?sort=id,desc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.[*].id").Should().Contain(_userApp.Id);
            json.SelectTokens("$.[*].firstName").Should().Contain(DefaultFirstName);
            json.SelectTokens("$.[*].lastName").Should().Contain(DefaultLastName);
            json.SelectTokens("$.[*].email").Should().Contain(DefaultEmail);
            json.SelectTokens("$.[*].dateBirth").Should().Contain(DefaultDateBirth);
            json.SelectTokens("$.[*].age").Should().Contain(DefaultAge);
            json.SelectTokens("$.[*].passwordHash").Should().Contain(DefaultPasswordHash);
            json.SelectTokens("$.[*].securityStamp").Should().Contain(DefaultSecurityStamp);
            json.SelectTokens("$.[*].concurrencyStamp").Should().Contain(DefaultConcurrencyStamp);
        }

        [Fact]
        public async Task GetUserApp()
        {
            // Initialize the database
            await _userAppRepository.CreateOrUpdateAsync(_userApp);
            await _userAppRepository.SaveChangesAsync();

            // Get the userApp
            var response = await _client.GetAsync($"/api/user-apps/{_userApp.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.id").Should().Contain(_userApp.Id);
            json.SelectTokens("$.firstName").Should().Contain(DefaultFirstName);
            json.SelectTokens("$.lastName").Should().Contain(DefaultLastName);
            json.SelectTokens("$.email").Should().Contain(DefaultEmail);
            json.SelectTokens("$.dateBirth").Should().Contain(DefaultDateBirth);
            json.SelectTokens("$.age").Should().Contain(DefaultAge);
            json.SelectTokens("$.passwordHash").Should().Contain(DefaultPasswordHash);
            json.SelectTokens("$.securityStamp").Should().Contain(DefaultSecurityStamp);
            json.SelectTokens("$.concurrencyStamp").Should().Contain(DefaultConcurrencyStamp);
        }

        [Fact]
        public async Task GetNonExistingUserApp()
        {
            var maxValue = long.MaxValue;
            var response = await _client.GetAsync("/api/user-apps/" + maxValue);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateUserApp()
        {
            // Initialize the database
            await _userAppRepository.CreateOrUpdateAsync(_userApp);
            await _userAppRepository.SaveChangesAsync();
            var databaseSizeBeforeUpdate = await _userAppRepository.CountAsync();

            // Update the userApp
            var updatedUserApp = await _userAppRepository.QueryHelper().GetOneAsync(it => it.Id == _userApp.Id);
            // Disconnect from session so that the updates on updatedUserApp are not directly saved in db
            //TODO detach
            updatedUserApp.FirstName = UpdatedFirstName;
            updatedUserApp.LastName = UpdatedLastName;
            updatedUserApp.Email = UpdatedEmail;
            updatedUserApp.DateBirth = UpdatedDateBirth;
            updatedUserApp.Age = UpdatedAge;
            updatedUserApp.PasswordHash = UpdatedPasswordHash;
            updatedUserApp.SecurityStamp = UpdatedSecurityStamp;
            updatedUserApp.ConcurrencyStamp = UpdatedConcurrencyStamp;

            UserAppDto updatedUserAppDto = _mapper.Map<UserAppDto>(updatedUserApp);
            var response = await _client.PutAsync($"/api/user-apps/{_userApp.Id}", TestUtil.ToJsonContent(updatedUserAppDto));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the UserApp in the database
            var userAppList = await _userAppRepository.GetAllAsync();
            userAppList.Count().Should().Be(databaseSizeBeforeUpdate);
            var testUserApp = userAppList.Last();
            testUserApp.FirstName.Should().Be(UpdatedFirstName);
            testUserApp.LastName.Should().Be(UpdatedLastName);
            testUserApp.Email.Should().Be(UpdatedEmail);
            testUserApp.DateBirth.Should().Be(UpdatedDateBirth);
            testUserApp.Age.Should().Be(UpdatedAge);
            testUserApp.PasswordHash.Should().Be(UpdatedPasswordHash);
            testUserApp.SecurityStamp.Should().Be(UpdatedSecurityStamp);
            testUserApp.ConcurrencyStamp.Should().Be(UpdatedConcurrencyStamp);
        }

        [Fact]
        public async Task UpdateNonExistingUserApp()
        {
            var databaseSizeBeforeUpdate = await _userAppRepository.CountAsync();

            // If the entity doesn't have an ID, it will throw BadRequestAlertException
            UserAppDto _userAppDto = _mapper.Map<UserAppDto>(_userApp);
            var response = await _client.PutAsync("/api/user-apps/1", TestUtil.ToJsonContent(_userAppDto));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the UserApp in the database
            var userAppList = await _userAppRepository.GetAllAsync();
            userAppList.Count().Should().Be(databaseSizeBeforeUpdate);
        }

        [Fact]
        public async Task DeleteUserApp()
        {
            // Initialize the database
            await _userAppRepository.CreateOrUpdateAsync(_userApp);
            await _userAppRepository.SaveChangesAsync();
            var databaseSizeBeforeDelete = await _userAppRepository.CountAsync();

            var response = await _client.DeleteAsync($"/api/user-apps/{_userApp.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Validate the database is empty
            var userAppList = await _userAppRepository.GetAllAsync();
            userAppList.Count().Should().Be(databaseSizeBeforeDelete - 1);
        }

        [Fact]
        public void EqualsVerifier()
        {
            TestUtil.EqualsVerifier(typeof(UserApp));
            var userApp1 = new UserApp
            {
                Id = 1L
            };
            var userApp2 = new UserApp
            {
                Id = userApp1.Id
            };
            userApp1.Should().Be(userApp2);
            userApp2.Id = 2L;
            userApp1.Should().NotBe(userApp2);
            userApp1.Id = 0;
            userApp1.Should().NotBe(userApp2);
        }
    }
}
