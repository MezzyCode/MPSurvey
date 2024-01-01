using AutoMapper;
using ClosedXML.Excel;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using Model.JsonModels;
using Model.JsonModels.Master;
using Model.JsonModels.Setting;
using Model.Models;
using NPOI.SS.Formula.Functions;
using Repository.Repository.Master;
using Service.Helpers;
using Service.Services.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Database.Context.HelperFunction;
using ConstantVariableKey = Model.InfrastructurClass.ConstantVariable;
using static Service.Helpers.GlobalHelpers;
using System.Data;

namespace Service.Services.Master
{
    public class Answer2Service
    {
        Answer2Repository Repo;

        private readonly ApplicationDbContext _dbcontext;
        private UnitOfWork.UnitOfWork UnitOfWork;
        private readonly IMapper _mapper;
        HelperTableService ServiceHelper;

        public Answer2Service(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            UnitOfWork = new UnitOfWork.UnitOfWork(dbcontext);
            ServiceHelper = new HelperTableService(dbcontext, mapper);
            Repo = new Answer2Repository(dbcontext);
        }

        public async Task<JsonReturn> GenerateData(string filename, string fileLogName, ClaimsPrincipal claims)
        {
            JsonReturn Result = new JsonReturn(false);
            try
            {
                GlobalHelpers.ReGenerateThreadClaim(claims);

                Result = await GenerateImport(filename, fileLogName, claims);

            }
            catch (Exception ex)
            {
                Result = new JsonReturn(false);
                Result.message = ex.Message;
            }
            return Result;
        }
        public async Task<JsonReturn> GenerateImport(String NamaFile, string FileLogName, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(true);
            try
            {
                String ClientID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);

                FileInfo fileInfo = new FileInfo(NamaFile);

                bool isError = false;

                var dataCalon = await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CALONCODE }, claims);

                DataTable DTGENERATE = ExcelHelper.GetRequestsDataFromExcel(NamaFile, 0);

