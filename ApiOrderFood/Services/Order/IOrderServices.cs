using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOrderFood.Services.Order
{
    public interface IOrderServices
    {
        Task<dynamic> GetAllMenu();
    }
}
