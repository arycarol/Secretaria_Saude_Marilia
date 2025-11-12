using CareMove.Argument.Argument;

namespace CareMove.Domain.Interface.Service;

public interface IAuthenticateService
{
    string Authenticate(InputAuthenticate inputAuthenticate);
}
