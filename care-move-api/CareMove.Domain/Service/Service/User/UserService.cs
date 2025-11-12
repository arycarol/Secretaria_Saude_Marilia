using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.DTO.DTO.User;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service.Service;

namespace CareMove.Domain.Service.Service;

public class UserService : BaseService<IUserRepository, InputCreateUser, InputUpdateUser, InputGenericDelete, OutputUser, UserDTO>, IUserService
{
    public UserService(IUserRepository repository, IMapper mapper) : base(repository, mapper) { }

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

    public List<OutputUser>? GetFilterByName(string parameter)
    {
        return Convert(_repository.GetFilterByName(parameter));
    }
}