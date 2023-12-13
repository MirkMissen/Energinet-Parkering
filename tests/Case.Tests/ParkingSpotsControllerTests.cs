using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using Case.Application;
using Case.Tests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Case.Tests;

public sealed class ParkingSpotsControllerTests : IClassFixture<OptionsProvider>
{
    private readonly HttpClient _client;
    private static readonly CultureInfo CultureInfo = new("en-US");

    public ParkingSpotsControllerTests(OptionsProvider optionsProvider)
    {
        var app = new TestApp(ConfigureServices);
        _client = app.Client;
    }

    [Fact]
    public async Task Get_should_return_200()
    {
        var response = await _client.GetAsync("/parking-spots");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_should_return_parking_spots()
    {
        var response = await _client.GetAsync($"/parking-spots?date={DateTime.Now.ToString(CultureInfo)}");
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<GetWeeklyParkingSpots.WeeklyParkingSpotDto>>();

        content.ShouldNotBeNull();
        content.ShouldNotBeEmpty();
    }
    private void ConfigureServices(IServiceCollection services)
    {
    }
}