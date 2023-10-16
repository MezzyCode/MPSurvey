using AutoMapper;
using Database.JsonModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Model.JsonModels;
using Model.JsonModels.Master;
using Model.Models;
using NPOI.SS.Formula.Functions;
using Repository.Repository.Master;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Database.Context.HelperFunction;
using static Service.Helpers.GlobalHelpers;

namespace Service.Services.Master
{
    public class UserService
    {
        UserRepository Repo;

        private UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private UnitOfWork.UnitOfWork UnitOfWork;

        public UserService(ApplicationDbContext dbContext, IMapper mapper, IConfiguration config, UserManager<User> userMgr)
        {
            _userManager = userMgr;
            _mapper = mapper;
            _config = config;
            UnitOfWork = new UnitOfWork.UnitOfWork(dbContext);
            Repo = new UserRepository(dbContext);
        }

        public async Task<List<JsonUser>> FindAsync(JsonUser filter, ClaimsPrincipal claims)
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

        public async Task<int> FindCountAsync(JsonUser filter, ClaimsPrincipal claims)
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

        public async Task<JsonUser> GetUserAsync(string email)
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

        public async Task<JsonReturn> SaveAsync(JsonUser Save, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(false);

            try
            {
                String Error = "";
                if (String.IsNullOrEmpty(Save.ID))
                {
                    if (Repo.IsUniqueKeyCodeExist(Save.Username, GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims))) Error = "Username sudah terdaftar";

                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = Error;

                }
                if (String.IsNullOrEmpty(Save.Email))
                {
                    Error = "email is required";
                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = Error;

                }



                if (String.IsNullOrEmpty(Error))
                {
                    String KlienID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);
                    string UserID = GlobalHelpers.GetClaimValueByType(EnumClaims.ID.ToString(), claims);
                    User NewData = new User();

                    //var flagElsa = CreateUserElsa(Save, out Error);
                    //if (flagElsa == false)
                    //{
                    //    jsonReturn = new JsonReturn(false);
                    //    jsonReturn.message = "failed create user at workflow";
                    //    return jsonReturn;
                    //}
                    if (String.IsNullOrEmpty(Save.ID))
                    {
                        NewData = Repo.GetUserByEmail(Save.Email);
                        if (NewData != null)
                        {
                            Error = "Account with this email already exist";
                            jsonReturn = new JsonReturn(false);
                            jsonReturn.message = Error;

                            return jsonReturn;
                        }
                        NewData = new User();

                        //NewData.Id = Guid.NewGuid().ToString("N").ToUpper();
                        NewData.Id = Save.Email; // Make sure whether to use email or username or guid (the base code use guid, but the data in db use email, so does the get function)
                        NewData.UserName = Save.Username;
                        NewData.Password = GlobalHelpers.GetHash(Save.Password);
                        NewData.PasswordHash = GlobalHelpers.GetHash(Save.Password);

                        //NewData.UserRoleID = Save.UserRoleID;
                        //NewData.RoleID = Save.RoleID;
                        NewData.Name = Save.Name;
                        NewData.Email = Save.Email;
                        //NewData.IsAD = Save.IsAD;
                        NewData.ClientID = KlienID;

                        UnitOfWork.InsertOrUpdateUser(NewData, ObjectState.Added, claims);

                        var result = await _userManager.CreateAsync(NewData, Save.Password);

                        if (result.Succeeded)
                        {


                            UnitOfWork.Commit();


                            jsonReturn = new JsonReturn(true);
                            jsonReturn.message = "Succes Save Data";
                        }
                        else
                        {
                            jsonReturn = new JsonReturn(false);
                            foreach (var error in result.Errors)
                            {
                                jsonReturn.message += $"{error.Description}\\n";
                            }
                        }

                    }
                    if (!String.IsNullOrEmpty(Save.ID))
                    {
                        NewData = Repo.GetUserByIDData(Save.ID);
                        if (NewData == null)
                        {
                            jsonReturn = new JsonReturn(false);
                            jsonReturn.message = "User not found";
                            return jsonReturn;
                        }

                        //NewData.IsAD = Save.IsAD;
                        NewData.Password = GlobalHelpers.GetHash(Save.Password);
                        NewData.PasswordHash = GlobalHelpers.GetHash(Save.Password);

                        //NewData.UserRoleID = Save.UserRoleID;
                        //NewData.RoleID = Save.RoleID;
                        NewData.Name = Save.Name;

                        UnitOfWork.InsertOrUpdateUser(NewData, ObjectState.Modified, claims);
                        UnitOfWork.Commit();


                        jsonReturn = new JsonReturn(true);
                        jsonReturn.message = "Succes Save Data";
                    }
                }
            }
            catch (Exception ex)
            {
                jsonReturn = new JsonReturn(false);
                jsonReturn.message = ex.Message;
            }

            return jsonReturn;
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


        public async Task<JsonReturn> DeleteAsync(JsonUser SelectedUser, ClaimsPrincipal claims)
        {
            try
            {
                JsonReturn jsonReturn = new JsonReturn(false);


                User DelUser = Repo.GetUserByIDData(SelectedUser.ID);

                UnitOfWork.InsertOrUpdateUser(DelUser, ObjectState.SoftDelete, claims);

                var result = await _userManager.DeleteAsync(DelUser);

                if (result.Succeeded)
                {
                    UnitOfWork.Commit();

                    jsonReturn = new JsonReturn(true);
                    jsonReturn.message = "Succes Save Data";
                    return jsonReturn;
                }
                else
                {
                    jsonReturn = new JsonReturn(false);
                    foreach (var error in result.Errors)
                    {
                        jsonReturn.message += $"{error.Description}\\n";

                    }
                    return jsonReturn;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
