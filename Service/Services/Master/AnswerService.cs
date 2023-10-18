using AutoMapper;
using ClosedXML.Excel;
using Service.Helpers;
using Model.JsonModels;
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
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Database.Context.HelperFunction;
using ConstantVariableKey = Model.InfrastructurClass.ConstantVariable;
using static Service.Helpers.GlobalHelpers;

namespace Service.Services.Master
{
    public class AnswerService
    {
        AnswerRepository Repo;

        private readonly ApplicationDbContext _dbcontext;
        private UnitOfWork.UnitOfWork UnitOfWork;
        private readonly IMapper _mapper;
        HelperTableService ServiceHelper;

        public AnswerService(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            UnitOfWork = new UnitOfWork.UnitOfWork(dbcontext);
            ServiceHelper = new HelperTableService(dbcontext, mapper);
            Repo = new AnswerRepository(dbcontext);
        }

        public async Task<List<JsonChart>> FindChartAsync(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer> answerList = await FindAsync(filter, claims);

                List<string> listCalon = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CALONCODE }, claims)).OrderBy(x => x.Description).Select(x => x.Value).ToList();

                var grouped = answerList.GroupBy(x => x.C6).Select(x => new JsonChart
                {
                    Label = x.Key,
                    Count = x.Count()
                }).ToList();

                List<JsonChart> listChart = new List<JsonChart>();

                foreach (var calon in listCalon)
                {
                    JsonChart newChart = new JsonChart();
                    newChart.Label = calon;
                    newChart.Count = 0;

                    var thisCalon = grouped.FirstOrDefault(x => x.Label == calon);
                    if (thisCalon != null) newChart.Count = thisCalon.Count;

                    listChart.Add(newChart);
                }


                return listChart.OrderBy(x => x.Label).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public async Task<List<JsonChartLabel>> FindChartReligionAsync(JsonAnswer filter, ClaimsPrincipal claims)
        //{
        //    try
        //    {
        //        List<JsonAnswer> answerList = await FindAsync(filter, claims);

        //        //List<JsonHelperTable> listAgamaHelper = await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.AGAMACODE }, claims);
        //        List<string> listCalon = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CALONCODE }, claims)).OrderBy(x => x.Description).Select(x => x.Value).ToList();

        //        List<string> listAgama = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.AGAMACODE }, claims)).OrderBy(x => x.Description).Select(x => x.Value).ToList();

        //        var grouped = answerList.GroupBy(x => new { x.C6, x.C8 }).Select(x => new
        //        {
        //            Religion = x.Key.C8,
        //            Label = x.Key.C6,
        //            Count = x.Count()
        //        }).ToList();

        //        List<JsonChartLabel> listChartLabel = new List<JsonChartLabel>();

        //        foreach (var calon in listCalon)
        //        {
        //            JsonChartLabel newChartLabel = new JsonChartLabel();
        //            newChartLabel.XLabel = calon;

        //            var groupedbyCalon = grouped.Where(x => x.Label == calon).OrderBy(x => x.Religion).ToList();

        //            foreach (var agama in listAgama)
        //            {
        //                JsonChart newChart = new JsonChart();
        //                newChart.Label = agama;
        //                newChart.Count = 0;

        //                var calonAgama = groupedbyCalon.FirstOrDefault(x => x.Religion == agama);
        //                if (calonAgama != null) newChart.Count = calonAgama.Count;

        //                newChartLabel.Charts.Add(newChart);
        //            }

        //            listChartLabel.Add(newChartLabel);
        //        }

        //        return listChartLabel.OrderBy(x => x.XLabel).ToList();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public async Task<List<JsonChartLabel>> FindC3Async(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer> answerList = await FindAsync(filter, claims);

                //List<JsonHelperTable> listAgamaHelper = await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.AGAMACODE }, claims);
                List<string> listChoice = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE3CODE }, claims)).OrderBy(x => x.Description).Select(x => x.Value).ToList();

                List<JsonChart> groupedChart = new List<JsonChart>();

                var groupedC3A = answerList.GroupBy(x => x.C3A).Select(x => new JsonChart
                {
                    Label = x.Key,
                    Count = x.Count()
                }).ToList();

                var groupedC3B = answerList.GroupBy(x => x.C3B).Select(x => new JsonChart
                {
                    Label = x.Key,
                    Count = x.Count()
                }).ToList();

                groupedChart.AddRange(groupedC3A);
                groupedChart.AddRange(groupedC3B);

                List<JsonChartLabel> listChartLabel = new List<JsonChartLabel>();

                foreach (var choice in listChoice)
                {
                    JsonChartLabel newChartLabel = new JsonChartLabel();
                    newChartLabel.XLabel = choice;  // YA, TIDAK, TT/TJ

                    var groupedbyChoice = groupedChart.Where(x => x.Label == choice).ToList();  // 

                    newChartLabel.Charts.AddRange(groupedbyChoice);

                    listChartLabel.Add(newChartLabel);
                }

                return listChartLabel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<JsonChart>> FindC1Async(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C1).Select(x => new JsonChart
                {
                    Label = x.Key,
                    Count = x.Count()
                }).ToList();

                List<JsonChart> listChart = new List<JsonChart>();

                List<string> list2Choice = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE2CODE }, claims)).Select(x => x.Value).ToList();

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

        public async Task<List<JsonChart>> FindC3AAsync(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C3A).Select(x => new JsonChart
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

        public async Task<List<JsonChart>> FindC3BAsync(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C3B).Select(x => new JsonChart
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


        public async Task<List<JsonChart>> FindC4Async(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C4).Select(x => new JsonChart
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

        public async Task<List<JsonChart>> FindC7Async(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C7).Select(x => new JsonChart
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

        public async Task<List<JsonChart>> FindAgeAsync(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer> listAnswer = await FindAsync(filter, claims);

                int maxAge = listAnswer.Max(x => x.Usia).GetValueOrDefault();
                int minAge = listAnswer.Min(x => x.Usia).GetValueOrDefault();

                int minAgeRound = minAge - (minAge % 5);
                int maxAgeRound = maxAge - (maxAge % 5) + 5;

                var ageGroup = Enumerable
                    .Range(minAgeRound, maxAgeRound)
                    .Where(age => age % 5 == 0 && age >= minAgeRound && age < maxAgeRound)
                    .ToList();

                var groupedData = ageGroup.Select(rangeStart =>
                {
                    var rangeEnd = rangeStart + 4;
                    var label = $"{rangeStart} - {rangeEnd}";
                    var groupData = listAnswer.Where(x => x.Usia >= rangeStart && x.Usia <= rangeEnd).ToList();
                    return new JsonChart { Label = label, Count = groupData.Count };
                }).ToList();

                return groupedData;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<JsonChart>> FindReligionAsync(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C8).Select(x => new JsonChart
                {
                    Label = x.Key,
                    Count = x.Count()
                }).ToList();

                List<JsonChart> listChart = new List<JsonChart>();

                List<string> listAgama = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.AGAMACODE }, claims)).OrderBy(x => x.Description).Select(x => x.Value).ToList();

                foreach (var choice in listAgama)
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

        public async Task<List<JsonChart>> FindTribeAsync(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C10).Select(x => new JsonChart
                {
                    Label = x.Key,
                    Count = x.Count()
                }).ToList();

                List<JsonChart> listChart = new List<JsonChart>();

                List<string> listSuku = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.SUKUCODE }, claims)).Select(x => x.Value).ToList();

                foreach (var choice in listSuku)
                {
                    JsonChart newChart = new JsonChart();
                    newChart.Label = choice.ToString();
                    newChart.Count = 0;

                    var cekList = grouped.FirstOrDefault(x => x.Label == choice);
                    if (cekList != null) newChart.Count = cekList.Count;

                    listChart.Add(newChart);
                }

                listChart.RemoveAll(x => x.Count < 1);

                return listChart;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<JsonChart>> FindAcademicAsync(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                List<JsonAnswer> answerList = await FindAsync(filter, claims);

                var grouped = answerList.GroupBy(x => x.C9).Select(x => new JsonChart
                {
                    Label = x.Key,
                    Count = x.Count()
                }).ToList();

                List<JsonChart> listChart = new List<JsonChart>();

                List<string> listPendidikan = (await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.PENDIDIKANCODE }, claims)).OrderBy(x => x.Description).Select(x => x.Value).ToList();

                foreach (var choice in listPendidikan)
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

        public async Task<List<JsonAnswer>> FindAsync(JsonAnswer filter, ClaimsPrincipal claims)
        {
            List<JsonAnswer> answerList = new List<JsonAnswer>();

            try
            {
                String ClientID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);
                filter.ClientID = ClientID;
                Expression<Func<Answer, bool>> filterExp = c => true;
                if (!String.IsNullOrEmpty(filter.Nama)) filterExp = filterExp.And(x => x.Nama.StartsWith(filter.Nama));
                if (!String.IsNullOrEmpty(filter.Kelurahan)) filterExp = filterExp.And(x => x.Kelurahan == filter.Kelurahan);
                if (!String.IsNullOrEmpty(filter.Kecamatan)) filterExp = filterExp.And(x => x.Kecamatan == filter.Kecamatan);
                if (!String.IsNullOrEmpty(filter.C8)) filterExp = filterExp.And(x => x.C8 == filter.C8);
                if (!String.IsNullOrEmpty(filter.C9)) filterExp = filterExp.And(x => x.C9 == filter.C9);
                if (!String.IsNullOrEmpty(filter.C10)) filterExp = filterExp.And(x => x.C10 == filter.C10);
                if (!String.IsNullOrEmpty(filter.C6)) filterExp = filterExp.And(x => x.C6 == filter.C6);
                filterExp = filterExp.And(x => x.ClientID == filter.ClientID);

                answerList = _mapper.Map<IEnumerable<Answer>, List<JsonAnswer>>(await Repo.QueryAnswersAsync(filterExp, filter.OrderBy, filter.OrderByDirection, filter.Take.GetValueOrDefault(), filter.Skip.GetValueOrDefault()));

                answerList = answerList.AsQueryable().OrderBy(filter.OrderBy + " " + filter.OrderByDirection).ToList();
                return answerList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> FindCountAsync(JsonAnswer filter, ClaimsPrincipal claims)
        {
            try
            {
                String ClientID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);
                filter.ClientID = ClientID;
                Expression<Func<Answer, bool>> filterExp = c => true;
                if (!String.IsNullOrEmpty(filter.Kelurahan)) filterExp = filterExp.And(x => x.Kelurahan == filter.Kelurahan);
                if (!String.IsNullOrEmpty(filter.Kecamatan)) filterExp = filterExp.And(x => x.Kecamatan == filter.Kecamatan);
                if (!String.IsNullOrEmpty(filter.C6)) filterExp = filterExp.And(x => x.C6 == filter.C6);
                filterExp = filterExp.And(x => x.ClientID == filter.ClientID);

                int TotalCount = await Repo.QueryAnswersCountAsync(filterExp);

                return TotalCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<JsonAnswer> GetAnswer(string ID)
        {
            try
            {
                return _mapper.Map<Answer, JsonAnswer>(await Repo.GetAnswerByIDAsync(ID));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<JsonReturn> SaveAsync(JsonAnswer Save, ClaimsPrincipal claims)
        {
            JsonReturn result = new JsonReturn(false);
            try
            {
                String ClientID = GlobalHelpers.GetClaimValueByType(EnumClaims.ClientID.ToString(), claims);

                string Error = "";
                //if (String.IsNullOrEmpty(Save.ID))
                //{
                //    if (await Repo.IsUniqueKeyCodeExist(Save.Nama, Save.Nama_Kk))
                //    {
                //        Error = $"{Save.Nama} dengan KK : {Save.Nama_Kk} sudah pernah diinput";

                //        result = new JsonReturn(false);
                //        result.message = Error;
                //    }
                //}

                if (String.IsNullOrEmpty(Error))
                {
                    Answer NewData = new Answer();

                    if (String.IsNullOrEmpty(Save.ID))
                    {
                        NewData.ID = Guid.NewGuid().ToString();
                        NewData.Nama = Save.Nama;
                        NewData.Usia = Save.Usia;
                        NewData.Nama_kk = Save.Nama_Kk;
                        NewData.Alamat = Save.Alamat;
                        NewData.Rt = Save.Rt;
                        NewData.Rw = Save.Rw;
                        NewData.Kelurahan = Save.Kelurahan;
                        NewData.Kecamatan = Save.Kecamatan;
                        NewData.NIK = Save.NIK;
                        NewData.Nomor_telp = Save.Nomor_telp;
                        NewData.C1 = Save.C1;
                        NewData.C2 = Save.C2;
                        NewData.C3A = Save.C3A;
                        NewData.C3B = Save.C3B;
                        NewData.C4 = Save.C4;
                        NewData.C5 = Save.C5;
                        if (Save.C6 != "Other") NewData.C6 = Save.C6;
                        else if (!string.IsNullOrEmpty(Save.C6Other)) NewData.C6 = Save.C6Other.ToUpper();
                        NewData.C7 = Save.C7;
                        NewData.C8 = Save.C8;
                        NewData.C9 = Save.C9;
                        NewData.C10 = Save.C10;

                        NewData.ModelState = ObjectState.Added;

                    }
                    if (!String.IsNullOrEmpty(Save.ID))
                    {
                        NewData = await Repo.GetAnswerByIDAsync(Save.ID);

                        if (NewData == null)
                        {
                            result = new JsonReturn(false);
                            result.message = "Data tidak ditemukan";
                            return result;
                        }

                        NewData.Nama = Save.Nama;
                        NewData.Nama_kk = Save.Nama_Kk;
                        NewData.Usia = Save.Usia;
                        NewData.Alamat = Save.Alamat;
                        NewData.Rt = Save.Rt;
                        NewData.Rw = Save.Rw;
                        NewData.Kelurahan = Save.Kelurahan;
                        NewData.Kecamatan = Save.Kecamatan;
                        NewData.NIK = Save.NIK;
                        NewData.Nomor_telp = Save.Nomor_telp;
                        NewData.C1 = Save.C1;
                        NewData.C2 = Save.C2;
                        NewData.C3A = Save.C3A;
                        NewData.C3B = Save.C3B;
                        NewData.C4 = Save.C4;
                        NewData.C5 = Save.C5;
                        if (Save.C6 != "Other") NewData.C6 = Save.C6;
                        else if (!string.IsNullOrEmpty(Save.C6Other)) NewData.C6 = Save.C6Other.ToUpper();
                        NewData.C7 = Save.C7;
                        NewData.C8 = Save.C8;
                        NewData.C9 = Save.C9;
                        NewData.C10 = Save.C10;

                        NewData.ModelState = ObjectState.Modified;

                    }


                    if (Save.C6 == "Other" && !string.IsNullOrEmpty(Save.C6Other))
                    {
                        HelperTable newHelper = new HelperTable();

                        newHelper.Code = "CALON";
                        newHelper.Name = Save.C6Other.ToUpper();
                        newHelper.Value = Save.C6Other.ToUpper();
                        newHelper.Description = Save.C6Other;

                        newHelper.ID = newHelper.Code + newHelper.Value;
                        newHelper.ModelState = ObjectState.Added;

                        UnitOfWork.InsertOrUpdate(claims, newHelper);
                    }

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

        public async Task<bool> DeleteAsync(string ID)
        {
            try
            {
                Answer data = await Repo.GetAnswerByIDAsync(ID);

                if (data != null)
                {
                    _dbcontext.Answers.Remove(data);
                    _dbcontext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<byte[]> DownloadExcelDocument(JsonAnswer filter, ClaimsPrincipal claims)
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
                    worksheet.Cell(1, 5).Value = "Nama KK";
                    worksheet.Cell(1, 6).Value = "Alamat";
                    worksheet.Cell(1, 7).Value = "C1";
                    worksheet.Cell(1, 8).Value = "C2";
                    worksheet.Cell(1, 9).Value = "C3A";
                    worksheet.Cell(1, 10).Value = "C3B";
                    worksheet.Cell(1, 11).Value = "C4";
                    worksheet.Cell(1, 12).Value = "C5";
                    worksheet.Cell(1, 12).Value = "C6";
                    worksheet.Cell(1, 14).Value = "C7";
                    worksheet.Cell(1, 15).Value = "C8";
                    worksheet.Cell(1, 16).Value = "C9";
                    worksheet.Cell(1, 17).Value = "C10";

                    List<JsonAnswer> datas = await FindAsync(filter, claims);

                    for (int index = 1; index < datas.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = datas[index - 1].Nama;
                        worksheet.Cell(index + 1, 2).Value = datas[index - 1].Usia;
                        worksheet.Cell(index + 1, 3).Value = datas[index - 1].NIK;
                        worksheet.Cell(index + 1, 4).Value = datas[index - 1].Nomor_telp;
                        worksheet.Cell(index + 1, 5).Value = datas[index - 1].Nama_Kk;
                        worksheet.Cell(index + 1, 6).Value = $"{datas[index - 1].Alamat}, RT {datas[index - 1].Rt} RW {datas[index - 1].Rw}, {datas[index - 1].Kelurahan} {datas[index - 1].Kecamatan}";
                        worksheet.Cell(index + 1, 7).Value = datas[index - 1].C1;
                        worksheet.Cell(index + 1, 8).Value = datas[index - 1].C2;
                        worksheet.Cell(index + 1, 9).Value = datas[index - 1].C3A;
                        worksheet.Cell(index + 1, 10).Value = datas[index - 1].C3B;
                        worksheet.Cell(index + 1, 11).Value = datas[index - 1].C4;
                        worksheet.Cell(index + 1, 12).Value = datas[index - 1].C5;
                        worksheet.Cell(index + 1, 13).Value = datas[index - 1].C6;
                        worksheet.Cell(index + 1, 14).Value = datas[index - 1].C7;
                        worksheet.Cell(index + 1, 15).Value = datas[index - 1].C8;
                        worksheet.Cell(index + 1, 16).Value = datas[index - 1].C9;
                        worksheet.Cell(index + 1, 17).Value = datas[index - 1].C10;
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
