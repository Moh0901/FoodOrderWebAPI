namespace UserService.TokenHandler
{
    public interface ITokenHandler
    {
        string CreateToken(string username);
    }
}
