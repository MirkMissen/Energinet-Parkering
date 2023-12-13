# Case: Reservering af parkeringspladser

## Introduktion
I vedhæftede kildekode er implementeret et system til reservering af virksomhedens parkeringspladser. Udviklingen sker efter agile metoder, og derfor er kun den basale funktionalitet endnu på plads.
### Forretningsregler
Alle virksomhedens ansatte kan reservere en parkeringsplads. En plads kan kun reserveres i indeværende uge, så det er altså ikke muligt at reservere pladser i kommende uger før tidligst mandag i den uge.
### Teknologi
Systemet indeholder ikke brugerflade, men udstiller et RESTful API over https.
Foreløbig er følgende endpoints implementeret:
`GET /parking-spots` returnerer alle parkeringspladser og deres eventuelle reserveringer for en given uge.
`POST /parking-spots/{parkingSpotId}/reservations` behandler reservationsønsker indeholdende:
```
parkingSpotId:	string($uuid)
userId:			string($uuid)
licensePlate:	string
date:			string($date-time)
```
Ovenstående endpoints kan motioneres run-time via [Swagger UI](https://localhost:7265/swagger/index.html).
Derudover er der i `Case.Tests` projektet implementeret tests der viser hvordan endpoints kan tilgåes i kode.
## Funktionalitet
Den næste funktionalitet der ønkes udviklet er muligheden for at kunne ændre nummerplade i en allerede oprettet reservation. Det eksisterende API ønskes udvidet med følgende endpoint:
`PUT /parking-spots/{parkingSpotId}/reservations` der behandler et ændringsønske indeholdende:
```
reservationId:	string($uuid)
licensePlate:	string
```
Følgende test beskriver den ønskede udvidelse til det eksisterende API:
```cs
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
```
## Hovedopgave
***Implementér*** det ønskede endpoint til ændring af eksisterende reservationer under hensyntagen til den eksisterende arkitektur.
Derudover må du gerne, hvis tiden tillader, ***overveje*** muligheder for at identificere brugeren og dennes rettigheder inden en reservation oprettes eller ændres.

