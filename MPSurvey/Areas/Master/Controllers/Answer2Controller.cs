using AutoMapper;
using Model.JsonModels.Master;
using Model.JsonModels.Setting;
using Model.JsonModels;
using Service.Helpers;
using Service.Services.Master;
using Service.Services.Setting;
using static Service.Helpers.GlobalHelpers;
using Model.Models;
using Microsoft.AspNetCore.Mvc;
using ConstantVariableKey = Model.InfrastructurClass.ConstantVariable;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MainProject.Areas.Master.Controllers
{
    [Area("Master")]
    public class Answer2Controller : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        Answer2Service ServiceAnswer2;
        HelperTableService ServiceHelper;

        public Answer2Controller(ApplicationDbContext dbcontext, IMapper mapper, IHostingEnvironment hostenv)
        {
            _dbcontext = dbcontext;
            _hostenv = hostenv;
            _mapper = mapper;
            ServiceAnswer2 = new Answer2Service(_dbcontext, _mapper);
            ServiceHelper = new HelperTableService(_dbcontext, _mapper);
        }

        public async Task<IActionResult> Index()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            var TaskKelurahan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KELURAHANCODE }, User);
            var TaskCalon = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CALONCODE }, User);
            var TaskRw = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.RWCODE }, User);

            List<JsonHelperTable> ListKelurahan = await TaskKelurahan;
            List<JsonHelperTable> ListCalon = await TaskCalon;
            List<JsonHelperTable> ListRw = await TaskRw;

            ViewBag.listKelurahan = ListKelurahan;
            ViewBag.listCalon = ListCalon;
            ViewBag.listRw = ListRw;

            //List<JsonAnswer2> page1 = await ServiceAnswer2.FindAsync(new JsonAnswer2 { }, User);
            IndexAnswer2VM data = new IndexAnswer2VM();
            //data.listIndex = page1;
            return View(data);
        }

        public async Task<IActionResult> IndexPersonal()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            var TaskKelurahan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KELURAHANCODE }, User);
            var TaskCalon = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CALONCODE }, User);
            var TaskRw = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.RWCODE }, User);

            List<JsonHelperTable> ListKelurahan = await TaskKelurahan;
            List<JsonHelperTable> ListCalon = await TaskCalon;
            List<JsonHelperTable> ListRw = await TaskRw;

            ViewBag.listKelurahan = ListKelurahan;
            ViewBag.listCalon = ListCalon;
            ViewBag.listRw = ListRw;

            //List<JsonAnswer2> page1 = await ServiceAnswer2.FindAsync(new JsonAnswer2 { }, User);
            IndexAnswer2VM data = new IndexAnswer2VM();
            //data.listIndex = page1;
            return View(data);
        }

        public async Task<IActionResult> Upload()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            IndexAnswer2VM data = new IndexAnswer2VM();
            var idLog = TempData["idLog"];
            if (idLog != null)
            {
                data.UrlFileLog = idLog.ToString();
            }

            return View(data);

        }

        [HttpPost]
        public async Task<IActionResult> Upload(IndexAnswer2VM file)
        {
            try
            {
                if (file.Upload == null)
                {
                    ModelState.AddModelError("FileURL", "Mohon upload file");
                    return View();
                }

                string UrlLog = "";
                IndexAnswer2VM data = new IndexAnswer2VM();

                if (file.Upload.FileName != null)
                {
                    var filename = GlobalHelpers.CopyFile(file.Upload, _hostenv);
                    var _path = Path.Combine(_hostenv.WebRootPath, "Upload/" + filename);

                    var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";

                    string FileNameWithoutEx = Path.GetFileNameWithoutExtension(filename);
                    String LogName = FileNameWithoutEx + "_LOGEXPORT.xlsx";
                    string FileLogName = Path.Combine(_hostenv.WebRootPath, "Upload/" + LogName);
                    UrlLog = baseUrl + "/Upload/" + LogName;
                    JsonReturn jsonresult = await ServiceAnswer2.GenerateData(_path, FileLogName, User);

                    if (System.IO.File.Exists(FileLogName))
                    {
                        data.UrlFileLog = UrlLog;
                    }


                    if (jsonresult.result == false)
                    {
                        Alert(jsonresult.message, NotificationType.warning);
                    }
                    else
                    {
                        Alert(jsonresult.message, NotificationType.success);
                    }
                    try
                    {
                        if (System.IO.File.Exists(_path))
                        {
                            System.IO.File.Delete(_path);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }

                ViewBag.Message = "File Uploaded Successfully!!";

                return View(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IActionResult> Create()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            var cookiesRole = GlobalHelpers.GetClaimValueByType(EnumClaims.RolesCode.ToString(), User);
            if (cookiesRole == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }
            else if (cookiesRole != "Staff")
            {
                return RedirectToAction("Home", "Login", new { area = "" });
            }

            try
            {

                var TaskKelurahan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KELURAHANCODE }, User);
                var TaskKecamatan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KECAMATANCODE }, User);
                var Task3Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE3CODE }, User);
                var Task2Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE2CODE }, User);
                var TaskRw = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.RWCODE }, User);

                List<JsonHelperTable> ListKelurahan = await TaskKelurahan;
                List<JsonHelperTable> ListKecamatan = await TaskKecamatan;
                List<JsonHelperTable> List3Choice = await Task3Choice;
                List<JsonHelperTable> List2Choice = await Task2Choice;
                List<JsonHelperTable> ListRw = await TaskRw;

                JsonAnswer2 data = new JsonAnswer2();
                data.ListKelurahan = ListKelurahan.OrderBy(x => x.Value).ToList();
                data.ListKecamatan = ListKecamatan.OrderBy(x => x.Value).ToList();
                data.List3Choice = List3Choice.OrderBy(x => x.Description).ToList();
                data.List2Choice = List2Choice.OrderBy(x => x.Description).ToList();
                data.ListRw = ListRw;

                return View(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JsonAnswer2 data)
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var returnSave = await ServiceAnswer2.SaveAsync(data, User);

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
            var TaskRw = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.RWCODE }, User);

            List<JsonHelperTable> ListKelurahan = await TaskKelurahan;
            List<JsonHelperTable> ListKecamatan = await TaskKecamatan;
            List<JsonHelperTable> List3Choice = await Task3Choice;
            List<JsonHelperTable> List2Choice = await Task2Choice;
            List<JsonHelperTable> ListRw = await TaskRw;

            data.ListKelurahan = ListKelurahan.OrderBy(x => x.Value).ToList();
            data.ListKecamatan = ListKecamatan.OrderBy(x => x.Value).ToList();
            data.List3Choice = List3Choice.OrderBy(x => x.Description).ToList();
            data.List2Choice = List2Choice.OrderBy(x => x.Description).ToList();
            data.ListRw = ListRw;


            Alert("Mohon isi semua kolom yang diperlukan!", NotificationType.error);

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
                JsonAnswer2 data = await ServiceAnswer2.GetAnswer2(ID);

                if (data.CreatedBy != GlobalHelpers.GetClaimValueByType(EnumClaims.Username.ToString(), User))
                {
                    Alert("Akses Ditolak!", NotificationType.error);
                    return RedirectToAction(nameof(Index));
                }

                var TaskKelurahan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KELURAHANCODE }, User);
                var TaskKecamatan = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KECAMATANCODE }, User);
                var Task3Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE3CODE }, User);
                var Task2Choice = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.CHOISE2CODE }, User);
                var TaskRw = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.RWCODE }, User);

                List<JsonHelperTable> ListKelurahan = await TaskKelurahan;
                List<JsonHelperTable> ListKecamatan = await TaskKecamatan;
                List<JsonHelperTable> List3Choice = await Task3Choice;
                List<JsonHelperTable> List2Choice = await Task2Choice;
                List<JsonHelperTable> ListRw = await TaskRw;

                data.ListKelurahan = ListKelurahan.OrderBy(x => x.Value).ToList();
                data.ListKecamatan = ListKecamatan.OrderBy(x => x.Value).ToList();
                data.List3Choice = List3Choice.OrderBy(x => x.Description).ToList();
                data.List2Choice = List2Choice.OrderBy(x => x.Description).ToList();
                data.ListRw = ListRw;

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
        public async Task<IActionResult> Edit(JsonAnswer2 data)
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var returnSave = await ServiceAnswer2.SaveAsync(data, User);

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
            var TaskRw = ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.RWCODE }, User);

            List<JsonHelperTable> ListKelurahan = await TaskKelurahan;
            List<JsonHelperTable> ListKecamatan = await TaskKecamatan;
            List<JsonHelperTable> List3Choice = await Task3Choice;
            List<JsonHelperTable> List2Choice = await Task2Choice;
            List<JsonHelperTable> ListRw = await TaskRw;

            data.ListKelurahan = ListKelurahan.OrderBy(x => x.Value).ToList();
            data.ListKecamatan = ListKecamatan.OrderBy(x => x.Value).ToList();
            data.List3Choice = List3Choice.OrderBy(x => x.Description).ToList();
            data.List2Choice = List2Choice.OrderBy(x => x.Description).ToList();
            data.ListRw = ListRw;

            Alert("Mohon isi semua kolom!", NotificationType.error);

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string ID)
        {
            try
            {
                JsonReturn result = await ServiceAnswer2.SoftDeleteAsync(ID, User);


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
        public async Task<IActionResult> C1Count(string? Kelurahan, string? Rw)
        {
            try
            {
                JsonAnswer2 filter = new JsonAnswer2();
                filter.Kelurahan = Kelurahan;
                filter.Rw = Rw;
                List<JsonChart> chartData = await ServiceAnswer2.FindC1Async(filter, User);

                return Json(chartData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> C2Count(string? Kelurahan, string? Rw)
        {
            try
            {
                JsonAnswer2 filter = new JsonAnswer2();
                filter.Kelurahan = Kelurahan;
                filter.Rw = Rw;
                List<JsonChart> chartData = await ServiceAnswer2.FindC2Async(filter, User);

                return Json(chartData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> C3Count(string? Kelurahan, string? Rw)
        {
            try
            {
                JsonAnswer2 filter = new JsonAnswer2();
                filter.Kelurahan = Kelurahan;
                filter.Rw = Rw;
                List<JsonChart> chartData = await ServiceAnswer2.FindC3Async(filter, User);

                return Json(chartData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> C4Count(string? Kelurahan, string? Rw)
        {
            try
            {
                JsonAnswer2 filter = new JsonAnswer2();
                filter.Kelurahan = Kelurahan;
                filter.Rw = Rw;
                List<JsonChart> chartData = await ServiceAnswer2.FindC4Async(filter, User);

                return Json(chartData);
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


                JsonAnswer2 filterJson = new JsonAnswer2();
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
                filterJson.Nama = Request.Form["nama"];
                filterJson.Kelurahan = Request.Form["kelurahan"];
                filterJson.Rw = Request.Form["rw"];


                IEnumerable<JsonAnswer2> ListDataGrid = await ServiceAnswer2.FindAsync(filterJson, User);
                #region Sorting




                #endregion

                recordsTotal = await ServiceAnswer2.FindCountAsync(filterJson, User);
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

        public async Task<IActionResult> LoadDataPersonal()
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


                JsonAnswer2 filterJson = new JsonAnswer2();
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
                filterJson.Nama = Request.Form["nama"];
                filterJson.Kelurahan = Request.Form["kelurahan"];
                filterJson.Rw = Request.Form["rw"];


                IEnumerable<JsonAnswer2> ListDataGrid = await ServiceAnswer2.FindByUserAsync(filterJson, User);
                #region Sorting




                #endregion

                recordsTotal = await ServiceAnswer2.FindCountByUserAsync(filterJson, User);
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
                    var content = await ServiceAnswer2.DownloadExcelDocument(new JsonAnswer2(), User);
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
