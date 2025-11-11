namespace CareMove.Argument.Argument;

public class OutputTransportRequest : BaseOutput<OutputTransportRequest>
{
    public long UserId { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly Hour { get; private set; }
    public string TransportKind { get; private set; }
    public string TransportStatus { get; private set; }
    public string OriginLocation { get; private set; }
    public string DestinationLocation { get; private set; }

    public OutputTransportRequest(long userId, DateOnly date, TimeOnly hour, string transportKind, string transportStatus, string originLocation, string destinationLocation)
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
