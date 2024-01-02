using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Model.Models;
using Service.Services.Master;
using Model.JsonModels.Master;
using Database.JsonModels;
using Service.Services.Setting;
using Model.JsonModels;
using Service.Helpers;
using static Service.Helpers.GlobalHelpers;
using ConstantVariableKey = Model.InfrastructurClass.ConstantVariable;
using Model.JsonModels.Setting;

namespace MainProject.Controllers
{
    public class LoginController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostenv;
        private IConfiguration _config;
        HelperTableService ServiceHelper;
        UserService ServiceUser;

        public LoginController(ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostenv, IMapper mapper, IConfiguration config, UserManager<User> userMgr, SignInManager<User> signinMgr)
        {
            _context = context;
            _mapper = mapper;
            _hostenv = hostenv;
            _config = config;
            ServiceUser = new UserService(_context, _mapper, config, userMgr);

            _userManager = userMgr;
            _signInManager = signinMgr;
            ServiceHelper = new HelperTableService(context, _mapper);
        }




        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();


            return RedirectToAction("LoginForm", "Login", new { area = "" });
        }

        public async Task<IActionResult> Home()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }

            List<JsonHelperTable> ListKelurahan = await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.KELURAHANCODE }, User);
            List<JsonHelperTable> ListRw = await ServiceHelper.FindAsync(new JsonHelperTable { Code = ConstantVariableKey.RWCODE }, User);

            ViewBag.listKelurahan = ListKelurahan.OrderBy(x => x.Name).ToList();
            ViewBag.listRw = ListRw;

            return View();
        }

        public async Task GenerateClaimAsync(User user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(EnumClaims.Username.ToString(), user.UserName),
                    //new Claim(EnumClaims.UserRoleID.ToString(), user.UserRoleID),
                    new Claim(EnumClaims.Email.ToString(), user.Email),
                    new Claim(EnumClaims.ClientID.ToString(), user.ClientID),
                    new Claim(EnumClaims.RolesCode.ToString(), user.Role)

                };

                await _signInManager.SignInWithClaimsAsync(user, true, claims);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginForm(JsonUser loginmodel)
        {
            if (String.IsNullOrEmpty(loginmodel.Password))
            {
                return View(loginmodel);
            }

            try
            {
                await _signInManager.SignOutAsync();

                var user = await _userManager.FindByEmailAsync(loginmodel.Email);

                if (user == null)
                {
                    ViewBag.ErrorResult = "No User Found!";
                    return View();
                }

                var logintry = await _signInManager.CheckPasswordSignInAsync(user, loginmodel.Password, false);

                if (logintry.Succeeded)
                {
                    await GenerateClaimAsync(user);
                    return RedirectToAction("Home", "Login");
                }
                else
                {
                    ViewBag.ErrorResult = "Wrong Password!";
                }

                return View();
            }
            catch (Exception ex)
            {
                Alert(ex.Message, NotificationType.error);
                return View(loginmodel);
            }
        }


        // GET: LoginPage
        //[AllowAnonymous]
        public async Task<IActionResult> LoginForm()
        {

            return View();
        }

        //GET: chagepass
        public async Task<IActionResult> ChangePass()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePass(JsonUser change)
        {
            if (ModelState.IsValid)
            {
                if (change.Password != change.PasswordConfirm)
                {
                    Alert("Password confirm does not match", NotificationType.error);
                    return View(change);
                }
                var result = ServiceUser.ChangePassword(change, User);
                if (result.result)
                {
                    Alert("Success Change password!", NotificationType.success);

                    await _signInManager.SignOutAsync();


                    return RedirectToAction("LoginForm", "Login", new { area = "" });

                }
                Alert("Failed change password! " + result.message, NotificationType.error);
                return View(change);
            }
            return View(change);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPass(string email)
        {
            JsonReturn res = new JsonReturn(false);
            res.message = "Connection error";


            //cek email exist gak
            var user = await ServiceUser.GetUserAsync(email);
            if (user == null)
            {
                res = new JsonReturn(false);
                Alert(res.message, NotificationType.warning);

                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }
            else
            {
                user.Password = GeneratePass();
                var respo = ServiceUser.ChangePassword(user, User);
                if (respo.result)
                {
                    // data.TwoFactorCode = token;
                    //string Body = await ServiceHelper.GetHelperTableDescByIDAsync("BodyEmailResetPassword");
                    //string Subject = await ServiceHelper.GetHelperTableValueByIDAsync("SubjectEmailResetPassword");
                    //string BCC = await ServiceHelper.GetHelperTableValueByIDAsync("BCCEmailResetPassword");
                    //Body = Body.Replace("@Password@", user.Password);
                    //Body = Body.Replace("@email@", email);

                    //if (_emailConfig.SystemMailDLL == 1)
                    //{
                    //    service.SentMail(Subject, Body, email, null, BCC);
                    //}
                    //else
                    //{
                    //    service.SentMailMailKit(Subject, Body, email, null, BCC);
                    //}


                    Alert("Success Reset password! please check your email", NotificationType.success);
                    return RedirectToAction("LoginForm", "Login", new { area = "" });
                }
                Alert(res.message, NotificationType.warning);

                return RedirectToAction("LoginForm", "Login", new { area = "" });
            }


        }


        public string GeneratePass()
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < 11; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
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
