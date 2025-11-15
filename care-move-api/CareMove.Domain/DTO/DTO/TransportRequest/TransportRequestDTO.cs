using CareMove.Argument.Argument;
using CareMove.Domain.DTO.Base;

namespace CareMove.Domain.DTO.DTO;

public class TransportRequestDTO : BaseDTO<TransportRequestDTO, OutputTransportRequest>
{
    public long UserId { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly Hour { get; private set; }
    public string TransportKind { get; private set; }
    public string TransportStatus { get; set; }
    public string OriginLocation { get; private set; }
    public string DestinationLocation { get; private set; }

    public TransportRequestDTO() { }

    public TransportRequestDTO(long userId, DateOnly date, TimeOnly hour, string transportKind, string transportStatus, string originLocation, string destinationLocation)
    {
        UserId = userId;
        Date = date;
        Hour = hour;
        TransportKind = transportKind;
        TransportStatus = transportStatus;
        OriginLocation = originLocation;
        DestinationLocation = destinationLocation;
    }
}