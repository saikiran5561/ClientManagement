using ClientManagement.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace ClientManagement.Repository
{
    public interface IClientsRepository
    {
        Task<int> AddClientAsync(ClientModel clientModel);
        Task<bool> DeleteClientAsync(int clientId);
        Task EditClientAsync(int clientId, ClientModel clientsModel);
        Task EditClientPatchAsync(int clientId, JsonPatchDocument<ClientModel> clientsModel);
        Task<bool> EditClientsAsync(int id);
        Task<ClientResult> GetAllClientsAsync(ClientResult client);
        Task<ClientModel> GetClientByIdAsync(int clientId);
    }
}