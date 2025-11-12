namespace CareMove.Argument.Argument;

public class InputAuthenticate
{
    public string CPF { get; private set; }
    public string Password { get; private set; }

    public InputAuthenticate(string cpf, string password)
    {
        CPF = cpf;
        Password = password;
    }
}
