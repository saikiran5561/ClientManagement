using AutoMapper;
using ClientManagement.ClientData;
using ClientManagement.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Repository
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly ClientDataContext _clientDataContext;
        private readonly IMapper _mapper;

        public ClientsRepository(ClientDataContext clientDataContext, IMapper mapper)
        {
            _clientDataContext = clientDataContext;
            _mapper = mapper;
        }

        public async Task<List<ClientModel>> GetAllClientsAsync()
        {
            var clients = await _clientDataContext.Clients.ToListAsync();

            return _mapper.Map<List<ClientModel>>(clients);
        }

        public async Task<ClientModel> GetClientByIdAsync(int clientId)
        {
            var client = await _clientDataContext.Clients.FindAsync(clientId);

            return _mapper.Map<ClientModel>(client);
        }

        public async Task<int> AddClientAsync(ClientModel clientModel)
        {
            var client = _mapper.Map<Client>(clientModel);
            client.CreatedDate = DateTime.Now;
            _clientDataContext.Clients.Add(client);
            await _clientDataContext.SaveChangesAsync();

            return client.ClientId;
        }

        public async Task EditClientAsync(int clientId, ClientModel clientsModel)
        {
            var client = await _clientDataContext.Clients.FindAsync(clientId);

            client.UpdatedDate = DateTime.Now;
            _mapper.Map(clientsModel, client);
            await _clientDataContext.SaveChangesAsync();
        }

        public async Task<bool> EditClientsAsync(int id)
        {
            var client = await _clientDataContext.Clients.AnyAsync(clients => clients.ClientId == id);

            return client;
        }

        public async Task EditClientPatchAsync(int clientId, JsonPatchDocument<ClientModel> clientsModel)
        {
            var client = await _clientDataContext.Clients.FindAsync(clientId);

            var existingClient = _mapper.Map<ClientModel>(client);

            clientsModel.ApplyTo(existingClient);
            client.UpdatedDate = DateTime.Now;
            _mapper.Map(existingClient, client);
            await _clientDataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteClientAsync(int clientId)
        {
            var client = await _clientDataContext.Clients.FindAsync(clientId);

            _clientDataContext.Clients.Remove(client);
            await _clientDataContext.SaveChangesAsync();

            return true;
        }
    }
}
