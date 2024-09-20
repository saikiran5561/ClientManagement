using AutoMapper;
using ClientManagement.Caching;
using ClientManagement.Data;
using ClientManagement.Models;
using LazyCache;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Dynamic.Core;

namespace ClientManagement.Repository
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly ClientManagementContext _clientDataContext;
        private readonly IMapper _mapper;
        private readonly ICacheProvider _cacheProvider;

        public ClientsRepository(ClientManagementContext clientDataContext, IMapper mapper, ICacheProvider cacheProvider)
        {
            _clientDataContext = clientDataContext;
            _mapper = mapper;
            _cacheProvider = cacheProvider;
        }

        public async Task<ClientResult> GetAllClientsAsync(ClientResult client)
        {
            var clientQuery = client.FilterBy?.Split(' ')[0];

            var clients = _clientDataContext.Clients.AsQueryable();

            //Filtering
            if (!string.IsNullOrWhiteSpace(clientQuery))
            {
                clients = clients.Where(client => client.ClientName.ToLower().Contains(clientQuery) ||
                                                         client.Description.ToLower().Contains(clientQuery));
            }

            //Sorting
            if (!string.IsNullOrWhiteSpace(client.SortBy))
            {
                clients = clients.OrderBy(client.SortBy);
            }
            else
            {
                clients = clients.OrderBy(client => client.ClientId);
            }

            //Pagination
            client.TotalCount = await clients.CountAsync();
            var pagedClientData = await clients
                .Skip((client.Page - 1) * client.Limit)
                .ToListAsync();

            client.Clients = _mapper.Map<List<ClientModel>>(pagedClientData);

            return client;
        }

        public async Task<ClientModel> GetClientByIdAsync(int clientId)
        {
            if (!_cacheProvider.TryGetValue(string.Format(CacheKeys.Client, clientId), out ClientModel clientModel))
            {
                var client = await _clientDataContext.Clients.FindAsync(clientId);

                clientModel = _mapper.Map<ClientModel>(client);

                var entryOption = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(CacheKeys.AbsoluteExpiration),
                    SlidingExpiration = TimeSpan.FromSeconds(CacheKeys.SlidingExpiration),
                    Size = CacheKeys.EntryOptionsSize
                };
                _cacheProvider.Set(string.Format(CacheKeys.Client, clientId), clientModel, entryOption);
            }
            return clientModel;
        }

        public async Task<int> AddClientAsync(ClientModel clientModel)
        {
            var client = _mapper.Map<Client>(clientModel);
            client.CreatedDate = DateTime.Now;
            _clientDataContext.Clients.Add(client);
            await _clientDataContext.SaveChangesAsync();

            return client.ClientId;
        }

        public async Task EditClientAsync(Client existingClient, ClientModel clientsModel)
        {
            existingClient.UpdatedDate = DateTime.Now;
            _mapper.Map(clientsModel, existingClient);
            await _clientDataContext.SaveChangesAsync();
        }

        public async Task EditClientPatchAsync(Client existingClint, JsonPatchDocument<ClientModel> clientsModel)
        {
            var client = _mapper.Map<ClientModel>(existingClint);

            clientsModel.ApplyTo(client);
            existingClint.UpdatedDate = DateTime.Now;
            _mapper.Map(client, existingClint);
            await _clientDataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteClientAsync(int clientId)
        {
            var client = await _clientDataContext.Clients.FindAsync(clientId);

            _clientDataContext.Clients.Remove(client);
            await _clientDataContext.SaveChangesAsync();

            return true;
        }

        public async Task<Client> GetExistingClient(int clientId)
        {
            var existingClient = await _clientDataContext.Clients.FirstOrDefaultAsync(client => client.ClientId == clientId);

            return existingClient;
        }
    }
}
