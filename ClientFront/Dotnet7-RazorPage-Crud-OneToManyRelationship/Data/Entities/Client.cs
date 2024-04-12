namespace dot7.razor.crudsample.Data.Entities
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public List<Address> Addresses { get; set; }

        public static Application.DTO.ClientDTO MapToDTO(Client client)
        {
            var clientDTO = new Application.DTO.ClientDTO();
            clientDTO.Id = client.Id;
            clientDTO.Name = client.Name;
            clientDTO.Email = client.Email;
            if (client.Addresses != null && client.Addresses.Any())
            {
                clientDTO.Address = new List<Application.DTO.AddressDTO>();
                foreach (var address in client.Addresses)
                {
                    if (!string.IsNullOrEmpty(address.Street) &&
                        !string.IsNullOrEmpty(address.ZipCode) &&
                        address.Id != 0)
                    {
                        var addressDTO = new Application.DTO.AddressDTO();
                        addressDTO.ClientId = address.ClientId;
                        addressDTO.ZipCode = address.ZipCode;
                        addressDTO.Id = address.Id;
                        addressDTO.Street = address.Street;
                        clientDTO.Address.Add(addressDTO);
                    }
                }
            }

            return clientDTO;
        }


        public static Client MapToEntity(Application.DTO.ClientDTO clientDTO)
        {
            var client = new Client();

            client.Id = clientDTO.Id;
            client.Name = clientDTO.Name;
            client.Email = clientDTO.Email;

            if (clientDTO.Address != null && clientDTO.Address.Any())
            {
                client.Addresses = new List<Address>();
                foreach (var addressDTO in clientDTO.Address)
                {
                    var address = new Address();
                    address.Street = addressDTO.Street;
                    address.Id = addressDTO.Id;
                    address.ClientId = addressDTO.ClientId;
                    address.ZipCode = addressDTO.ZipCode;
                    client.Addresses.Add(address);
                    
                }
               
                
            }
            if (client.Addresses == null)
            {
                client.Addresses = new List<Address>();
                client.Addresses.Add(new Address());
                client.Addresses.Add(new Address());
            }
            else if (client.Addresses.Count == 1)
            {
                client.Addresses.Add(new Address());

            }




            return client;
        }
    }
}
