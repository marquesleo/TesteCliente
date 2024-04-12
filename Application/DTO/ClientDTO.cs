using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ClientDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IList<AddressDTO> Address { get; set; }   
       
        public static ClientDTO MapToDTO(Domain.Entities.Client client)
        {
          var cliente =  new ClientDTO()
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
            };

           if (client.AddressList.Any())
            {
                cliente.Address = new List<AddressDTO>();
                foreach (var address in client.AddressList) {

                    var addressDTO = new AddressDTO();
                    addressDTO.Street= address.Street;
                    addressDTO.Id = address.Id;
                    addressDTO.ClientId = address.ClientId;
                    addressDTO.ZipCode = address.ZipCode;
                    cliente.Address.Add(addressDTO);
                }
            }
            return cliente;
        }

        public static Domain.Entities.Client MapToEntity(ClientDTO dto)
        {
            var client = new Domain.Entities.Client
            {
                Name = dto.Name,
                Email = dto.Email,
                Id = dto.Id
            };

            if (dto.Address != null && dto.Address.Any())
            {
                client.AddressList = new List<Domain.Entities.Address>();
                foreach (var addressDTO in dto.Address)
                {
                    var address = new Domain.Entities.Address
                    {
                        Id = addressDTO.Id,
                        ClientId = client.Id,
                        Street = addressDTO.Street,
                        ZipCode = addressDTO.ZipCode,
                        Client = client
                    };
                    client.AddressList.Add(address);
                }
            }
            return client;
        }
    }
}
