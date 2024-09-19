using ClientManagement.Models;

namespace ClientManagement.Repository
{
    public interface IAccountRepository
    {
        Task<string> LoginAsync(SignInModel signInModel);
        Task<string> SignUpAsync(SignUpModel signUpModel);
    }
}