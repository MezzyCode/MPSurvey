using AutoMapper;
using Model.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Service.Services.Master;
using Service.Services.Setting;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Identity;
using Service.Helpers;
using Model.JsonModels.Master;
using Database.JsonModels;
using Model.JsonModels;

namespace MainProject.Areas.Master.Controllers
{
    [Area("Master")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        HelperTableService ServiceHelper;
        UserService ServiceUser;



        public UserController(ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile, UserManager<User> userMgr, IHttpContextAccessor accessor)
        {


            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            ServiceHelper = new HelperTableService(_context, _mapper);
            ServiceUser = new UserService(_context, _mapper, configfile, userMgr);
            
        }

        public IActionResult Index(string Url)
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            IndexUserVM data = new IndexUserVM();

            data.UrlFileLog = Url;
            return View(data);
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

                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                /// INI KALAU DI PAKE AJA BUAT CONTOH TRANSAKSI LAEN 


                JsonUser filterJson = new JsonUser();
                filterJson.Skip = skip;
                filterJson.Take = pageSize;
                filterJson.OrderBy = !String.IsNullOrEmpty(sortColumn) ? sortColumn : "CreatedTime";
                filterJson.OrderByDirection = !String.IsNullOrEmpty(sortColumnDirection) ? sortColumnDirection : "desc";


                var ListQuery = Request.Form.ToList();
                string Query = "";
                //try
                //{
                //    foreach (var item in ListQuery.Where(x => x.Key.Contains("[search][value]")).Where(x => !String.IsNullOrEmpty(x.Value)).ToList())
                //    {
                //        string CheckBaris = item.Key.Replace("[search][value]", "");
                //        var ListNamaKolom = ListQuery.Where(x => x.Key.Contains(CheckBaris)).ToList();
                //        var namakolom = ListNamaKolom.FirstOrDefault(x => x.Key.Contains("name"));
                //        var kolom = namakolom.Value;
                //        //Search  
                //        if (!string.IsNullOrEmpty(kolom))
                //        {
                //            Query += (" and " + kolom + " like '%" + item.Value + "%'");

                //        }

                //    }

                //}
                //catch (Exception)
                //{

                //    throw;
                //}


                filterJson.Query = Query;


                IEnumerable<JsonUser> ListDataGrid = await ServiceUser.FindAsync(filterJson, User);
                #region Sorting




                #endregion

                recordsTotal = await ServiceUser.FindCountAsync(filterJson, User);
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = ListDataGrid });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Create()
        {
            JsonUser data = new JsonUser();
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });

            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JsonUser data)
        {

            if (ModelState.IsValid)
            {
                string errMsg = "";

                var retrunSave = await ServiceUser.SaveAsync(data, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Create User", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }

                errMsg = retrunSave.message;
                Alert(errMsg, NotificationType.error);
                return View(data);
            }

            return View(data);
        }

        public async Task<IActionResult> View(String formID)
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }
            var data = ServiceUser.GetUserAsync(formID);
            return View(data);
        }

        public async Task<IActionResult> Edit(String ID)
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            var data = await ServiceUser.GetUserAsync(ID);
            data.PasswordConfirm = data.Password;

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JsonUser data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var upddata = _mapper.Map<JsonUser, JsonUser>(data);
                var retrunSave = await ServiceUser.SaveAsync(upddata, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Update User", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }


                errMsg = retrunSave.message;
                Alert(errMsg, NotificationType.error);
                return View(data);
            }

            return View(data);
        }

        public string CopyFile(IFormFile fileInput)
        {

            var fileName2 = System.IO.Path.GetFileName(Guid.NewGuid().ToString().Substring(0, 7) + "-" + fileInput.FileName);
            // Create new local file and copy contents of uploaded file
            using (var localFile = System.IO.File.OpenWrite(Path.Combine(_hostenv.WebRootPath, "Upload/") + fileName2))
            using (var uploadedFile = fileInput.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }

            return fileName2;
        }

        public IActionResult Upload()
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            IndexUserVM data = new IndexUserVM();
            var idLog = TempData["idLog"];
            if (idLog != null)
            {
                data.UrlFileLog = idLog.ToString();
            }

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteData(string id)
        {
            JsonReturn result = new JsonReturn(false);

            try
            {
                result.message = "data not found";
                var com = await ServiceUser.GetUserAsync(id);
                if (com != null)
                {
                    result = await ServiceUser.DeleteAsync(com, User);


                }



                if (result.result == true)
                {
                    return Json(result);
                }
                else
                {
                    Alert(result.message, NotificationType.error);
                    return Json(result);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "swal('" + notificationType.ToString().ToUpper() + "', `" + message + "`,'" + notificationType + "')" + "";
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
