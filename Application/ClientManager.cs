using Application.DTO;
using Application.Ports;
using Application.Reponses;
using Application.Requests;
using Domain.Exceptions;
using Domain.Ports;

namespace Application
{
    public class ClientManager : IClientManager
    {

        private IClientRepository _clientRepository;
        
        public ClientManager(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public async Task<ClientResponse> CreateClient(CreateClientRequest request)
        {
            try
            {
                var cliente = ClientDTO.MapToEntity(request.Data);

                await cliente.Save(_clientRepository);
                request.Data.Id = cliente.Id;

                return new ClientResponse
                {
                    Data = request.Data,
                    Success = true
                };
            }
            catch (AddressDuplicateException)
            {
                return new ClientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ADDRESS_DUPLICATE,
                    Message = ErrorCodes.ADDRESS_DUPLICATE.ToString(),
                };
            }
            catch (EmailDuplicateException)
            {
                return new ClientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.EMAIL_DUPLICATE,
                    Message = ErrorCodes.EMAIL_DUPLICATE.ToString(),
                };
            }
            catch (InvalidEmailException e)
            {
                return new ClientResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INVALID_EMAIL,
                    Message = "E-mail invalid",
                };
            }
            catch (Exception ex)
            {
                return new ClientResponse
                {

                    Success = false,
                    ErrorCode = ErrorCodes.COULDNOT_STORE_DATA,
                    Message = ex.Message
                };

            }
        }

        public async Task<ClientResponse> GetClient(int id)
        {
            var client = await _clientRepository
               .GetById(id);

            if (client == null)
            {
                return new ClientResponse()
                {
                    Success = false,
                    ErrorCode = ErrorCodes.CLIENT_NOT_FOUND,
                    Message = ErrorCodes.CLIENT_NOT_FOUND.ToString()
                };
            }

            return new ClientResponse()
            {
                Data = ClientDTO.MapToDTO(client),
                Success = true
            };

        }
    

        public async Task<IEnumerable<ClientDTO>> GetClients()
        {
         var client = await _clientRepository
             .GetAll();


            var lstClientes = new List<ClientDTO>();   
            if (client != null)
            {
                foreach (var item in client)
                {
                    var clientDTO = new ClientDTO();
                    clientDTO = ClientDTO.MapToDTO(item);
                    lstClientes.Add(clientDTO);
                }
            }

            return lstClientes;
        
        }
}
}
