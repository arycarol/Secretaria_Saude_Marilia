namespace CareMove.Argument.Argument;

public class OutputUser : BaseOutput<OutputUser>
{
    public string Name { get; private set; }
    public string CPF { get; private set; }
    public string Email { get; private set; }
    public string Telephone { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public string Password { get; set; }
    public string UserCategory { get; private set; }
    public long? VehicleId { get; private set; }

    public OutputUser(string name, string cPF, string email, string telephone, DateOnly birthDate, string password, string userCategory, long? vehicleId)
    {
        Name = name;
        CPF = cPF;
        Email = email;
        Telephone = telephone;
        BirthDate = birthDate;
        Password = password;
        UserCategory = userCategory;
        VehicleId = vehicleId;
    }
}
