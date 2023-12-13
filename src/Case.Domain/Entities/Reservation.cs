using Case.Domain.ValueObjects;

namespace Case.Domain.Entities;

public sealed record Reservation(ReservationId Id, UserId UserId, EmployeeName EmployeeName, LicensePlate LicensePlate, Date Date);