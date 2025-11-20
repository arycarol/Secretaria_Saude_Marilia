using System.Text.Json.Serialization;

namespace CareMove.Argument.Argument;

public class InputCreateTransportAssignment : BaseInputCreate<InputCreateTransportAssignment>
{
    public long? VehicleId { get; private set; }
    public DateOnly Date { get; private set; }
    public long DriverUserId { get; private set; }
    public long PacientUserId { get; private set; }
    public long TransportRequestId { get; private set; }

    [JsonConstructor]
    public InputCreateTransportAssignment(long? vehicleId, DateOnly date, long driverUserId, long pacientUserId, long transportRequestId)
    {
        VehicleId = vehicleId;
        Date = date;
        DriverUserId = driverUserId;
        PacientUserId = pacientUserId;
        TransportRequestId = transportRequestId;
    }
}
