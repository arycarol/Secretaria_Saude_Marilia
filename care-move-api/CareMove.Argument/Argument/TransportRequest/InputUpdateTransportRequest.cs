namespace CareMove.Argument.Argument;

public class InputUpdateTransportRequest : BaseInputUpdate<InputUpdateTransportRequest>
{
    public long UserId { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly Hour { get; private set; }
    public string TransportKind { get; private set; }
    public string TransportStatus { get; private set; }
    public string OriginLocation { get; private set; }
    public string DestinationLocation { get; private set; }

    public InputUpdateTransportRequest(long id, long userId, DateOnly date, TimeOnly hour, string transportKind, string transportStatus, string originLocation, string destinationLocation) : base(id)
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