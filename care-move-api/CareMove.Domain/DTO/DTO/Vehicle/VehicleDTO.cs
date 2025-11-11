using CareMove.Argument.Argument;
using CareMove.Domain.DTO.Base;

namespace CareMove.Domain.DTO.DTO;

public class VehicleDTO : BaseDTO<VehicleDTO, OutputVehicle>
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

    public VehicleDTO() { }

    public VehicleDTO(string name, string licensePlate, string vehicleCategory, string vehicleModel, string color, string vehicleFuelType, string vehicleStatus, string renavam = null, string year = null, int capacity = 0)
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