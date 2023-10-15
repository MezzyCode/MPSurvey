using DAL.Helpers;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Database.Context.HelperFunction;

namespace Service.UnitOfWork
{
    public class UnitOfWork
    {


        private readonly ApplicationDbContext _centralizedDbContext;
        private bool _disposed = false;


        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _centralizedDbContext = dbContext;

        }

        public void InsertOrUpdateUser(User user, ObjectState objectState, ClaimsPrincipal User)
        {
            try
            {
                System.Threading.Thread.CurrentPrincipal = User;
                if (objectState == ObjectState.SoftDelete)
                {
                    _centralizedDbContext.Users.Remove(user);
                }
                else
                {


                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                    // Get the claims values
                    var name = identity.Claims.Where(c => c.Type == GlobalHelpers.EnumClaims.Username.ToString())
                                       .Select(c => c.Value).SingleOrDefault();
                    var empName = identity.Claims.Where(c => c.Type == GlobalHelpers.EnumClaims.Nama.ToString())
                                      .Select(c => c.Value).SingleOrDefault();
                    var empID = identity.Claims.Where(c => c.Type == GlobalHelpers.EnumClaims.Email.ToString()).Select(c => c.Value).SingleOrDefault();

                    var ClientID = identity.Claims.Where(c => c.Type == GlobalHelpers.EnumClaims.ClientID.ToString())
                                       .Select(c => c.Value).SingleOrDefault();

                    DateTime now = DateTime.Now;


                    user.LastModifiedBy = name;
                    user.LastModifiedTime = DateTime.Now;
                    if (objectState == ObjectState.Added)
                    {
                        user.CreatedBy = name;
                        user.CreatedTime = DateTime.Now;
                        user.ClientID = ClientID;

                        _centralizedDbContext.Users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void InsertOrUpdate<T>(ClaimsPrincipal User, params T[] entities) where T : EntityBase
        {
            try
            {
                System.Threading.Thread.CurrentPrincipal = User;
                foreach (T item in entities)
                {
                    if (item.ModelState == ObjectState.SoftDelete)
                    {
                        _centralizedDbContext.Remove(item);
                    }
                    else
                    {
                        var entry = _centralizedDbContext.Entry(item);
                        entry.State = HelperFunction.ConvertState(item.ModelState);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }



        public object Commit()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            try
            {
                object varA;


                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                // Get the claims values
                var name = identity.Claims.Where(c => c.Type == GlobalHelpers.EnumClaims.Username.ToString())
                                   .Select(c => c.Value).SingleOrDefault();
                var ClientID = identity.Claims.Where(c => c.Type == GlobalHelpers.EnumClaims.ClientID.ToString())
                                   .Select(c => c.Value).SingleOrDefault();

                DateTime now = DateTime.Now;

                var cekdata = _centralizedDbContext.ChangeTracker.Entries<IEntity>().ToList();

                foreach (var entry in cekdata.ToList()
                    .Where(c => c.Entity.ModelState != ObjectState.Processed
                        && c.Entity.ModelState != ObjectState.Unchanged))
                {
                    if (entry.Entity.IsTrackedId)
                        varA = entry.Entity;

                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy = name;
                        entry.Entity.CreatedTime = DateTime.Now;
                        entry.Entity.ClientID = ClientID;
                        entry.Entity.LastModifiedBy = name;
                        entry.Entity.LastModifiedTime = DateTime.Now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Property(x => x.CreatedBy).IsModified = false;
                        entry.Property(x => x.CreatedTime).IsModified = false;
                        entry.Entity.LastModifiedBy = name;

                        entry.Entity.ClientID = ClientID;
                        entry.Entity.LastModifiedTime = DateTime.Now;
                    }


                    entry.Entity.ModelState = ObjectState.Processed;
                }
                var totalProcessed = _centralizedDbContext.SaveChanges();

                return totalProcessed;
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing && _centralizedDbContext != null)
            {
                _centralizedDbContext.Dispose();
            }

            _disposed = true;
        }

        public void InsertOrUpdate()
        {
            throw new NotImplementedException();
        }



    }
}
