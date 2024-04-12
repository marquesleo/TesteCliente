using Polly;
using Polly.Retry;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Acess.Util
{
    public class RestBase
    {
        public string Url { get; set; }
        public IRestClient Client { get; set; }
        public IRestRequest Request { get; set; }

        public RestBase(string _url)
        {
            Url = _url;
            Client = new RestClient(Url);
            Request = new RestRequest();
        }

        public T Get<T>() where T : class
        {
            Request.Method = Method.GET;

            var response = Client.Execute(Request);
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(response.Content + ex.Message);
            }
        }

        public IRestResponse Post()
        {
            Request.Method = Method.POST;

            return Client.Execute(Request);
        }
        public async Task<IRestResponse> PostAsync<T>(T o) where T : class
        {
            Request.Method = Method.POST;
            var contentType = "application/json;charset=UTF-8";
            Request.AddParameter(new Parameter(contentType, Newtonsoft.Json.JsonConvert.SerializeObject(o), ParameterType.RequestBody));

            return await Client.ExecuteAsync(Request);
        }
        public async Task<IRestResponse> PutAsync<T>(T o) where T : class
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(o);
            return await Client.ExecuteAsync(Request);
        }

        public IRestResponse Post<T>(T o, bool useRetry = false, int timeOutSeconds = -1) where T : class
        {
            IRestResponse response;
            int originalTimeout = timeOutSeconds;

            if (timeOutSeconds > -1)
            {
                Client.Timeout = timeOutSeconds * 1000;
                originalTimeout = Client.Timeout;
            }

            var contentType = "application/json;charset=UTF-8";
            Request.Method = Method.POST;
            Request.AddParameter(new Parameter(contentType, Newtonsoft.Json.JsonConvert.SerializeObject(o), ParameterType.RequestBody));

            if (useRetry)
                response = Client.Execute(Request);//RetryPolicy().Execute(() => Client.Execute(Request));
            else
                response = Client.Execute(Request);

            try
            {
                StatusCheck(response);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(response.Content + ex.Message);
            }
            finally
            {
                if (timeOutSeconds > -1)
                    Client.Timeout = originalTimeout;
            }
        }
        public async Task<T> GetAsync<T>() where T : class
        {
            Request.Method = Method.GET;

            Console.WriteLine($"[{DateTime.Now}] INICIANDO - Request ao Endpoint: {Client.BaseUrl}");

            var response = await Client.ExecuteAsync(Request);
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                var retryNum = 3;
                for (int i = 1; i <= retryNum && response.StatusCode == HttpStatusCode.TooManyRequests; i++)
                {
                    //var retrySleepDuration = Newtonsoft.Json.JsonConvert.DeserializeObject<RetornoErroMercosTooManyRequestDTO>(response.Content).TempoAtePermitirNovamente;
                    // Console.WriteLine($"[{DateTime.Now}] - Falha na chamada ao Endpoint: {response.ResponseUri} - {response.StatusCode}. Iniciando Retry nº {i} de {retryNum} em {retrySleepDuration}s");
                    //  await Task.Delay(TimeSpan.FromSeconds(retrySleepDuration+1));
                    Console.WriteLine($"[{DateTime.Now}] - Refazendo chamada ao Endpoint: {response.ResponseUri}");
                    response = await Client.ExecuteAsync(Request);
                }
            }
            //var response = await RetryPolicy().ExecuteAsync(() => Client.ExecuteAsync(Request));
            Console.WriteLine($"[{DateTime.Now}] CONCLUIDA - Request ao Endpoint: {Client.BaseUrl}");
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(response.Content + ex.Message);
            }
        }

        public IRestResponse PostUrlEnconded<T>(T o = null) where T : class
        {
            //precisamos interagir pelas properties de "o"
            Request.Method = Method.POST;

            IList<PropertyInfo> props = new List<PropertyInfo>(typeof(T).GetProperties());
            foreach (var prop in props)
            {
                Request.AddParameter(prop.Name, prop.GetValue(o));
            }

            return Client.Execute(Request);
        }


        public IRestResponse Put()
        {
            Request.Method = Method.PUT;
            return Client.Execute(Request);
        }
        public IRestResponse Delete()
        {
            Request.Method = Method.DELETE;

            return Client.Execute(Request);
        }

        public IRestResponse Put<T>(T o = null) where T : class
        {
            Request.Method = Method.PUT;
            Request.AddJsonBody(o);
            return Client.Execute(Request);
        }

        public void AddHeaders(params string[] _params)
        {
            if (_params.Count() % 2 == 0)
            {
                for (int i = 0; i < _params.Count(); i += 2)
                {
                    Request.AddHeader(_params[i], _params[i + 1]);
                }
            }
        }

        public void AddParameters(List<Parameter> parameters)
        {
            if (parameters != null && parameters.Count > 0)
                parameters.ForEach(p => Request.AddParameter(p));
        }

        public void AddParameters(params string[] _params)
        {
            if (_params.Count() % 2 == 0)
            {
                for (int i = 0; i < _params.Count(); i += 2)
                {
                    Request.AddParameter(_params[i], _params[i + 1]);
                }
            }
        }

        public void AddFile(string name, string path)
        {
            Request.AddFile(name, path);
        }

        public void AddFileBytes(string name, byte[] bytes)
        {
            Request.AddFileBytes(name, bytes, "csvFile");
        }

        public void SetContentType(string contentType)
        {
            if (Request.JsonSerializer != null)
                Request.JsonSerializer.ContentType = contentType;
        }

        /// <summary>
        /// Verifica se o status code pertence a classe de Successful responses (200, 201, 204... 299).
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            var difference = (int)statusCode - 200;

            return difference >= 0 && difference < 100;
        }

        private void StatusCheck(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode == HttpStatusCode.Forbidden)
                throw new Exception($"[{DateTime.Now}] Erro ao fazer request: {response.StatusCode} - {response.Content} - Request: {response.ResponseUri} ");

            if (!IsSuccessStatusCode(response.StatusCode))
            {
                var errorMessage = response.ErrorMessage != null ? response.ErrorMessage : response.Content;
                throw new Exception($"[{DateTime.Now}] Erro ao fazer request: {response.StatusCode} - {errorMessage} - Request: {response.ResponseUri} ");
            }

        }
        /// <summary>
        /// Post or put using an retry Policy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="useRetry"></param>
        /// <param name="timeOutSeconds"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IRestResponse> PostAsync<T>(T o = null, bool useRetry = false,
            int timeOutSeconds = -1, Method method = Method.POST) where T : class
        {
            IRestResponse response;
            int originalTimeout = timeOutSeconds;

            if (timeOutSeconds > -1)
            {
                Client.Timeout = timeOutSeconds * 1000;
                originalTimeout = Client.Timeout;
            }

            var contentType = "application/json;charset=UTF-8";
            Request.Method = method;
            Request.AddParameter(new Parameter(contentType, Newtonsoft.Json.JsonConvert.SerializeObject(o), ParameterType.RequestBody));

            if (useRetry)
            {
                response = await Client.ExecuteAsync(Request);
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    var retryNum = 3;
                    for (int i = 1; i <= retryNum && response.StatusCode == HttpStatusCode.TooManyRequests; i++)
                    {
                        //var retrySleepDuration = Newtonsoft.Json.JsonConvert.DeserializeObject<RetornoErroMercosTooManyRequestDTO>(response.Content).TempoAtePermitirNovamente;
                        //Console.WriteLine($"[{DateTime.Now}] - Falha na chamada ao Endpoint: {response.ResponseUri} - {response.StatusCode}. Iniciando Retry nº {i} de {retryNum} em {retrySleepDuration}s");
                        // await Task.Delay(TimeSpan.FromSeconds(retrySleepDuration + 1));
                        response = await Client.ExecuteAsync(Request);
                        Console.WriteLine($"[{DateTime.Now}] - Refazendo chamada ao Endpoint: {response.ResponseUri}");
                    }
                }

            }
            //response = await RetryPolicy().ExecuteAsync(() => {
            //    Console.WriteLine($"[{DateTime.Now}] - Executando request ");
            //    return Client.ExecuteAsync(Request);
            //   }
            //);
            else
                response = await Client.ExecuteAsync(Request);

            try
            {
                StatusCheck(response);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(response.Content + ex.Message);
            }
            finally
            {
                if (timeOutSeconds > -1)
                    Client.Timeout = originalTimeout;
            }
        }
        /// <summary>
        /// Política de retry para algumas apis, a maioria delas limita por 5  segundos no caso de bloqueio por
        /// TooManyRequests via sandbox, feita tratativa pra executar requests de acordo com liberação de response 
        /// POR ENQUANTO NÃO ESTÁ SENDO MAIS UTILIZADO POIS O PACOTE NÃO PERMITE ALTERAÇÃO DINAMICA DO RETRY SLEEP DURATION 
        /// https://github.com/App-vNext/Polly/issues/473#issuecomment-401261146
        /// </summary>
        /// <returns></returns>
        private AsyncRetryPolicy<IRestResponse> RetryPolicy()
        {
            var retryCount = 3;
            var retrySleepDuration = 6;

            return Policy
                .HandleResult<IRestResponse>(response => response.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(
                    retryCount,
                    sleepDurations => {
                        Console.WriteLine($"[{DateTime.Now}] - duração para proxima chamada {TimeSpan.FromSeconds(retrySleepDuration)}s");
                        return TimeSpan.FromSeconds(retrySleepDuration);
                    },
                    onRetry: (response, timespan, retryNum, context) =>
                    {
                        //TODO tornar este objeto da mercos genérico
                        // retrySleepDuration = Newtonsoft.Json.JsonConvert.DeserializeObject<RetornoErroMercosTooManyRequestDTO>(response.Result.Content).TempoAtePermitirNovamente;
                        // Logging so we know the retry has happened.
                        Console.WriteLine($"[{DateTime.Now}] - Falha na chamada ao Endpoint: {response.Result.ResponseUri} - {response.Result.StatusCode}. Iniciando Retry nº {retryNum} de {retryCount} em {retrySleepDuration}s");
                    }
                );
        }
    }
}
