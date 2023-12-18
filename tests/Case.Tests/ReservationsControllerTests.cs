using System.Globalization;
using System.Net.Http.Json;
using Case.Application;
using Case.Application.Exceptions;
using Case.Tests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Case.Tests;

public sealed class ReservationsControllerTests : IClassFixture<OptionsProvider>
{
    private readonly HttpClient _client;
    private static readonly Guid SpotId = Guid.Parse("00000000-0000-0000-0000-000000000002");
    private static readonly Guid UserId = Guid.Parse("00000000-0000-0000-0000-000000000003");
    private static readonly DateTime ReservationDate = DateTime.Now.Date;
    private static readonly CultureInfo CultureInfo = new("en-US");

    public ReservationsControllerTests(OptionsProvider optionsProvider)
    {
        var app = new TestApp(ConfigureServices);
        _client = app.Client;
    }

    [Fact]
    public async Task Post_should_reserve_empty_spot()
    {
        //arrange
        var reservationRequest = new ReserveParkingSpot.Command(Guid.Empty, Guid.Empty, UserId, "AB12345", ReservationDate);

        //act
        _ = await _client.PostAsJsonAsync($"/parking-spots/{SpotId}/reservations", reservationRequest);

        //assert
        var reservation = await GetReservation(ReservationDate, SpotId, UserId);
        reservation.ShouldNotBeNull();
    }

    [Fact]
    public async Task Put_should_update_update_license_plate()
    {
        //arrange
        var reservationRequest = new ReserveParkingSpot.Command(Guid.Empty, Guid.Empty, UserId, "AB12345", ReservationDate);
        _ = await _client.PostAsJsonAsync($"/parking-spots/{SpotId}/reservations", reservationRequest);
        var reservationId = (await GetReservation(ReservationDate, SpotId, UserId)).Id;

        var updateReservationRequest = new ChangeReservationLicencePlate.Command(reservationId, "CD67890");

        //act
        _ = await _client.PutAsJsonAsync($"/parking-spots/{SpotId}/reservations", updateReservationRequest);

        //assert
        var reservation = await GetReservation(ReservationDate, SpotId, UserId);
        reservation.LicensePlate.ShouldBe("CD67890");
    }
    
    [Fact]
    public async Task Put_should_throw_exception_when_reservation_not_found()
    {
        //arrange
        var unusedReservationId = Guid.Parse("11111111-0000-0000-0000-000000000001");
        var updateReservationRequest = new ChangeReservationLicencePlate.Command(unusedReservationId, "CD67890");

        //act & Assert
        await Assert.ThrowsAsync<WeeklyParkingSpotReservationNotFound>(() =>
            _client.PutAsJsonAsync($"/parking-spots/{SpotId}/reservations", updateReservationRequest));
    }
    
    [Fact]
    public async Task Put_should_throw_exception_when_weekly_parking_spot_is_not_found()
    {
        //arrange
        var unusedParkingSpotId = Guid.Parse("11111111-0000-0000-0000-000000000001");

        //act & Assert
        await Assert.ThrowsAsync<WeeklyParkingSpotNotFoundException>(() =>
            _client.PutAsJsonAsync($"/parking-spots/{unusedParkingSpotId}/reservations", new ChangeReservationLicencePlate.Command()));
    }

    private async Task<GetWeeklyParkingSpots.ReservationDto>? GetReservation(DateTime date, Guid spotId, Guid userId)
    {
        var spotsResponse = await _client.GetAsync($"/parking-spots?date={date.ToString(CultureInfo)}");
        var spots = await spotsResponse.Content.ReadFromJsonAsync<List<GetWeeklyParkingSpots.WeeklyParkingSpotDto>>();

        var spot = spots.FirstOrDefault(dto => dto.Id == spotId.ToString());
        return spot.Reservations.FirstOrDefault(dto => dto.UserId == userId && dto.Date == ReservationDate);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        
    }
}