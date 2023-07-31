using System.Threading.Tasks;
using ApiOrderFood.DTO;
using ApiOrderFood.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiOrderFood.Services.Order
{
    public class OrderServices : IOrderServices
    {
        private readonly DatabaseContext _context;
        public OrderServices(DatabaseContext context)
        {
            _context = context;
        }

        async Task<dynamic> IOrderServices.GetAllMenu()
        {
            return await _context.Menu.Select(x => new MenuDTO { Name = x.Name, Description = x.Description, Price = x.Price, Status = x.Status, Id = x.Id }).ToListAsync();
        }
    }
}
