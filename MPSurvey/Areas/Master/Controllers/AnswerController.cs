﻿using AutoMapper;
using DAL.Helpers;
using Microsoft.AspNetCore.Mvc;
using Model.InfrastructurClass;
using Model.JsonModels.Master;
using Model.JsonModels.Setting;
using Model.Models;
using Service.Services.Master;
using Service.Services.Setting;
using ConstantVariableKey = Model.InfrastructurClass.ConstantVariable;

namespace MainProject.Areas.Master.Controllers
{
    [Area("Master")]
    public class AnswerController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        AnswerService ServiceAnswer;
        HelperTableService ServiceHelper;

        public AnswerController(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            ServiceAnswer = new AnswerService(_dbcontext, _mapper);
            ServiceHelper = new HelperTableService(_dbcontext, _mapper);
        }

        public async Task<IActionResult> Index()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            List<JsonAnswer> page1 = await ServiceAnswer.FindAsync(new JsonAnswer { }, User);
            IndexAnswerVM data = new IndexAnswerVM();
            data.listIndex = page1;
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            try
            {

                var TaskKelurahan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KELURAHANCODE }, User);
                var TaskKecamatan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KECAMATANCODE }, User);
                var Task3Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE3CODE }, User);
                var Task2Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE2CODE }, User);
                var TaskCalon = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CALONCODE }, User);
                var TaskAgama = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.AGAMACODE }, User);
                var TaskPendidikan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.PENDIDIKANCODE }, User);
                var TaskSuku = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.SUKUCODE }, User);

                List<JsonHelperTable> ListKelurahan = await TaskKelurahan;
                List<JsonHelperTable> ListKecamatan = await TaskKecamatan;
                List<JsonHelperTable> List3Choice = await Task3Choice;
                List<JsonHelperTable> List2Choice = await Task2Choice;
                List<JsonHelperTable> ListCalon = await TaskCalon;
                List<JsonHelperTable> ListAgama = await TaskAgama;
                List<JsonHelperTable> ListPendidikan = await TaskPendidikan;
                List<JsonHelperTable> ListSuku = await TaskSuku;

                JsonAnswer data = new JsonAnswer();
                data.ListKelurahan = ListKelurahan.OrderBy(x => x.Value).ToList();
                data.ListKecamatan = ListKecamatan.OrderBy(x => x.Value).ToList();
                data.List3Choice = List3Choice.OrderBy(x => x.Description).ToList();
                data.List2Choice = List2Choice.OrderBy(x => x.Description).ToList();
                data.ListCalon = ListCalon.OrderBy(x => x.Value).ToList();
                data.ListAgama = ListAgama.OrderBy(x => x.Description).ToList();
                data.ListPendidikan = ListPendidikan.OrderBy(x => x.Description).ToList();
                data.ListSuku = ListSuku.OrderBy(x => x.Value).ToList();

                return View(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JsonAnswer data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var returnSave = await ServiceAnswer.SaveAsync(data, User);
                    
                    if (returnSave.result)
                    {
                        Alert(returnSave.message, NotificationType.success);
                    }
                    else
                    {
                        Alert(returnSave.message, NotificationType.error);
                    }
                    return RedirectToAction(nameof(Create));

                }
                catch (Exception ex)
                {
                    Alert(ex.Message, NotificationType.error);
                    return RedirectToAction(nameof(Create));
                }
            }

            var TaskKelurahan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KELURAHANCODE }, User);
            var TaskKecamatan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KECAMATANCODE }, User);
            var Task3Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE3CODE }, User);
            var Task2Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE2CODE }, User);
            var TaskCalon = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CALONCODE }, User);
            var TaskAgama = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.AGAMACODE }, User);
            var TaskPendidikan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.PENDIDIKANCODE }, User);
            var TaskSuku = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.SUKUCODE }, User);

            List<JsonHelperTable> ListKelurahan = await TaskKelurahan;
            List<JsonHelperTable> ListKecamatan = await TaskKecamatan;
            List<JsonHelperTable> List3Choice = await Task3Choice;
            List<JsonHelperTable> List2Choice = await Task2Choice;
            List<JsonHelperTable> ListCalon = await TaskCalon;
            List<JsonHelperTable> ListAgama = await TaskAgama;
            List<JsonHelperTable> ListPendidikan = await TaskPendidikan;
            List<JsonHelperTable> ListSuku = await TaskSuku;

            data.ListKelurahan = ListKelurahan.OrderBy(x => x.Value).ToList();
            data.ListKecamatan = ListKecamatan.OrderBy(x => x.Value).ToList();
            data.List3Choice = List3Choice.OrderBy(x => x.Description).ToList();
            data.List2Choice = List2Choice.OrderBy(x => x.Description).ToList();
            data.ListCalon = ListCalon.OrderBy(x => x.Value).ToList();
            data.ListAgama = ListAgama.OrderBy(x => x.Description).ToList();
            data.ListPendidikan = ListPendidikan.OrderBy(x => x.Description).ToList();
            data.ListSuku = ListSuku.OrderBy(x => x.Value).ToList();

            return View(data);
        }

        public async Task<IActionResult> Edit(string ID)
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            try
            {
                JsonAnswer data = await ServiceAnswer.GetAnswer(ID);

                var TaskKelurahan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KELURAHANCODE }, User);
                var TaskKecamatan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KECAMATANCODE }, User);
                var Task3Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE3CODE }, User);
                var Task2Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE2CODE }, User);
                var TaskCalon = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CALONCODE }, User);
                var TaskAgama = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.AGAMACODE }, User);
                var TaskPendidikan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.PENDIDIKANCODE }, User);
                var TaskSuku = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.SUKUCODE }, User);

                List<JsonHelperTable> ListKelurahan = await TaskKelurahan;
                List<JsonHelperTable> ListKecamatan = await TaskKecamatan;
                List<JsonHelperTable> List3Choice = await Task3Choice;
                List<JsonHelperTable> List2Choice = await Task2Choice;
                List<JsonHelperTable> ListCalon = await TaskCalon;
                List<JsonHelperTable> ListAgama = await TaskAgama;
                List<JsonHelperTable> ListPendidikan = await TaskPendidikan;
                List<JsonHelperTable> ListSuku = await TaskSuku;

                data.ListKelurahan = ListKelurahan.OrderBy(x => x.Value).ToList();
                data.ListKecamatan = ListKecamatan.OrderBy(x => x.Value).ToList();
                data.List3Choice = List3Choice.OrderBy(x => x.Description).ToList();
                data.List2Choice = List2Choice.OrderBy(x => x.Description).ToList();
                data.ListCalon = ListCalon.OrderBy(x => x.Value).ToList();
                data.ListAgama = ListAgama.OrderBy(x => x.Description).ToList();
                data.ListPendidikan = ListPendidikan.OrderBy(x => x.Description).ToList();
                data.ListSuku = ListSuku.OrderBy(x => x.Value).ToList();

                return View(data);
            }
            catch (Exception ex)
            {
                Alert(ex.Message, NotificationType.error);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JsonAnswer data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var returnSave = await ServiceAnswer.SaveAsync(data, User);

                    if (returnSave.result)
                    {
                        Alert(returnSave.message, NotificationType.success);
                    }
                    else
                    {
                        Alert(returnSave.message, NotificationType.error);
                    }
                    return RedirectToAction(nameof(Create));

                }
                catch (Exception ex)
                {
                    Alert(ex.Message, NotificationType.error);
                    return RedirectToAction(nameof(Index));
                }
            }

            var TaskKelurahan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KELURAHANCODE }, User);
            var TaskKecamatan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KECAMATANCODE }, User);
            var Task3Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE3CODE }, User);
            var Task2Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE2CODE }, User);
            var TaskCalon = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CALONCODE }, User);
            var TaskAgama = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.AGAMACODE }, User);
            var TaskPendidikan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.PENDIDIKANCODE }, User);
            var TaskSuku = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.SUKUCODE }, User);

            List<JsonHelperTable> ListKelurahan = await TaskKelurahan;
            List<JsonHelperTable> ListKecamatan = await TaskKecamatan;
            List<JsonHelperTable> List3Choice = await Task3Choice;
            List<JsonHelperTable> List2Choice = await Task2Choice;
            List<JsonHelperTable> ListCalon = await TaskCalon;
            List<JsonHelperTable> ListAgama = await TaskAgama;
            List<JsonHelperTable> ListPendidikan = await TaskPendidikan;
            List<JsonHelperTable> ListSuku = await TaskSuku;

            data.ListKelurahan = ListKelurahan.OrderBy(x => x.Value).ToList();
            data.ListKecamatan = ListKecamatan.OrderBy(x => x.Value).ToList();
            data.List3Choice = List3Choice.OrderBy(x => x.Description).ToList();
            data.List2Choice = List2Choice.OrderBy(x => x.Description).ToList();
            data.ListCalon = ListCalon.OrderBy(x => x.Value).ToList();
            data.ListAgama = ListAgama.OrderBy(x => x.Description).ToList();
            data.ListPendidikan = ListPendidikan.OrderBy(x => x.Description).ToList();
            data.ListSuku = ListSuku.OrderBy(x => x.Value).ToList();

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string ID)
        {
            try
            {
                bool result = await ServiceAnswer.DeleteAsync(ID);

                //if (result)
                //{
                //    Alert("Berhasil menghapus data!", NotificationType.success);
                //}
                //else
                //{
                //    Alert("Gagal menghapus data!", NotificationType.error);
                //}
                return Json(result);
            }
            catch (Exception ex)
            {
                Alert(ex.Message, NotificationType.error);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> MainChart()
        {
            try
            {
                List <JsonChart> chartData = await ServiceAnswer.FindChartAsync(new JsonAnswer(), User);

                return Json(chartData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReligionChart()
        {
            try
            {
                List<JsonChartLabel> chartData = await ServiceAnswer.FindChartReligionAsync(new JsonAnswer(), User);

                return Json(chartData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> C1Count()
        {
            try
            {
                List<JsonChart> data = await ServiceAnswer.FindC1Async(new JsonAnswer(), User);

                return Json(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> C3Chart()
        {
            try
            {
                List<JsonChartLabel> chartData = await ServiceAnswer.FindC3Async(new JsonAnswer(), User);

                return Json(chartData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> C3AChart()
        {
            try
            {
                List<JsonChart> data = await ServiceAnswer.FindC3AAsync(new JsonAnswer(), User);

                return Json(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> C3BChart()
        {
            try
            {
                List<JsonChart> data = await ServiceAnswer.FindC3BAsync(new JsonAnswer(), User);

                return Json(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> C4Chart()
        {
            try
            {
                List<JsonChart> data = await ServiceAnswer.FindC4Async(new JsonAnswer(), User);

                return Json(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> C7Chart()
        {
            try
            {
                List<JsonChart> data = await ServiceAnswer.FindC7Async(new JsonAnswer(), User);

                return Json(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IActionResult> LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                if (pageSize <= 0)
                {
                    pageSize = 0;
                }
                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                /// INI KALAU DI PAKE AJA BUAT CONTOH TRANSAKSI LAEN 


                JsonAnswer filterJson = new JsonAnswer();
                filterJson.Skip = skip;
                filterJson.Take = pageSize;
                filterJson.OrderBy = !String.IsNullOrEmpty(sortColumn) ? sortColumn : "CreatedTime";
                filterJson.OrderByDirection = !String.IsNullOrEmpty(sortColumnDirection) ? sortColumnDirection : "desc";


                var ListQuery = Request.Form.ToList();
                string Query = "";
                try
                {
                    foreach (var item in ListQuery.Where(x => x.Key.Contains("[search][value]")).Where(x => !String.IsNullOrEmpty(x.Value)).ToList())
                    {
                        string CheckBaris = item.Key.Replace("[search][value]", "");
                        var ListNamaKolom = ListQuery.Where(x => x.Key.Contains(CheckBaris)).ToList();
                        var namakolom = ListNamaKolom.FirstOrDefault(x => x.Key.Contains("name"));
                        var kolom = namakolom.Value;
                        //Search  
                        if (!string.IsNullOrEmpty(kolom))
                        {
                            Query += (" and " + kolom + " like ''%" + item.Value + "%'' ");

                        }

                    }

                }
                catch (Exception)
                {

                    throw;
                }


                filterJson.Query = Query;


                IEnumerable<JsonAnswer> ListDataGrid = await ServiceAnswer.FindAsync(filterJson, User);
                #region Sorting




                #endregion

                recordsTotal = await ServiceAnswer.FindCountAsync(filterJson, User);
                //Paging   
                //var data = ListDataGrid.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = ListDataGrid });

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IActionResult> DownloadExcelDocument()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string filename = ServiceHelper.GetHelperTableValueByID(ConstantVariableKey.ANSWEREXPORTFILENAME);
            try
            {
                if (!string.IsNullOrEmpty(filename))
                {
                    var content = await ServiceAnswer.DownloadExcelDocument(new JsonAnswer(), User);
                    return File(content, contentType, filename);
                }
                else
                {
                    Alert("Nama file tidak ditemukan, mohon cek tabel config", NotificationType.error);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "swal('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "";
            TempData["notification"] = msg;
        }

        public enum NotificationType
        {
            error,
            success,
            warning,
            info
        }

    }
}
