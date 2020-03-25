using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.API.Models;

namespace WebApplication1.API.User
{
    public interface IUserService
    {
        bool IsValid(LoginRequestDTO req);
    }
}
