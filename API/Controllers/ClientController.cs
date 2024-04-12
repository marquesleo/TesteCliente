using Application;
using Application.DTO;
using Application.Ports;
using Application.Reponses;
using Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientManager _clientManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILogger<ClientController> _logger;

        public ClientController( IClientManager clientManager,
                                 IAuthenticationManager authenticationManager,
                                 ILogger<ClientController> logger)
        {
            this._clientManager = clientManager;
            this._logger = logger;
            this._authenticationManager = authenticationManager;
        }


        [HttpGet]
        [Route("autenticar")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate()
        {
            try
            {
                var response = _authenticationManager.Authenticate("leonardo");
                return Ok(response);
                
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }


        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> Get(int id)
        {

            var res = await _clientManager.GetClient(id);

            if (res.Success)
                return Created("", res.Data);
            else if (res.ErrorCode == ErrorCodes.CLIENT_NOT_FOUND)
                return NotFound(res);
            else
                return BadRequest(res);
        }
        [HttpDelete("{id}")]
      
        public async Task<ActionResult<ClientResponse>> Delete(int id)
        {

            var res = await _clientManager.DeleteCliente(id);

            if (res != null && res.Success)
                return Ok(res);
           
            else
                return BadRequest(res);
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<ClientDTO>> GetAll()
        {

            var res = await _clientManager.GetClients();

            if (res != null && res.Any())
                return Ok(res);
            else if (res != null && !res.Any())
                return NotFound(res);
            else
                return BadRequest(res);
        }

        [HttpPost]
        public async Task<ActionResult<ClientResponse>> Post(ClientDTO clientDTO)
        {

            var request = new CreateClientRequest
            {
                Data = clientDTO,
            };
            var res = await _clientManager.CreateClient(request);

            if (res.Success)
            {
                return Created("", res.Data);

            }
            else if (res.ErrorCode == ErrorCodes.INVALID_EMAIL)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.EMAIL_DUPLICATE)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.ADDRESS_DUPLICATE)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }


    }
}
