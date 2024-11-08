﻿using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Database.Context;

namespace Repository.Repository.Master
{
    public class AnswerRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        private readonly IQueryable<Answer> _q_QueryData;

        public AnswerRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            _q_QueryData = _dbcontext.Answers;
        }

        public async Task<List<Answer>> QueryAnswersAsync(Expression<Func<Answer, bool>> expression, string orderby, string orderdir, int take = 0, int skip = 0)
        {
            try
            {
                if (take > 0)
                {
                    return  _q_QueryData.Where(x => x.RowStatus == 0).Where(expression).OrderBy(orderby + " " + orderdir).Skip(skip).Take(take).ToList();
                }
                else
                {
                    return _q_QueryData.Where(x => x.RowStatus == 0).Where(expression).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> QueryAnswersCountAsync(Expression<Func<Answer, bool>> expression)
        {
            try
            {
                    return _q_QueryData.Where(x => x.RowStatus == 0).Count(expression);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Answer> GetAnswerByIDAsync(string ID)
        {
            try
            {
                return _q_QueryData.FirstOrDefault(x => x.ID == ID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Answer> GetAnswerByNameAsync(string Name)
        {
            try
            {
                return _q_QueryData.FirstOrDefault(x => x.Nama == Name);
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
                return _q_QueryData.Any(x => x.Nama == Nama && x.Alamat == Alamat && x.Usia == Usia && x.RowStatus == 0);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
