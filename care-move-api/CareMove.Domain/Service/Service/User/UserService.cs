using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.DTO.DTO;
using CareMove.Domain.DTO.DTO.User;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service.Service;

namespace CareMove.Domain.Service.Service;

public class UserService : BaseService<IUserRepository, InputCreateUser, InputUpdateUser, InputGenericDelete, OutputUser, UserDTO>, IUserService
{
    private readonly IVehicleRepository _vehicleRepository;

    public UserService(IUserRepository repository, IMapper mapper, IVehicleRepository vehicleRepository) : base(repository, mapper)
    {
        _vehicleRepository = vehicleRepository;
    }

    #region Base - Tirar senha
    public override OutputUser? Get(long id)
    {
        OutputUser? user = base.Get(id);

        if (user == null)
            return default;

        user.Password = "";
        return user;
    }

    public override List<OutputUser>? GetAll()
    {
        List<OutputUser>? listUser = base.GetAll();

        if (listUser == null || listUser.Count == 0)
            return listUser;

        foreach (var i in listUser)
            i.Password = "";

        return listUser;
    }
    #endregion

    protected override void OnCreateMultiple(List<InputCreateUser>? listInputCreate)
    {
        List<VehicleDTO> listVehicleDTO = _vehicleRepository.GetListByListId((from I in listInputCreate where I.VehicleId != null select I.VehicleId!.Value).ToList())!;
        foreach (var i in listInputCreate)
        {
            VehicleDTO? relatedVehicleDTO = (from j in listVehicleDTO where j.Id == i.VehicleId select j).FirstOrDefault();
            if (i.VehicleId != null && relatedVehicleDTO == null)
                throw new Exception("Id veículo inválido");

            UserDTO? originalUserDTO = _repository.GetFilterByCPF(i.CPF);
            if(originalUserDTO != null)
                throw new Exception("Já existe um usuário com esse CPF");
        }
    }

    public List<OutputUser>? GetFilterByName(string parameter)
    {
        return Convert(_repository.GetFilterByName(parameter));
    }
}