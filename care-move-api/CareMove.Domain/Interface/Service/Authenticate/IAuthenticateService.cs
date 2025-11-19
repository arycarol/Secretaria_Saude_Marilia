using CareMove.Argument.Argument;

namespace CareMove.Domain.Interface.Service;

public interface IAuthenticateService
{
    OutputAuthenticate Authenticate(InputAuthenticate inputAuthenticate);
}
