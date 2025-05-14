using DataAccesLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesLayer.RepositoriesInterface
{
    public interface IUserRepository : IGenericRepository<Users>
    {
        public Task<Users> GetUserByEmailPass(string email, string password);
        public Task<Users> GetUserByEmail(string email);
    }
}
