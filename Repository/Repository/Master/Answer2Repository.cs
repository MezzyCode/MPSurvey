using Microsoft.EntityFrameworkCore;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.Master
{
    public class Answer2Repository
    {
        private readonly ApplicationDbContext _dbcontext;

        private readonly IQueryable<Answer2> _q_QueryData;

        public Answer2Repository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            _q_QueryData = _dbcontext.Answer2s;
        }

        public async Task<List<Answer2>> QueryAnswer2sAsync(Expression<Func<Answer2, bool>> expression, string orderby, string orderdir, int take = 0, int skip = 0)
        {
            try
            {
                if (take > 0)
                {
                    return await _q_QueryData.Where(x => x.RowStatus == 0).Where(expression).OrderBy(orderby + " " + orderdir).Skip(skip).Take(take).ToListAsync();
                }
                else
                {
                    return await _q_QueryData.Where(x => x.RowStatus == 0).Where(expression).OrderBy(orderby + " " + orderdir).ToListAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> QueryAnswer2sCountAsync(Expression<Func<Answer2, bool>> expression)
        {
            try
            {
                return await _q_QueryData.Where(x => x.RowStatus == 0).CountAsync(expression);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Answer2> GetAnswer2ByIDAsync(string ID)
        {
            try
            {
                return await _q_QueryData.FirstOrDefaultAsync(x => x.ID == ID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Answer2> GetAnswer2ByNameAsync(string Name)
        {
            try
            {
                return await _q_QueryData.FirstOrDefaultAsync(x => x.Nama == Name);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> IsUniqueKeyCodeExist(string Nama, string Alamat, int? Usia)
        {
            try
            {
                return await _q_QueryData.AnyAsync(x => x.Nama == Nama && x.Alamat == Alamat && x.Usia == Usia && x.RowStatus == 0);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
