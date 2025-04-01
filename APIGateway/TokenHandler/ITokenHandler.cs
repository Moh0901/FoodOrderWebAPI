using APIGateway.Model;

namespace APIGateway.TokenHandler
{
    public interface ITokenHandler
    {
        String CreateToken(string username);
    }
}
