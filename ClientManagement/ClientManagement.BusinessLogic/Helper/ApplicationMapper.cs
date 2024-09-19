using AutoMapper;
using ClientManagement.Data;
using ClientManagement.Models;

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
