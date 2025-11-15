using System.Text.Json.Serialization;

namespace CareMove.Argument.Argument;

public class InputUpdateUser : BaseInputUpdate<InputUpdateUser>
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Telephone { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public string UserCategory { get; private set; }
    public long? VehicleId { get; private set; }

    [JsonConstructor]
    public InputUpdateUser(long id, string name, string email, string telephone, DateOnly birthDate, string userCategory, long? vehicleId) : base(id)
    {
        Name = name;
        Email = email;
        Telephone = telephone;
        BirthDate = birthDate;
        UserCategory = userCategory;
        VehicleId = vehicleId;
    }
}