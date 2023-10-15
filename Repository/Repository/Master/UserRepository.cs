using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.Master
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        private readonly IQueryable<User> _q_QueryData;

        public UserRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            _q_QueryData = _dbcontext.Users;
        }


        public List<User> QueryUsers(Expression<Func<User, bool>> expression, int take = 0, int skip = 0)
        {

            try
            {

                if (take < 1)
                {
                    return _q_QueryData.Where(x => x.RowStatus == 0).Where(expression).ToList();
                }
                else
                {
                    return _q_QueryData.Where(x => x.RowStatus == 0).Where(expression).Skip(skip).Take(take).ToList(); ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public User GetUserByUsernamePassword(string Username, string Password)
        {
            try
            {
                return _q_QueryData.FirstOrDefault(x => x.UserName == Username && x.Password == Password);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public User GetUserByEmail(string email)
        {
            try
            {
                return _q_QueryData.FirstOrDefault(x => x.Email == email);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public User GetUserByIDData(string ID)
        {
            try
            {
                return _q_QueryData.FirstOrDefault(x => x.Id == ID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public User GetUserByUsername(string username)
        {
            try
            {
                return _q_QueryData.FirstOrDefault(x => x.UserName == username);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool IsUniqueKeyCodeExist(string Username, String ClientID)
        {
            return _q_QueryData.Where(x => x.RowStatus == 0).FirstOrDefault(x => x.UserName == Username && x.ClientID == ClientID) != null ? true : false;
        }
    }
}
