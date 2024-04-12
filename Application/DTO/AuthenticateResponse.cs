using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class AuthenticateResponse
    {
        public string token { get; set; }
        public DateTime expira { get; set; }
        public string usuario { get; set; }
        public AuthenticateResponse(string usuario,
                                    string jwtToken, 
                                    DateTime expira)
        {

            this.usuario = usuario;
            this.token = jwtToken;
            this.expira = expira;
        }
    }
}