                if (DTGENERATE != null && DTGENERATE.Rows.Count > 0)
                {
                    if (ExcelHelper.ContainColumn("Nama", DTGENERATE)
                        && ExcelHelper.ContainColumn("No. Telp", DTGENERATE)
                        && ExcelHelper.ContainColumn("Usia", DTGENERATE)
                        && ExcelHelper.ContainColumn("NIK", DTGENERATE)
                        && ExcelHelper.ContainColumn("Alamat", DTGENERATE)
                        && ExcelHelper.ContainColumn("RT", DTGENERATE)
                        && ExcelHelper.ContainColumn("RW", DTGENERATE)
                        && ExcelHelper.ContainColumn("Kecamatan", DTGENERATE)
                        && ExcelHelper.ContainColumn("Kelurahan", DTGENERATE)
                        && ExcelHelper.ContainColumn("Simpul", DTGENERATE)
                        && ExcelHelper.ContainColumn("C1", DTGENERATE)
                        && ExcelHelper.ContainColumn("C2", DTGENERATE)
                        && ExcelHelper.ContainColumn("C3", DTGENERATE)
                        && ExcelHelper.ContainColumn("C4", DTGENERATE)
                        )
                    {

                        List<JsonFileLogDetail> logDetail = new List<JsonFileLogDetail>();

                        for (int i = 0; i < DTGENERATE.Rows.Count; i++)
                        {

                            JsonFileLogDetail jsonFileLogDetail = new JsonFileLogDetail();
                            jsonFileLogDetail.OrderNo = i + 2;
                            jsonFileLogDetail.Status = true;

                            String? Nama = DTGENERATE.Rows[i]["Nama"].ToString();
                            String? NoTelp = DTGENERATE.Rows[i]["No. Telp"].ToString();
                            String? Usia = DTGENERATE.Rows[i]["Usia"].ToString();
                            String? NIK = DTGENERATE.Rows[i]["NIK"].ToString();
                            String? Alamat = DTGENERATE.Rows[i]["Alamat"].ToString();
                            String? RT = DTGENERATE.Rows[i]["RT"].ToString();
                            String? RW = DTGENERATE.Rows[i]["RW"].ToString();
                            String? Kecamatan = DTGENERATE.Rows[i]["Kecamatan"].ToString();
                            String? Kelurahan = DTGENERATE.Rows[i]["Kelurahan"].ToString();
                            String? Simpul = DTGENERATE.Rows[i]["Simpul"].ToString();
                            String? C1 = DTGENERATE.Rows[i]["C1"].ToString();
                            String? C2 = DTGENERATE.Rows[i]["C2"].ToString();
                            String? C3 = DTGENERATE.Rows[i]["C3"].ToString();
                            String? C4 = DTGENERATE.Rows[i]["C4"].ToString();

                            List<JsonAnswer2> Answer2_temp = new List<JsonAnswer2>();
                            String Remarks = "";
                            if (String.IsNullOrEmpty(Nama))
                            {
                                Remarks += "Kolom Nama Harus diisi";
                            }
                            if (String.IsNullOrEmpty(NoTelp))
                            {
                                Remarks += "Kolom No. Telp Harus diisi";
                            }
                            if (String.IsNullOrEmpty(Alamat))
                            {
                                Remarks += "Kolom Alamat Harus diisi";
                            }
                            if (String.IsNullOrEmpty(RW))
                            {
                                Remarks += "Kolom RW Harus diisi";
                            }
                            if (String.IsNullOrEmpty(Kelurahan))
                            {
                                Remarks += "Kolom Kelurahan Harus diisi";
                            }
                            if (String.IsNullOrEmpty(Kecamatan))
                            {
                                Remarks += "Kolom Kecamatan Harus diisi";
                            }
                            if (String.IsNullOrEmpty(Simpul))
                            {
                                Remarks += "Kolom Simpul Harus diisi";
                            }
                            if (String.IsNullOrEmpty(C1))
                            {
                                Remarks += "Kolom C1 Harus diisi";
                            }
                            if (String.IsNullOrEmpty(C2))
                            {
                                Remarks += "Kolom C2 Harus diisi";
                            }
                            if (String.IsNullOrEmpty(C3))
                            {
                                Remarks += "Kolom C3 Harus diisi";
                            }
                            if (String.IsNullOrEmpty(C4))
                            {
                                Remarks += "Kolom C4 Harus diisi";
                            }

                            if (String.IsNullOrEmpty(Remarks))
                            {
                                try
                                {
                                    int? UsiaInt = null;
                                    if (!String.IsNullOrEmpty(Usia)) UsiaInt = int.Parse(Usia);

                                    if (await Repo.IsUniqueKeyCodeExist(Nama, Alamat, UsiaInt))
                                    {
                                        jsonFileLogDetail.Remarks = "Data sudah dimasukkan";
                                        jsonFileLogDetail.Status = false;
                                        logDetail.Add(jsonFileLogDetail);

                                        continue;
                                    }

                                    Answer2 NewData = new Answer2();
                                    NewData.ID = Guid.NewGuid().ToString();
                                    NewData.Nama = Nama;
                                    NewData.Usia = UsiaInt;
                                    NewData.Alamat = Alamat;
                                    NewData.NoTelp = NoTelp;
                                    NewData.Rt = RT;
                                    NewData.Rw = RW;
                                    NewData.Kecamatan = Kecamatan;
                                    NewData.Kelurahan = Kelurahan;
                                    NewData.C1 = C1;
                                    NewData.C2 = C2;
                                    NewData.C3 = C3;
                                    NewData.C4 = C4;

                                    NewData.ModelState = ObjectState.Added;

                                    UnitOfWork.InsertOrUpdate(claims, NewData);
                                    UnitOfWork.Commit();

                                    jsonFileLogDetail.Remarks = "Berhasil Menambahkan Jawaban";
                                    jsonFileLogDetail.Status = true;
                                }
                                catch (Exception ex)
                                {
                                    var Entries = _dbcontext.ChangeTracker.Entries<IEntity>().ToList();

                                    foreach (var entry in Entries)
                                    {
                                        entry.State = EntityState.Detached;
                                    }

                                    jsonFileLogDetail.Remarks = ex.Message;
                                    jsonFileLogDetail.Status = false;
                                    logDetail.Add(jsonFileLogDetail);

                                    isError = true;
                                    continue;
                                }
                            }
                            else
                            {
                                jsonFileLogDetail.Remarks = Remarks;
                                jsonFileLogDetail.Status = false;

                                isError = true;
                            }

                            logDetail.Add(jsonFileLogDetail);

                        }

                        if (!String.IsNullOrEmpty(FileLogName))
                            ExcelHelper.GenerateExcelFileFromFileLogDetail(FileLogName, logDetail);

                    }
                    else
                    {
                        var remarks = "Format tidak sesuai";
                        jsonReturn = new JsonReturn(false);
                        jsonReturn.message = remarks;
                    }

                    if (isError)
                    {
                        jsonReturn = new JsonReturn(false);
                        jsonReturn.message = "Terdapat beberapa error data, mohon cek log!";
                    }
                    else
                    {
                        jsonReturn = new JsonReturn(true);
                        jsonReturn.message = "File berhasil ditambahkan, mohon cek log!";
                    }
                }
            }
            catch (Exception ex)
            {
                var remarks = ex.Message;
                jsonReturn = new JsonReturn(false);
                jsonReturn.message = remarks;
            }
            return jsonReturn;
        }

        public async Task<List<JsonChart>> FindC1Async(JsonAnswer2 filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer2> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C1).Select(x => new JsonChart
                {
                    Label = x.Key,
                    Count = x.Count()
                }).ToList();

                List<JsonChart> listChart = new List<JsonChart>();

                List<string> list2Choice = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE2CODE }, claims)).OrderBy(x => x.Description).Select(x => x.Value).ToList();

                foreach (var choice in list2Choice)
                {
                    JsonChart newChart = new JsonChart();
                    newChart.Label = choice.ToString();
                    newChart.Count = 0;

                    var cekList = grouped.FirstOrDefault(x => x.Label == choice);
                    if (cekList != null) newChart.Count = cekList.Count;

                    listChart.Add(newChart);
                }

                return listChart;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<JsonChart>> FindC2Async(JsonAnswer2 filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer2> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C2).Select(x => new JsonChart
                {
                    Label = x.Key,
                    Count = x.Count()
                }).ToList();

                List<JsonChart> listChart = new List<JsonChart>();

                List<string> list2Choice = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE2CODE }, claims)).OrderBy(x => x.Description).Select(x => x.Value).ToList();

                foreach (var choice in list2Choice)
                {
                    JsonChart newChart = new JsonChart();
                    newChart.Label = choice.ToString();
                    newChart.Count = 0;

                    var cekList = grouped.FirstOrDefault(x => x.Label == choice);
                    if (cekList != null) newChart.Count = cekList.Count;

                    listChart.Add(newChart);
                }

                return listChart;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<JsonChart>> FindC3Async(JsonAnswer2 filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer2> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C3).Select(x => new JsonChart
                {
                    Label = x.Key,
                    Count = x.Count()
                }).ToList();

                List<JsonChart> listChart = new List<JsonChart>();

                List<string> list2Choice = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE2CODE }, claims)).OrderBy(x => x.Description).Select(x => x.Value).ToList();

                foreach (var choice in list2Choice)
                {
                    JsonChart newChart = new JsonChart();
                    newChart.Label = choice.ToString();
                    newChart.Count = 0;

                    var cekList = grouped.FirstOrDefault(x => x.Label == choice);
                    if (cekList != null) newChart.Count = cekList.Count;

                    listChart.Add(newChart);
                }

                return listChart;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<JsonChart>> FindC4Async(JsonAnswer2 filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer2> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C4).Select(x => new JsonChart
                {
                    Label = x.Key,
                    Count = x.Count()
                }).ToList();

                List<JsonChart> listChart = new List<JsonChart>();

                List<string> list3Choice = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE3CODE }, claims)).OrderBy(x => x.Description).Select(x => x.Value).ToList();

                foreach (var choice in list3Choice)
                {
                    JsonChart newChart = new JsonChart();
                    newChart.Label = choice.ToString();
                    newChart.Count = 0;

                    var cekList = grouped.FirstOrDefault(x => x.Label == choice);
                    if (cekList != null) newChart.Count = cekList.Count;

                    listChart.Add(newChart);
                }

                return listChart;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<JsonAnswer2>> FindAsync(JsonAnswer2 filter, ClaimsPrincipal claims)
        {
            List<JsonAnswer2> Answer2List = new List<JsonAnswer2>();

            try
            {
                String ClientID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);
                filter.ClientID = ClientID;
                Expression<Func<Answer2, bool>> filterExp = c => true;
                if (!String.IsNullOrEmpty(filter.Nama)) filterExp = filterExp.And(x => x.Nama.StartsWith(filter.Nama));
                if (!String.IsNullOrEmpty(filter.Kelurahan)) filterExp = filterExp.And(x => x.Kelurahan == filter.Kelurahan);
                if (!String.IsNullOrEmpty(filter.Kecamatan)) filterExp = filterExp.And(x => x.Kecamatan == filter.Kecamatan);
                filterExp = filterExp.And(x => x.ClientID == filter.ClientID);

                Answer2List = _mapper.Map<IEnumerable<Answer2>, List<JsonAnswer2>>(await Repo.QueryAnswer2sAsync(filterExp, filter.OrderBy, filter.OrderByDirection, filter.Take.GetValueOrDefault(), filter.Skip.GetValueOrDefault()));

                return Answer2List;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> FindCountAsync(JsonAnswer2 filter, ClaimsPrincipal claims)
        {
            try
            {
                String ClientID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);
                filter.ClientID = ClientID;
                Expression<Func<Answer2, bool>> filterExp = c => true;
                if (!String.IsNullOrEmpty(filter.Nama)) filterExp = filterExp.And(x => x.Nama.StartsWith(filter.Nama));
                if (!String.IsNullOrEmpty(filter.Kelurahan)) filterExp = filterExp.And(x => x.Kelurahan == filter.Kelurahan);
                if (!String.IsNullOrEmpty(filter.Kecamatan)) filterExp = filterExp.And(x => x.Kecamatan == filter.Kecamatan);
                filterExp = filterExp.And(x => x.ClientID == filter.ClientID);

                int TotalCount = await Repo.QueryAnswer2sCountAsync(filterExp);

                return TotalCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<JsonAnswer2>> FindByUserAsync(JsonAnswer2 filter, ClaimsPrincipal claims)
        {
            List<JsonAnswer2> Answer2List = new List<JsonAnswer2>();

            try
            {
                String ClientID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);
                filter.ClientID = ClientID;
                Expression<Func<Answer2, bool>> filterExp = c => true;
                if (!String.IsNullOrEmpty(filter.Nama)) filterExp = filterExp.And(x => x.Nama.StartsWith(filter.Nama));
                if (!String.IsNullOrEmpty(filter.Kelurahan)) filterExp = filterExp.And(x => x.Kelurahan == filter.Kelurahan);
                if (!String.IsNullOrEmpty(filter.Kecamatan)) filterExp = filterExp.And(x => x.Kecamatan == filter.Kecamatan);
                var CreatedBy = GlobalHelpers.GetClaimValueByType(EnumClaims.Username.ToString(), claims);
                filterExp = filterExp.And(x => x.CreatedBy == CreatedBy);
                filterExp = filterExp.And(x => x.ClientID == filter.ClientID);

                Answer2List = _mapper.Map<IEnumerable<Answer2>, List<JsonAnswer2>>(await Repo.QueryAnswer2sAsync(filterExp, filter.OrderBy, filter.OrderByDirection, filter.Take.GetValueOrDefault(), filter.Skip.GetValueOrDefault()));

                return Answer2List;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> FindCountByUserAsync(JsonAnswer2 filter, ClaimsPrincipal claims)
        {
            try
            {
                String ClientID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);
                filter.ClientID = ClientID;
                Expression<Func<Answer2, bool>> filterExp = c => true;
                if (!String.IsNullOrEmpty(filter.Nama)) filterExp = filterExp.And(x => x.Nama.StartsWith(filter.Nama));
                if (!String.IsNullOrEmpty(filter.Kelurahan)) filterExp = filterExp.And(x => x.Kelurahan == filter.Kelurahan);
                if (!String.IsNullOrEmpty(filter.Kecamatan)) filterExp = filterExp.And(x => x.Kecamatan == filter.Kecamatan);
                var CreatedBy = GlobalHelpers.GetClaimValueByType(EnumClaims.Username.ToString(), claims);
                filterExp = filterExp.And(x => x.CreatedBy == CreatedBy);
                filterExp = filterExp.And(x => x.ClientID == filter.ClientID);

                int TotalCount = await Repo.QueryAnswer2sCountAsync(filterExp);

                return TotalCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<JsonAnswer2> GetAnswer2(string ID)
        {
            try
            {
                return _mapper.Map<Answer2, JsonAnswer2>(await Repo.GetAnswer2ByIDAsync(ID));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<JsonReturn> SaveAsync(JsonAnswer2 Save, ClaimsPrincipal claims)
        {
            JsonReturn result = new JsonReturn(false);

            try
            {
                String ClientID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);


                string Error = "";

                if (String.IsNullOrEmpty(Error))
                {
                    Answer2 NewData = new Answer2();

                    if (String.IsNullOrEmpty(Save.ID))
                    {
                        //crudLog.ProcessName = "Create";

                        NewData.ID = Guid.NewGuid().ToString();
                        NewData.Nama = Save.Nama;
                        NewData.Usia = Save.Usia;
                        NewData.Alamat = Save.Alamat;

                        while (Save.Rw.Length < 3) Save.Rw = "0" + Save.Rw;
                        while (Save.Rt != null && Save.Rt.Length < 3) Save.Rt = "0" + Save.Rt;

                        NewData.Rt = Save.Rt;
                        NewData.Rw = Save.Rw;
                        NewData.Kelurahan = Save.Kelurahan;
                        NewData.Kecamatan = Save.Kecamatan;
                        NewData.NIK = Save.NIK;
                        NewData.NoTelp = Save.NoTelp;
                        NewData.Simpul = Save.Simpul;
                        NewData.C1 = Save.C1;
                        NewData.C2 = Save.C2;
                        NewData.C3 = Save.C3;
                        NewData.C4 = Save.C4;
                        NewData.SetRowStatus(0);

                        NewData.ModelState = ObjectState.Added;

                    }
                    if (!String.IsNullOrEmpty(Save.ID))
                    {
                        //crudLog.ProcessName = "Update";

                        NewData = await Repo.GetAnswer2ByIDAsync(Save.ID);

                        if (NewData == null)
                        {
                            result = new JsonReturn(false);
                            result.message = "Data tidak ditemukan";
                            return result;
                        }

                        NewData.Nama = Save.Nama;
                        NewData.Usia = Save.Usia;
                        NewData.Alamat = Save.Alamat;

                        while (Save.Rw.Length < 3) Save.Rw = "0" + Save.Rw;
                        while (Save.Rt != null && Save.Rt.Length < 3) Save.Rt = "0" + Save.Rt;

                        NewData.Rt = Save.Rt;
                        NewData.Rw = Save.Rw;
                        NewData.Kelurahan = Save.Kelurahan;
                        NewData.Kecamatan = Save.Kecamatan;
                        NewData.NIK = Save.NIK;
                        NewData.NoTelp = Save.NoTelp;
                        NewData.Simpul = Save.Simpul;
                        NewData.C1 = Save.C1;
                        NewData.C2 = Save.C2;
                        NewData.C3 = Save.C3;
                        NewData.C4 = Save.C4;

                        NewData.ModelState = ObjectState.Modified;

                    }

                    //crudLog.Data = JsonConvert.SerializeObject(NewData);
                    //GlobalHelpers.InsertCrudLog(crudLog, _dbcontext.Database.GetConnectionString(), claims);

                    UnitOfWork.InsertOrUpdate(claims, NewData);
                    UnitOfWork.Commit();

                    result = new JsonReturn(true);
                    result.message = "Berhasil diinput";
                }
            }
            catch (Exception ex)
            {
                result = new JsonReturn(false);
                result.message = ex.Message;
            }

            return result;
        }

        public async Task<JsonReturn> DeleteAsync(string ID)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                Answer2 data = await Repo.GetAnswer2ByIDAsync(ID);

                if (data != null)
                {
                    _dbcontext.Answer2s.Remove(data);
                    _dbcontext.SaveChanges();

                    jsonReturn = new JsonReturn(true);
                    jsonReturn.message = "Berhasil Menghapus Data!";
                }
                else
                {
                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = "Data Tidak Ditemukan!";
                }
            }
            catch (Exception ex)
            {
                jsonReturn = new JsonReturn(false);
                jsonReturn.message = ex.Message;
            }
            return jsonReturn;
        }

        public async Task<JsonReturn> SoftDeleteAsync(string ID, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                Answer2 data = await Repo.GetAnswer2ByIDAsync(ID);

                if (data != null)
                {
                    data.SetRowStatus(RowStatus.Deleted);

                    data.ModelState = ObjectState.Modified;

                    UnitOfWork.InsertOrUpdate(claims, data);
                    UnitOfWork.Commit();

                    jsonReturn = new JsonReturn(true);
                    jsonReturn.message = "Berhasil Menghapus Data!";
                }
                else
                {
                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = "Data Tidak Ditemukan!";
                }
            }
            catch (Exception ex)
            {
                jsonReturn = new JsonReturn(false);
                jsonReturn.message = ex.Message;
            }
            return jsonReturn;
        }

        public async Task<byte[]> DownloadExcelDocument(JsonAnswer2 filter, ClaimsPrincipal claims)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Hasil Survey");
                    worksheet.Cell(1, 1).Value = "Nama";
                    worksheet.Cell(1, 2).Value = "Usia";
                    worksheet.Cell(1, 3).Value = "NIK";
                    worksheet.Cell(1, 4).Value = "No. Telp";
                    worksheet.Cell(1, 5).Value = "Alamat";
                    worksheet.Cell(1, 6).Value = "Nama Simpul";
                    worksheet.Cell(1, 7).Value = "C1";
                    worksheet.Cell(1, 8).Value = "C2";
                    worksheet.Cell(1, 9).Value = "C3";
                    worksheet.Cell(1, 10).Value = "C4";

                    List<JsonAnswer2> datas = await FindAsync(filter, claims);

                    for (int index = 1; index < datas.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = datas[index - 1].Nama;
                        worksheet.Cell(index + 1, 2).Value = datas[index - 1].Usia;
                        worksheet.Cell(index + 1, 3).Value = datas[index - 1].NIK;
                        worksheet.Cell(index + 1, 4).Value = datas[index - 1].NoTelp;
                        worksheet.Cell(index + 1, 5).Value = $"{datas[index - 1].Alamat}, RT {datas[index - 1].Rt} RW {datas[index - 1].Rw}, {datas[index - 1].Kelurahan} {datas[index - 1].Kecamatan}";
                        worksheet.Cell(index + 1, 6).Value = datas[index - 1].Simpul;
                        worksheet.Cell(index + 1, 7).Value = datas[index - 1].C1;
                        worksheet.Cell(index + 1, 8).Value = datas[index - 1].C2;
                        worksheet.Cell(index + 1, 9).Value = datas[index - 1].C3;
                        worksheet.Cell(index + 1, 10).Value = datas[index - 1].C4;
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return content;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
