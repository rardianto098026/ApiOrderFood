using System.Threading.Tasks;
using ApiOrderFood.DTO;
using ApiOrderFood.Models;

namespace ApiOrderFood.Services.User
{
    public class UsersServices : IUsersServices
    {
        private readonly DatabaseContext _context;
        public UsersServices(DatabaseContext context)
        {
            _context = context;
        }
    }
}
