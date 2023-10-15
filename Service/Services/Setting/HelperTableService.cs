using AutoMapper;
using DAL.Helpers;
using Database.JsonModels.Setting;
using Database.JsonModels;
using Model.Models;
using Repository.Repository.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static DAL.Helpers.GlobalHelpers;
using Service.Helpers;
using Model.JsonModels.Setting;

namespace Service.Services.Setting
{
    public class HelperTableService
    {
        HelperTableRepository HelperTableRepository { get; set; }

        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        public HelperTableService(ApplicationDbContext dbcontext, IMapper mapper)
        {
            HelperTableRepository = new HelperTableRepository(dbcontext);
            _dbcontext = dbcontext;
            _mapper = mapper;

        }
        public JsonHelperTable GetHelperByCode(string name, string code, string clientid)
        {
            try
            {

                return _mapper.Map<HelperTable, JsonHelperTable>(HelperTableRepository.GetHelperByCode(name, code, clientid));
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
                return HelperTableRepository.GetHelperTableValueByID(ID);
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
                return HelperTableRepository.GetHelperTableDescByID(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonHelperTable GetHelperTable(string id)
        {
            try
            {
                return _mapper.Map<HelperTable, JsonHelperTable>(HelperTableRepository.GetHelperTable(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public JsonHelperTable GetHelperTable(String ClientID, String ID, String Code)
        {
            try
            {
                return _mapper.Map<HelperTable, JsonHelperTable>(HelperTableRepository.GetHelperTableValue(ClientID, ID, Code));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonReturn SaveAsync(JsonHelperTable Save, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                String Error = "";
                if (String.IsNullOrEmpty(Save.ID))
                {
                    if (HelperTableRepository.GetHelperByCode(Save.Name, Save.Code, GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims)) != null) Error = "duplicate data";

                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = Error;

                }
                String ClientID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);
                String CreatedBy = GlobalHelpers.GetClaimValueByType(EnumClaims.Username.ToString(), claims);
                if (String.IsNullOrEmpty(Error))
                {
                    HelperTable NewData = _dbcontext.HelperTables.FirstOrDefault(x => x.ID == Save.ID);


                    if (NewData == null)
                    {
                        NewData = new HelperTable();
                        NewData.ID = Save.ID;
                        NewData.Code = Save.Code;
                        NewData.Name = Save.Name;
                        NewData.Value = Save.Value;
                        NewData.Description = Save.Description;
                        NewData.ClientID = ClientID;
                        NewData.SetRowStatus(0);
                        NewData.CreatedTime = DateTime.Now;
                        NewData.CreatedBy = CreatedBy;
                        NewData.LastModifiedBy = CreatedBy;
                        NewData.LastModifiedTime = DateTime.Now;
                        _dbcontext.HelperTables.Add(NewData);
                        _dbcontext.SaveChanges();


                    }
                    else
                    {



                        NewData.Code = Save.Code;
                        NewData.Name = Save.Name;
                        NewData.Code = Save.Code;
                        NewData.Name = Save.Name;
                        NewData.Value = Save.Value;
                        NewData.Description = Save.Description;
                        NewData.LastModifiedBy = CreatedBy;
                        NewData.LastModifiedTime = DateTime.Now;
                        _dbcontext.SaveChanges();

                    }
                    NewData = HelperTableRepository.GetHelperTable(NewData.ID);

                    jsonReturn = new JsonReturn(true);
                    jsonReturn.message = "Succes Save Data";
                    jsonReturn.ObjectValue = NewData;

                }

                return jsonReturn;

            }
            catch (Exception ex)
            {

                throw ex;

            }


        }

        public async Task<List<JsonHelperTable>> FindAsync(JsonHelperTable filter, ClaimsPrincipal claims)
        {
            List<JsonHelperTable> listHelperTable = new List<JsonHelperTable>();
            JsonReturn returnData = new JsonReturn(false);
            try
            {
                returnData = new JsonReturn(true);
                String ClientID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);
                filter.ClientID = ClientID;
                Expression<Func<HelperTable, bool>> filterExp = c => true;
                if (!String.IsNullOrEmpty(filter.ClientID)) filterExp = filterExp.And(x => x.ClientID == filter.ClientID);
                if (!String.IsNullOrEmpty(filter.ID))
                {
                    filterExp = filterExp.And(x => x.ID != null);
                    filterExp = filterExp.And(x => x.ID == filter.ID);
                }
                if (!String.IsNullOrEmpty(filter.Code))
                {
                    filterExp = filterExp.And(x => x.Code != null);
                    filterExp = filterExp.And(x => x.Code == filter.Code);
                }
                if (!String.IsNullOrEmpty(filter.Description))
                {
                    filterExp = filterExp.And(x => x.Description != null);
                    filterExp = filterExp.And(x => x.Description == filter.Description);
                }


                listHelperTable = _mapper.Map<IEnumerable<HelperTable>, List<JsonHelperTable>>(await HelperTableRepository.QueryHelperTablesAsync(filterExp, 0, 0));
                return listHelperTable;

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

    }
}
