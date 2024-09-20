using ClientManagement.Data;
using ClientManagement.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace ClientManagement.Repository
{
    public interface IClientsRepository
    {
        Task<int> AddClientAsync(ClientModel clientModel);
        Task<bool> DeleteClientAsync(int clientId);
        Task EditClientAsync(Client existingClient, ClientModel clientsModel);
        Task EditClientPatchAsync(Client existingClint, JsonPatchDocument<ClientModel> clientsModel);
        Task<ClientResult> GetAllClientsAsync(ClientResult client);
        Task<ClientModel> GetClientByIdAsync(int clientId);
        Task<Client> GetExistingClient(int clientId);
    }
}