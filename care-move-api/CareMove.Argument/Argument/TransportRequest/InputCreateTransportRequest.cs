using System.Text.Json.Serialization;

namespace CareMove.Argument.Argument;

public class InputCreateTransportRequest : BaseInputCreate<InputCreateTransportRequest>
{
    public long UserId { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly Hour { get; private set; }
    public string TransportKind { get; private set; }
    public string OriginLocation { get; private set; }
    public string DestinationLocation { get; private set; }

    [JsonConstructor]
    public InputCreateTransportRequest(long userId, DateOnly date, TimeOnly hour, string transportKind, string originLocation, string destinationLocation)
    {
        UserId = userId;
        Date = date;
        Hour = hour;
        TransportKind = transportKind;
        OriginLocation = originLocation;
        DestinationLocation = destinationLocation;
    }
}