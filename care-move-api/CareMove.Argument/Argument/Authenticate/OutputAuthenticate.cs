namespace CareMove.Argument.Argument;

public class OutputAuthenticate
{
    public string Token { get; set; }
    public string UserCategory { get; set; }
    public long UserId { get; set; }

    public OutputAuthenticate(string token, string userCategory, long userId)
    {
        Token = token;
        UserCategory = userCategory;
        UserId = userId;
    }
}
