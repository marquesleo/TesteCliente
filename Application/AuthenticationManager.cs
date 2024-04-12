using Application.DTO;
using Application.Ports;
using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Services.TokenService;

namespace Application
{
    public class AuthenticationManager : IAuthenticationManager
    {
        public  AuthenticateResponse Authenticate(string usuario)
        {
            var jwtToken = generateJwtToken(usuario);
            return new AuthenticateResponse(usuario, jwtToken.token, jwtToken.expira);
        }

        private MeuToken generateJwtToken(string usuario)
        {
            return TokenService.GenerateToken(usuario);
        }
    }
}
