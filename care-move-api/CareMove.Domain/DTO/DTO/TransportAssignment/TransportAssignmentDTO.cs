using CareMove.Argument.Argument;
using CareMove.Domain.DTO.Base;

namespace CareMove.Domain.DTO.DTO;

public class TransportAssignmentDTO : BaseDTO<TransportAssignmentDTO, OutputTransportAssignment>
{
    public long VehicleId { get; private set; }
    public DateOnly Date { get; private set; }
    public long DriverUserId { get; private set; }
    public long PacientUserId { get; private set; }
    public long TransportRequestId { get; private set; }
    public string TransportAssignmentStatus { get; private set; }

    public TransportAssignmentDTO() { }

    public TransportAssignmentDTO(long vehicleId, DateOnly date, long driverUserId, long pacientUserId, long transportRequestId, string transportAssignmentStatus)
    {
        VehicleId = vehicleId;
        Date = date;
        DriverUserId = driverUserId;
        PacientUserId = pacientUserId;
        TransportRequestId = transportRequestId;
        TransportAssignmentStatus = transportAssignmentStatus;
    }
}
