using ClientManagement.Models;

namespace ClientManagement.Repository
{
    public interface IClientLoginRepository
    {
        Task<string> LoginAsync(SignInModel signInModel);
        Task<string> SignUpAsync(SignUpModel signUpModel);
    }
}