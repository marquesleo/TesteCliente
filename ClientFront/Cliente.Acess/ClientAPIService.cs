

using Application.DTO;
using Cliente.Acess.Util;
using Newtonsoft.Json;

namespace Cliente.Acess
{
    public class ClientAPIService
    {

        private AuthenticateResponse _token;
        private AuthenticateResponse token
        {
            get
            {
                if (_token == null || _token?.expira < DateTime.Now)
                    _token = GetToken();

                return _token;
            }
        }
        public ClientAPIService()
        {
            _token = GetToken();
        }
        const string url = "https://localhost:7138";
        const string controller = "/api/client";
        private async Task<RestBase> CreateBaseRequest(string path, string token, bool buscaComtoken = true)
        {
          
            var rest = new RestBase($"{url}{path}");
            if (buscaComtoken && !string.IsNullOrEmpty(token)) 
                rest.AddHeaders("Authorization", $"Bearer {token}");
            rest.AddHeaders("Content-Type", "application/json");
            rest.AddHeaders("Accept", "*/*");
            rest.AddHeaders("Accept", "application/json");
            return rest;
        }



        public async Task<ClientDTO> GetCliente(int id) {

            try
            {
                
                var rest = await CreateBaseRequest(controller + "/"+ id, token.token);
                var response = rest.Client.Execute(rest.Request);
                if (response.IsSuccessful)
                {
                    return JsonConvert.DeserializeObject<ClientDTO>(response.Content);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw (new Exception($"Erro ao buscar cliente  ${ex.Message}"));
            }
            return null;

        }

        public AuthenticateResponse GetToken()
        {

            try
            {
                var rest =  CreateBaseRequest(controller + "/autenticar", "",false);
                var response = rest.Result.Client.Execute(rest.Result.Request);
                if (response.IsSuccessful)
                {
                    return JsonConvert.DeserializeObject<AuthenticateResponse>(response.Content);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw (new Exception($"Erro ao buscar token  ${ex.Message}"));
            }
            return null;

        }


        public async Task<List<ClientDTO>> GetAll()
        {

            try
            {
                var rest = await CreateBaseRequest(controller+ "/all", token.token);
                var response = rest.Client.Execute(rest.Request);
                if (response.IsSuccessful)
                {
                    return JsonConvert.DeserializeObject<List<ClientDTO>>(response.Content);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw (new Exception($"Erro ao buscar cliente  ${ex.Message}"));
            }
            return null;

        }

        public async Task<bool> Post(ClientDTO cliente)
        {
            try
            {
                var rest = await CreateBaseRequest(controller , token.token);

                var json = JsonConvert.SerializeObject(cliente);
                var response = await rest.PostAsync(cliente, useRetry: true);
                if (response.IsSuccessful && response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return true;
                }
                else if (response?.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return false;
                }
                else
                    throw (new Exception($"Erro ao Incluir client . ${response?.StatusCode.ToString()}"));
            }
            catch (Exception ex)
            {
                throw (new Exception($"Erro ao incluir client. ${ex.Message}"));
            }
            return false;

        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var rest = await CreateBaseRequest(controller +"/"+ id, token.token);

                var response =  rest.Delete();
                if (response.IsSuccessful)
                {
                    return true;
                }
                else if (response?.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return false;
                }
                else
                    throw (new Exception($"Erro ao excluir . ${response?.StatusCode.ToString()}"));
            }
            catch (Exception ex)
            {
                throw (new Exception($"Erro ao excluir. ${ex.Message}"));
            }
            return false;

        }

    }
}
