using ClientManagement.Model;
using Microsoft.AspNetCore.JsonPatch;

namespace ClientManagement.Repository
{
    public interface IClientsRepository
    {
        Task<List<ClientModel>> GetAllClientsAsync();
        Task<ClientModel> GetClientByIdAsync(int clientId);
        Task<int> AddClientAsync(ClientModel clientModel);
        Task EditClientAsync(int patientId, ClientModel clientsModel);
        Task<bool> EditClientsAsync(int id);
        Task EditClientPatchAsync(int clientId, JsonPatchDocument<ClientModel> clientsModel);
        Task<bool> DeleteClientAsync(int clientId);
    }
}