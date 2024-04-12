using Application.DTO;


namespace Application.Ports
{
    public interface IAuthenticationManager
    {
        AuthenticateResponse Authenticate(string usuario);
    }
}
