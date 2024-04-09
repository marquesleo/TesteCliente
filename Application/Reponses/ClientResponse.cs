using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reponses
{
    public class ClientResponse: Response
    {

        public ClientDTO Data { get; set; }
    }
}
