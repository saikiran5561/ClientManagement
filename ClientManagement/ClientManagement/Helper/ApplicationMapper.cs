using AutoMapper;
using ClientManagement.ClientData;
using ClientManagement.Model;

namespace ClientManagement.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<ClientModel, Client>().ReverseMap();
        }
    }
}
