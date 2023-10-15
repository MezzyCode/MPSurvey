using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.Setting
{
    public class HelperTableRepository
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IQueryable<HelperTable> _q_QueryData;

        public HelperTableRepository (ApplicationDbContext ApplicationDbContext)
        {

            _dbContext = ApplicationDbContext;
            _q_QueryData = _dbContext.HelperTables;

        }

        #region query

        public async Task<List<HelperTable>> QueryHelperTablesAsync(Expression<Func<HelperTable, bool>> expression, int take = 0, int skip = 0)
        {

            try
            {
                if (take < 1)
                {
                    return _q_QueryData.Where(expression).ToList();
                }
                else
                {
                    return _q_QueryData.Where(expression).Skip(skip).Take(take).ToList(); ;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }








        }

        public HelperTable GetHelperByCode(string name, string code, string ClientId)
        {
            try
            {
                return _dbContext.HelperTables.FirstOrDefault(x => x.Name.ToUpper() == name.ToUpper() && x.Code == code && x.ClientID == ClientId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetHelperTableValueByID(string ID)
        {
            try
            {
                string result = "";
                var HelperTable = _dbContext.HelperTables.FirstOrDefault(x => x.ID == ID);
                if (HelperTable != null)
                {
                    result = HelperTable.Value;
                }


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetHelperTableDescByID(string ID)
        {
            try
            {
                string result = "";
                var HelperTable = _dbContext.HelperTables.FirstOrDefault(x => x.ID == ID);
                if (HelperTable != null)
                {
                    result = HelperTable.Description;
                }


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public HelperTable GetHelperTable(string id)
        {
            try
            {
                var result = _dbContext.HelperTables.FirstOrDefault(x => x.ID.ToUpper() == id.ToUpper());
                return (result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public HelperTable GetHelperTableValue(string ClientId, string ID, String Code)
        {
            try
            {
                var query = _q_QueryData.Where(x => x.ClientID == ClientId);
                if (!String.IsNullOrEmpty(ID))
                {
                    query = query.Where(x => x.ID == ID);
                }
                if (!String.IsNullOrEmpty(Code))
                {
                    query = query.Where(x => x.Code == Code);
                }



                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }








        #endregion
    }
}

