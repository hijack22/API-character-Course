using System.Threading.Tasks;
using API_Course.Models;

namespace API_Course.Data
{
    public interface IAuthRepository
    {
         Task<serviceResponse<int>> Register(User user, string password);

         Task<serviceResponse<string>> Login (string username, string password);

         Task<bool> UserExists (string username);

         
    }
}