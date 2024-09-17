using ClientManagement.Model;
using ClientManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsController(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients([FromQuery] PagedClientResult pagedClientResult)
        {
            var clients = await _clientsRepository.GetAllClientsAsync(pagedClientResult);
            if (clients.TotalCount == 0)
            {
                return NotFound();
            }
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
        public async Task<IActionResult> EditClientDetails([FromBody] ClientModel clientModel, [FromRoute] int id)
        {
            bool client = await _clientsRepository.EditClientsAsync(id);
            if (!client)
            {
                return NotFound("Entered client id is not found");
            }
            else
            {
                await _clientsRepository.EditClientAsync(id, clientModel);
                return Ok();
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditClientPatch([FromBody] JsonPatchDocument<ClientModel> clientModel,
            [FromRoute] int id)
        {
            bool client = await _clientsRepository.EditClientsAsync(id);
            if (!client)
            {
                return NotFound("Entered client id is not found");
            }
            else
            {
                await _clientsRepository.EditClientPatchAsync(id, clientModel);
                return Ok();
            }
        }

        [HttpDelete("{patientid}")]
        public async Task<IActionResult> DeleteClient([FromRoute] int clientId)
        {
            var client = await _clientsRepository.GetClientByIdAsync(clientId);
            if (client == null || client.ClientId <= 0)
            {
                return BadRequest("Requested client id is not found");
            }
            await _clientsRepository.DeleteClientAsync(clientId);
            return Ok();
        }
    }
}
