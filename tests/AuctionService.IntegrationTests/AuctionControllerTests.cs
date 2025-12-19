
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace AuctionService.IntegrationTests
{
    [Collection("Shared Collection")]
    public class AuctionControllerTests : IAsyncLifetime
    {
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;
        private const string BV_ID = "c8c3ec17-01bf-49db-82aa-1ef80b833a9f";
        public AuctionControllerTests(CustomWebAppFactory factory)
        {
            this._factory = factory;
            this._httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAuctions_ShouldReturn3Auctions()
        {
            //arrange
            //most setup up and running 
            //act
            var response = await _httpClient.GetFromJsonAsync<List<AuctionDto>>("api/auctions");
            //assert
            Assert.Equal(3, response.Count);
        }

        [Fact]
        public async Task GetAuctionById_WithValidId_ShouldReturnAuction()
        {
            //arrange
            //most setup up and running 
            //act
            var response = await _httpClient.GetFromJsonAsync<AuctionDto>($"api/auctions/{BV_ID}");
            //assert
            Assert.Equal("Veyron", response.Model);
        }

        [Fact]
        public async Task GetAuctionById_WithInvalidId_ShouldReturn404()
        {
            //arrange
            //most setup up and running 
            //act
            var response = await _httpClient.GetAsync($"api/auctions/{Guid.NewGuid()}");
            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAuctionById_WithInvalidGuid_ShouldReturn400()
        {
            //arrange
            //most setup up and running 
            //act
            var response = await _httpClient.GetAsync($"api/auctions/notaguid");
            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateAuction_WithNoAuth_ShouldReturn401()
        {
            //arrange
            var auction = new CreateAuctionDto { Make = "test" };
            //act
            var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);
            //assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreateAuction_WithAuthValidAuction_ShouldReturn201()
        {
            //arrange
            var auction = GetAuctionForCreate();
            var bearer = _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));
            //act
            var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);
            //assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var created = await response.Content.ReadFromJsonAsync<AuctionDto>();
            Assert.Equal("bob", created.Seller);
        }

        [Fact]
        public async Task CreateAuction_WithInvalidCreateAuctionDto_ShouldReturn400()
        {
            // arrange
            var auction = new CreateAuctionDto { Make="Pepega", Model="Spongy" }; //creating half-assed auciton
            var bearer = _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));
            // act
            var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);
            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAuction_WithValidUpdateDtoAndUser_ShouldReturn200()
        {
            // arrange
            var auction = new UpdateAuctionDto { Mileage = 9 };
            var bearer = _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("alice"));
            // act
            var response = await _httpClient.PutAsJsonAsync($"api/auctions/{BV_ID}", auction);
            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAuction_WithValidUpdateDtoAndInvalidUser_ShouldReturn403()
        {
            // arrange
            var auction = new UpdateAuctionDto { Mileage = 9 };
            var bearer = _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));
            // act
            var response = await _httpClient.PutAsJsonAsync($"api/auctions/{BV_ID}", auction);
            // assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAuction_WithValidUserAndID_ShouldReturn200()
        {
            // arrange
            var bearer = _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("alice"));
            // act
            var response = await _httpClient.DeleteAsync($"api/auctions/{BV_ID}");
            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAuction_WithInalidUserAndValidID_ShouldReturn403()
        {
            // arrange
            var bearer = _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));
            // act
            var response = await _httpClient.DeleteAsync($"api/auctions/{BV_ID}");
            // assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAuction_WithValidUserAndInvalidID_ShouldReturn404()
        {
            // arrange
            var bearer = _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("alice"));
            // act
            var response = await _httpClient.DeleteAsync($"api/auctions/{Guid.NewGuid()}");
            // assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
