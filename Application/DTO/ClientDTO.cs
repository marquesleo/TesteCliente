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
        
        public static 

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
    }
}
