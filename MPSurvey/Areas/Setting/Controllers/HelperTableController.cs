using AutoMapper;
using Model.JsonModels.Setting;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service.Services.Master;
using Service.Services.Setting;

namespace MainProject.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class HelperTableController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        HelperTableService ServiceHelper;
        public HelperTableController(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            ServiceHelper = new HelperTableService(_dbcontext, _mapper);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetHelperTables(string code = null, string description = null)
        {
            try
            {
                JsonHelperTable filter = new JsonHelperTable();
                if (!string.IsNullOrEmpty(code)) filter.Code = code;
                if (!string.IsNullOrEmpty(description)) filter.Description = description;
                List<JsonHelperTable> datas = await ServiceHelper.FindAsync(filter, User);

                return Json(datas);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
