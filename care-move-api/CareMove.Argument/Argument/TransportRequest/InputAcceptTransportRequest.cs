namespace CareMove.Argument.Argument;

public class InputAcceptTransportRequest
{
    public long Id { get;set; }
    public long? VehicleId { get;set; }
    public long DriverUserId { get;set; }

    public InputAcceptTransportRequest(long id, long? vehicleId, long driverUserId)
    {
        Id = id;
        VehicleId = vehicleId;
        DriverUserId = driverUserId;
    }
}
