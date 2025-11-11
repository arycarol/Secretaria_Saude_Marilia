using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Domain.DTO.DTO.User;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Infrastructure.Context;
using CareMove.Infrastructure.Encryption;
using CareMove.Infrastructure.Entity.Entity;
using CareMove.Infrastructure.Repository.Base;

namespace CareMove.Infrastructure.Repository.Repository;

public class UserRepository : BaseRepository<User, UserDTO, OutputUser>, IUserRepository
{
    public UserRepository(AppDbContext context, IMapper mapper) : base(context, mapper) { }

    #region Base
    #region Create
    public override List<long> CreateMultiple(List<UserDTO>? listDTO)
    {
        List<UserDTO> listEncryptedUserDTO = (from i in listDTO
                                              let encrypitedPassword = EncryptionHandler.Encrypt(i.Password)
                                              let setValue = i.Password = encrypitedPassword
                                              select i).ToList();

        return base.CreateMultiple(listEncryptedUserDTO);
    }
    #endregion
    #endregion
}