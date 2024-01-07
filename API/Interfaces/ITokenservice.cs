using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities; // Replace with the actual namespace of AppUser

namespace API.Interfaces
{
    public interface ITokenservice
    {
        string CreateToken(AppUser user);
    }
}