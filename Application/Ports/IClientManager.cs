using Application.DTO;
using Application.Reponses;
using Application.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ports
{
    public interface IClientManager
    {

        public Task<ClientResponse> GetClient(int id);

        public Task<ClientResponse> CreateClient(CreateClientRequest request);

        public Task<IEnumerable<ClientDTO>> GetClients();



    }
}
