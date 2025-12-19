namespace AuctionService.IntegrationTests.Fixtures
{
    [CollectionDefinition("Shared Collection")]
    public class SharedFixture : ICollectionFixture<CustomWebAppFactory>
    {
        //instead of creating a new postgres image in docker for every *Tests.cs File, we share once Postgres instance for all tests.
        //Saves a lot of time and energy!!
    }
}
