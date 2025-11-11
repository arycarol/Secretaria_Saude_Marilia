using CareMove.Argument.Argument;
using CareMove.Domain.DTO.DTO.User;

namespace CareMove.Infrastructure.Entity.Entity;

public class User : BaseEntity<User, UserDTO, OutputUser>
{
    public string Name { get; private set; }
    public string CPF { get; private set; }
    public string Email { get; private set; }
    public string Telephone { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string Password { get; private set; }
    public string UserCategory { get; private set; }
    public long VehicleId { get; private set; }

    public Vehicle Vehicle { get; private set; }

    public User() { }

    public User(string name, string cPF, string email, string telephone, DateTime birthDate, string password, string userCategory, long vehicleId, Vehicle vehicle)
    {
        Name = name;
        CPF = cPF;
        Email = email;
        Telephone = telephone;
        BirthDate = birthDate;
        Password = password;
        UserCategory = userCategory;
        VehicleId = vehicleId;
        Vehicle = vehicle;
    }
}