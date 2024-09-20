using ClientManagement.Data;
using ClientManagement.Models;
using ClientManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsController(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients([FromQuery] ClientResult pagedClientResult)
        {
            var clients = await _clientsRepository.GetAllClientsAsync(pagedClientResult);

            return Ok(clients);
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClientById([FromRoute] int clientId)
        {
            var client = await _clientsRepository.GetClientByIdAsync(clientId);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewClient([FromBody] ClientModel clientModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var client = await _clientsRepository.AddClientAsync(clientModel);

                return CreatedAtAction(
                    nameof(GetClientById),
                    new { clientId = client },
                    client);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditClientDetails([FromBody] ClientModel clientModel)
        {
            var client = await _clientsRepository.GetExistingClient(clientModel.ClientId);
            if (client == null)
            {
                return BadRequest("Entered client id is not found");
            }
            await _clientsRepository.EditClientAsync(client, clientModel);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditClientPatch([FromBody] JsonPatchDocument<ClientModel> clientModel,
            [FromRoute] int id)
        {
            var client = await _clientsRepository.GetExistingClient(id);
            if (client == null)
            {
                return BadRequest("Entered client id is not found");
            }
            await _clientsRepository.EditClientPatchAsync(client, clientModel);
            return Ok();
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClient([FromRoute] int clientId)
        {
            var client = await _clientsRepository.GetClientByIdAsync(clientId);
            if (client == null)
            {
                return BadRequest("Requested client id is not found");
            }
            await _clientsRepository.DeleteClientAsync(clientId);
            return Ok();
        }
    }
}