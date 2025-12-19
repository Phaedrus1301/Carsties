using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Utils;
using Contracts;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace AuctionService.IntegrationTests
{
    [Collection("Shared Collection")]
    public class AuctionBusTests : IAsyncLifetime
    {
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;
        private readonly ITestHarness _testHarness;
        private const string BV_ID = "c8c3ec17-01bf-49db-82aa-1ef80b833a9f";

        public AuctionBusTests(CustomWebAppFactory factory)
        {
            this._factory = factory;
            this._httpClient = factory.CreateClient();
            this._testHarness = factory.Services.GetTestHarness();
        }

        [Fact]
        public async Task CreateAuction_WithValidObject_ShouldPublishAuctionCreated()
        {
            //arrange
            var auction = GetAuctionForCreate();
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));
            //act
            var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);
            //assert
            response.EnsureSuccessStatusCode();
            Assert.True(await _testHarness.Published.Any<AuctionCreated>());
        }

        [Fact]
        public async Task UpdateAuction_WithValidObject_ShouldPublishAuctionUpdated()
        {
            //arrange
            var auction = new UpdateAuctionDto { Mileage=9 };
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("alice"));
            //act
            var response = await _httpClient.PutAsJsonAsync($"api/auctions/{BV_ID}", auction);
            //assert
            response.EnsureSuccessStatusCode();
            Assert.True(await _testHarness.Published.Any<AuctionUpdated>());
        }

        [Fact]
        public async Task DeleteAuction_WithValidObject_ShouldPublishAuctionDeleted()
        {
            //arrange
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("alice"));
            //act
            var response = await _httpClient.DeleteAsync($"api/auctions/{BV_ID}");
            //assert
            response.EnsureSuccessStatusCode();
            Assert.True(await _testHarness.Published.Any<AuctionDeleted>());
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
            DbHelper.ReinitDbForTests(db);
            return Task.CompletedTask;
        }

        private static CreateAuctionDto GetAuctionForCreate()
        {
            return new CreateAuctionDto
            {
                Make = "test",
                Model = "ModelTest",
                ImageUrl = "test",
                Color = "blue",
                Mileage = 21,
                Year = 10,
                ReservePrice = 220
            };
        }

    }
}
