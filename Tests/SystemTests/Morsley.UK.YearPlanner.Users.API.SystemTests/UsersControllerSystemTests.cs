using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Response;
using Morsley.UK.YearPlanner.Users.Persistence.Contexts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Morsley.UK.YearPlanner.Users.API.SystemTests
{
    public class UsersControllerSystemTests : IClassFixture<TestWebApplicationFactory<StartUp>>
    {
        private const string RequestUri = "api/v1/users";

        private readonly WebApplicationFactory<StartUp> _factory;
        private readonly IFixture _fixture;
        
        public UsersControllerSystemTests(TestWebApplicationFactory<StartUp> factory)
        {
            _factory = factory;
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GET_When_There_Are_No_Users_Should_Return_No_Users()
        {
            // Arrange...
            HttpClient client = _factory.CreateClient();

            // Act...
            HttpResponseMessage response = await client.GetAsync(RequestUri);

            // Assert...
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Dispose...
            client.Dispose();
        }

        [Fact]
        public async Task GET_When_There_Are_Users_Should_Return_Those_Users()
        {
            // Arrange...
            const int numberOfUsersToAdd = 5;
            AddUsersToDatabase(numberOfUsersToAdd);
            HttpClient client = _factory.CreateClient();

            // Act...
            HttpResponseMessage response = await client.GetAsync(RequestUri);

            // Assert...
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var users = await GetUserResponses(response);
            users.Count().Should().Be(numberOfUsersToAdd);

            // Dispose...
            client.Dispose();
        }


        #region Helper Methods

        private void AddUsersToDatabase(int quantity)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            for (var _ = 0; _ < quantity; _++)
            {
                var user = _fixture.Create<Domain.Models.User>();
                context.Users.Add(user);
            }
            context.SaveChanges();
        }

        private T DeserializeObjectFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        private async Task<string> ExtractJsonFromResponse(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<IEnumerable<UserResponse>> GetUserResponses(HttpResponseMessage response)
        {
            var json = await ExtractJsonFromResponse(response);
            return DeserializeObjectFromJson<IEnumerable<UserResponse>>(json);
        }


        #endregion Helper Methods
    }
}