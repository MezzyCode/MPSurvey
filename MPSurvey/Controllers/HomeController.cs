using System;
using System.Diagnostics;
using AutoMapper;
using MainProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model.Models;

namespace MainProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _logger = logger;
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            
        }
         
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
