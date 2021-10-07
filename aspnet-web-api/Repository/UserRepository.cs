using aspnet_web_api.Context;
using aspnet_web_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_web_api.Repository
{
    public class UserRepository
    {
        UserContext context;
        public UserRepository()
        {
            context = new UserContext();
        }

        public List<UserViewModel> GetAll()
        {
            return context.GetAll();
        }

        public List<UserViewModel> GetById(int id)
        {
            return context.GetById(id);
        }
    }
}
