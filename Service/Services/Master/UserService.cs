using AutoMapper;
using Database.JsonModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Model.JsonModels.Master;
using Model.Models;
using NPOI.SS.Formula.Functions;
using Repository.Repository.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Master
{
    public class UserService
    {
        UserRepository Repo;

        private UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserService(ApplicationDbContext dbContext, IMapper mapper, IConfiguration config, UserManager<User> userMgr)
        {
            _userManager = userMgr;
            _mapper = mapper;
            _config = config;
            Repo = new UserRepository(dbContext);
        }

        public JsonUser GetUserAsync(string email)
        {
            try
            {
                var User = _mapper.Map<User, JsonUser>(Repo.GetUserByEmail(email));
                if (User != null)
                {
                }
                return User;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public JsonReturn ChangePassword(JsonUser user, ClaimsPrincipal claims)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
