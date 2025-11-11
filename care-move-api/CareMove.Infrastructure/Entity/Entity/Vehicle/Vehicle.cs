using CareMove.Argument.Argument;
using CareMove.Domain.DTO.DTO;

namespace CareMove.Infrastructure.Entity.Entity;

public class Vehicle : BaseEntity<Vehicle, VehicleDTO, OutputVehicle>
{
    public string Name { get; private set; }
    public string LicensePlate { get; private set; }
    public string VehicleCategory { get; private set; }
    public string VehicleModel { get; private set; }
    public string Color { get; private set; }
    public string VehicleFuelType { get; private set; }
    public string VehicleStatus { get; private set; }
    public string Renavam { get; private set; }
    public string Year { get; private set; }
    public int Capacity { get; private set; }

    public Vehicle() { }

    public Vehicle(string name, string licensePlate, string vehicleCategory, string vehicleModel, string color, string vehicleFuelType, string vehicleStatus, string renavam, string year, int capacity)
    {
        Name = name;
        LicensePlate = licensePlate;
        VehicleCategory = vehicleCategory;
        VehicleModel = vehicleModel;
        Color = color;
        VehicleFuelType = vehicleFuelType;
        VehicleStatus = vehicleStatus;
        Renavam = renavam;
        Year = year;
        Capacity = capacity;
    }
}