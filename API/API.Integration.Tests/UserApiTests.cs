using API.Dtos;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System;

namespace API.Integration.Tests
{
    public class Tests
    {
        private HttpClient _httpClient;
        private ConcurrentDictionary<string, object> _itemsToDelete;
        private string RESOURCE_PATH = "/api/users/";

        [OneTimeSetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri("https://localhost:5001");

            _itemsToDelete = new ConcurrentDictionary<string, object>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            var deleteTasks = _itemsToDelete.Values.ToList().Select(async x =>
            {
                var response = await Delete(x as UserDto);
                if (response.StatusCode != HttpStatusCode.Accepted)
                {
                    throw new Exception($"Delete operation failed with HTTP Status Code: {response.StatusCode}");
                }
            }).AsEnumerable();

            Task.WaitAll(deleteTasks.Cast<Task>().ToArray());

            _httpClient.Dispose();
        }

        [Test]
        public async Task GetByEmail()
        {
            var email = "x@gmail.com";
            IEnumerable<FormattedUserDto> users = await GetUsers(email, true);

            users.ToList().ForEach(x => x.EmailAddress.Should().Be(email));
        }

        [Test]
        public async Task Post()
        {
            //setup
            var guidStr = Guid.NewGuid();
            var obj = new UserDto
            {
                FristName = "x",
                MiddleName = "x",
                LastName = "x",
                EmailAddress = $"{guidStr}@gmail.com",
                PhoneNumber = "112-233-1923"
            };

            //user doesn't exist with given email address
            var users = await GetUsers(obj.EmailAddress);
            users.Should().BeNull($"No users can exists with email: {obj.EmailAddress} for this to be a valid test");

            var response = await CreateObj(obj);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [TestCase("")]
        [TestCase("fdasfasfdasfd")]
        [TestCase(".@")]
        public async Task Post_Invalid_Email(string invalidEmail)
        {
            //setup
            var guidStr = Guid.NewGuid();
            var obj = new UserDto
            {
                FristName = "x",
                MiddleName = "x",
                LastName = "x",
                EmailAddress = invalidEmail,
                PhoneNumber = "112-233-1923"
            };

            var response = await CreateObj(obj);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Delete()
        {
            //setup
            var guidStr = Guid.NewGuid();
            var obj = new UserDto
            {
                FristName = "x",
                MiddleName = "x",
                LastName = "x",
                EmailAddress = $"{guidStr}@gmail.com",
                PhoneNumber = "112-233-1923"
            };

            //user doesn't exist with given email address
            var users = await GetUsers(obj.EmailAddress);
            users.Should().BeNull($"No users can exists with email: {obj.EmailAddress} for this to be a valid test");

            var response = await CreateObj(obj);

            response.StatusCode.Should().Be(HttpStatusCode.Created, $"Create operation failed with HTTP status code: {response.StatusCode}");

            var deleteResponse = await Delete(obj);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.Accepted);
            //manually remove delete item from dictionary to prevent cleanup removing it
            _itemsToDelete.Remove(obj.EmailAddress, out _);
        }

        private async Task<IEnumerable<FormattedUserDto>> GetUsers(string email, bool doAssertion = false)
        {
            var response = await _httpClient.GetAsync($"{RESOURCE_PATH}{email}");

            //only want to assert on test that are only for testing get
            if (doAssertion)
            {
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<FormattedUserDto>>(responseContent);
            return users;
        }

        private async Task<HttpResponseMessage> CreateObj<T>(T obj)
            where T: IEmailable
        {
            var objJson = JsonConvert.SerializeObject(obj);
            var content = new StringContent(objJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(RESOURCE_PATH, content);

            if(response.StatusCode == HttpStatusCode.Created)
            {
                _itemsToDelete.TryAdd(obj.EmailAddress, obj);
            }

            return response;
        }

        private async Task<HttpResponseMessage> Delete<T>(T obj)
            where T: IEmailable
        {
            var response = await _httpClient.DeleteAsync($"{RESOURCE_PATH}{obj.EmailAddress}");
            return response;
        }
    }
}