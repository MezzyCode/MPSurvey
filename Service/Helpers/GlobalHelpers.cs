using ClosedXML.Excel;
using Model.InfrastructurClass;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers
{
    public static class ConstantVariable
    {

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public enum MODULNAME
        {
            GENERALCLAIM,
            ADVANCEREQUEST,
            ADVANCESETTLEMANT,
        }

        public static string DiscountReference = "AmountDiscountItemRelated";
        public static string DiscountEmpReference = "AmountDiscountEmployeeRelated";
        public static string QueryDefaultUser = "SELECT * FROM USER WHERE 1=1 ";


        public enum FIELDBEHAVIOUR
        {
            UP,
            DOWN
        }
        public enum PERMANENTDELETE
        {
            RolePriviledge,
        }
        public static int RowStatusDone = 4;

        public static string DocumentExpiredVendor = "DocumentExpiredVendor";
        public static string JobReminderApproval = "ReminderApproval";
        public static int Settle = 1;
        public static string JobCreatedBy = "JOBHR";

        public static string SubmitDocument = "SubmitDocumentExpenseDevelop";
        public static string UpdateDocument = "UpdateStatusDocumentExpenseDevelop";
        public static string StatusDefault = "Draft";
        public static string IDStatusFinish = "99.1";
        public static string IDStatusReject = "99.2";
        public static string IDStatusReject_BACK = ".2";


        public static string IDStatusReject1 = "2.2";
        public static string IDStatusReject2 = "3.2";
        public static string IDStatusReject3 = "4.2";
        public static string IDStatusReject4 = "5.2";
        public static string IDStatusReject5 = "6.2";
        public static string IDStatusRejectACChecker = "7.2";
        public static string IDStatusRejectCMChecker = "8.2";
        public static string IDStatusRejectCMApprover = "9.2";

        public static string IDHelperNoWorkflow = "NOWF";
        public static string DocumentTypeReplenish = "9";

        public static string Currency_SAP = "IDR";

        public static string Code_PettyCashPakai = "CLAIM01";
        public static string Code_GeneralClaim = "CLAIM02";
        public static string Code_CC = "CLAIM03";
        public static string Code_WirelessPersonal = "CLAIM04";
        public static string Code_WirelessCorporate = "CLAIM05";
        public static string Code_PettyReplenish = "CLAIM06";
        public static string Code_PettyRequestToko = "CLAIM07";
        public static string Code_AdvOperational = "ADVANCE01";
        public static string Code_AdvTravel = "ADVANCE03";
        public static string Code_SettOper = "SETTLE01";
        public static string Code_SettTravel = "SETTLE02";

        public static string Code_PettOffRequest = "CLAIM08";
        public static string Code_PettOffReimburse = "CLAIM10";
        public static string Code_PettOffReplen = "CLAIM11";

        public static string Code_ReturnPettCash = "RETURN01";
        public static string Code_ReturnPettCashOffice = "RETURN02";
        public static string Expense_CashRegis = "Store Cashier Register";
        public static string Expense_PettyCashReq = "Petty Cash Request";
        public static string Expense_PettyCashOffice = "Petty Cash Office";

        public static string BLART_KR = "KR";
        public static string BLART_SA = "SA";
        public static string BLART_Z1 = "Z1";

        //Petty Cash Office
        public static string CLAIM08 = "CLAIM08";
        //Petty Cash Office Reimbursement (Penggunaan)
        public static string CLAIM10 = "CLAIM10";
        //Petty Cash Office Replenishment (No Workflow)
        public static string CLAIM11 = "CLAIM11";
        //Return Petty Cash Office
        public static string RETURN02 = "RETURN02";

        public static string FINISH = "FINISH";
        public static string PROCESS = "ON PROCESS";


        public static string ArticleMangoCreated = "Article Mango";
        public static string DetailShipmentCreated = "Detail Shipment";
        public static string StockMangoCreated = "Stock Mango";
        public static string CreatedBy = "System";

        public static string BRAND = "MANGO";
        public static string Shipment_type = "AIR";
        public static string Product_UOM = "EA";
        public static string Incremental = "incremental";

        public enum EditMode
        {
            INSERT,
            EDIT,
            DELETE,
            NEW
        }
        public enum EnumUserType
        {
            USER

        }




    }
    public static class GlobalHelpers
    {

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public static string CopyFile(IFormFile fileInput, IHostingEnvironment _hostenv, String folder, HttpRequest HttpRequest)
        {
            var baseUrl = $"{HttpRequest.Scheme}://{HttpRequest.Host.Value.ToString()}{HttpRequest.PathBase.Value.ToString()}";



            var fileName2 = System.IO.Path.GetFileName(Guid.NewGuid().ToString().Substring(0, 7) + "-" + fileInput.FileName);
            fileName2 = RemoveSpecialCharacters(fileName2);
            // Create new local file and copy contents of uploaded file
            using (var localFile = System.IO.File.OpenWrite(Path.Combine(_hostenv.WebRootPath, folder) + fileName2))
            using (var uploadedFile = fileInput.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }

            return baseUrl + "/" + folder + fileName2;
        }
        public static string CopyFile(IFormFile fileInput, IHostingEnvironment _hostenv)
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
        public static String ReGenerateThreadClaim(ClaimsPrincipal User)
        {
            String Result = "";
            try
            {
                System.Threading.Thread.CurrentPrincipal = User;
            }
            catch (Exception)
            {

                throw;
            }

            return Result;
        }
        public enum ClaimIdentity
        {
            Username,
            ClientID,
            ID,
            Nama,


        }


        public static string GetEmailFromIdentity(ClaimsPrincipal User)
        {
            String result = null;
            try
            {
                var resultx = User.Claims.ToList().FirstOrDefault(x => x.Type == EnumClaims.Email.ToString());
                result = resultx != null ? resultx.Value : null;

                return result;
            }
            catch (Exception)
            {

                return null;
            }

        }
        //public static string GetClientID(ClaimsPrincipal User, SystemConfig systemConfig)
        //{
        //    String result = null;
        //    try
        //    {
        //        if (User != null)
        //        {
        //            var resultx = User.Claims.ToList().FirstOrDefault(x => x.Type == EnumClaims.Email.ToString());
        //            result = resultx != null ? resultx.Value : null;

        //            return result;
        //        }
        //        else
        //        {
        //            return systemConfig.ClientID;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return systemConfig.ClientID;

        //    }

        //}




        //public static bool GetApiKeyValidation(String Username, ApplicationDbContext DBContext, HttpContext HttpContext, out ClaimsPrincipal claimsPrincipal)
        //{
        //    claimsPrincipal = null;
        //    try
        //    {

        //        string encryptionKey = "EDS";
        //        string dStr = Encryption.passwordDecrypt(Username, encryptionKey);
        //        var Userx = DBContext.Users.FirstOrDefault(x => x.Email == dStr);
        //        if (Userx != null)
        //        {
        //            //di clear dl logout dl





        //            // generate lah tokennya di sini
        //            String StoreID = "";

        //            if (!String.IsNullOrEmpty(Userx.StoreID))
        //            {
        //                StoreID = Userx.StoreID;
        //            }

        //            var claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.Name, Userx.Email),
        //    new Claim(EnumClaims.Username.ToString(), Userx.Email),
        //    new Claim(EnumClaims.IsAD.ToString(), Userx.IsAD.ToString()),
        //    new Claim(EnumClaims.UserRoleID.ToString(), Userx.UserRoleID),
        //    new Claim(EnumClaims.Email.ToString(), Userx.Email),
        //    new Claim(EnumClaims.ClientID.ToString(), Userx.ClientID),
        //};

        //            var claimsIdentity = new ClaimsIdentity(
        //          claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //            var authProperties = new AuthenticationProperties
        //            {
        //                AllowRefresh = true,
        //                // Refreshing the authentication session should be allowed.

        //                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
        //                // The time at which the authentication ticket expires. A 
        //                // value set here overrides the ExpireTimeSpan option of 
        //                // CookieAuthenticationOptions set with AddCookie.

        //                //IsPersistent = true,
        //                // Whether the authentication session is persisted across 
        //                // multiple requests. When used with cookies, controls
        //                // whether the cookie's lifetime is absolute (matching the
        //                // lifetime of the authentication ticket) or session-based.

        //                IssuedUtc = DateTime.Now,
        //                // The time at which the authentication ticket was issued.

        //                RedirectUri = "/Home/"
        //                // The full path or absolute URI to be used as an http 
        //                // redirect response value.
        //            };


        //            HttpContext.SignInAsync(
        //              CookieAuthenticationDefaults.AuthenticationScheme,
        //              new ClaimsPrincipal(claimsIdentity),
        //              authProperties).ConfigureAwait(false).GetAwaiter().GetResult();

        //            System.Threading.Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);



        //            System.Threading.Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);
        //            claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}

        public static ISheet GetFileStream(string fullFilePath)
        {
            var fileExtension = Path.GetExtension(fullFilePath);
            string sheetName;
            ISheet sheet = null;
            switch (fileExtension)
            {
                case ".xlsx":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new XSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (XSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
                case ".xls":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new HSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (HSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
            }
            return sheet;
        }
        public static Boolean ContainColumn(string columnName, DataTable table)
        {
            Boolean Result = false;
            DataColumnCollection columns = table.Columns;
            if (columns.Contains(columnName))
            {
                Result = true;
            }

            return Result;
        }


        public static string ConvertDateETPToString(string date)
        {
            string Result = "";
            //example : 20230401
            try
            {
                string Year = date.Substring(0, 4);
                string Month = date.Substring(4, 2);
                string day = date.Substring(6, 2);

                Result = Year + "-" + Month + "-" + day;
            }
            catch (Exception)
            {

                return "";
            }
            return Result;
        }

        public static DateTime? ConvertDateEASToDate(string date)
        {

            DateTime outputDateTimeValue;
            if (DateTime.TryParseExact(date, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out outputDateTimeValue))
            {
                return outputDateTimeValue;
            }
            else
            {
                return null;
            }
        }

        public static string ConvertTimeETPToString(string time)
        {
            string Result = "";
            //example : 20230401
            try
            {
                string hour = time.Substring(0, 2);
                string minute = time.Substring(2, 2);
                string second = time.Substring(4, 2);

                Result = hour + ":" + minute + ":" + second;
            }
            catch (Exception)
            {

                return "";
            }
            return Result;
        }

        public static DataTable GetRequestsDataFromExcel(string fullFilePath)
        {
            try
            {
                var sh = GetFileStream(fullFilePath);
                var dtExcelTable = new DataTable();
                dtExcelTable.Rows.Clear();
                dtExcelTable.Columns.Clear();
                var headerRow = sh.GetRow(0);
                int colCount = headerRow.LastCellNum;
                for (var c = 0; c < colCount; c++)
                    dtExcelTable.Columns.Add(headerRow.GetCell(c).ToString());
                var i = 1;
                var currentRow = sh.GetRow(i);
                while (currentRow != null)
                {
                    var dr = dtExcelTable.NewRow();
                    for (var j = 0; j < colCount; j++)
                    {
                        var cell = currentRow.GetCell(j);

                        if (cell != null)
                            switch (cell.CellType)
                            {
                                case CellType.Numeric:
                                    dr[j] = DateUtil.IsCellDateFormatted(cell)
                                        ? cell.DateCellValue.ToString(CultureInfo.InvariantCulture)
                                        : cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                                    break;
                                case CellType.String:
                                    dr[j] = cell.StringCellValue;
                                    break;
                                case CellType.Blank:
                                    dr[j] = string.Empty;
                                    break;
                            }
                    }
                    dtExcelTable.Rows.Add(dr);
                    i++;
                    currentRow = sh.GetRow(i);
                }
                return dtExcelTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static HelperTable GetHelperTable(String ID, String Code, string IDKlien, ApplicationDbContext BM)
        {

            HelperTable data = BM.HelperTables.FirstOrDefault(x => x.ID == ID && x.Code == Code && IDKlien == x.ClientID);
            if (data == null)
            {
                data = new HelperTable();
            }

            return data;
        }
        public static System.Boolean IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }


        public enum EnumClaims
        {
            ID
                , RolesCode
        , Nama
        , Username

            , IsAD
        , Email
        , CreatedBy
        , CreatedDate
        , ModifiedBy
        , ModifiedDate
        , ClientID
        , CostCenterID
        , VendorID
        , IDLevel
                , CompanyID
        , RowStatus
        , IDEmployee
        , UserRoleID
        , RoleID


        , CompanyName

        }
        public enum MethodEnum
        {
            POST,
            GET

        }
        public enum ActionEnum
        {
            Update,
            Delete,
            View,
            ViewDetail,
            Download,
            Add
        }

        public static HttpStatusCode SentRequest(String JsonData, String URL, String Method, Boolean isAuthorize, String Token, out string returndata)
        {
            returndata = "";

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod(Method), URL))
                {
                    try
                    {
                        if (isAuthorize)
                        {
                            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + Token);

                        }


                        request.Content = new StringContent(JsonData);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                        var response = httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        string result = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        returndata = result;
                        return response.StatusCode;



                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }



            }
        }

        public enum TableName
        {
            StoreMappingUser,
            DocumentType,
            Priviledge,
            BudgetType,
            BudgetLimitCost,
            Transportation,
            RolePriviledge,
            Currency,
            Vendor,
            UserRole,
            GLAccount,
            User,
            DocumentTypeClaim,
            DocumentSetting,
            DocumentWorkflow,
            DocumentHeaderExtend,
            DocumentSubType,
            ActivityDefinitions,
            ActivityInstances,
            BlockingActivities,
            DocumentDetail,
            ConnectionDefinitions,
            DocumentHeader,
            WorkflowDefinitionVersions,
            WorkflowInstances,
            DocumentSettingDetail,
            BankAccount,
            Employee,
            AttachmentTypeDocument,
            GradeEmployee,
            LevelEmployee,
            City,
            Company,
            CostCenter,
            Country,
            Department,
            DocumentAttachment,
            DocumentAttachmentAccess,
            PositionEmployee,
            DocumentHistory,
            DocumentWorkflowDetail,
            EmailJob,
            AccomodationCost,
            ExpenseGroup,
            ExpenseType,
            PerDiemCost,
            StatusDocument,
            Store,
            DocumentSettingConnectedList,
            SubBusinessUnit,
            DocumentSettingConnected
        }
        public static XLWorkbook GenerateExcelFileToByte(DataTable DT)
        {
            // Below code is create datatable and add one row into datatable.  

            // Declare HSSFWorkbook object for create sheet  
            var workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Workflow Matrix");

            int row = 1;


            //Below loop is create header  
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                worksheet.Cell(row, i + 1).Value = DT.Columns[i].ColumnName.ToUpper();

            }

            //Below loop is fill content  
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                row++;


                for (int j = 0; j < DT.Columns.Count; j++)
                {


                    var Data = DT.Rows[i][j];
                    DateTime temp;
                    if (DateTime.TryParse(Data.ToString(), out temp))
                    {

                        worksheet.Cell(row, j + 1).Value = Convert.ToDateTime(Data.ToString()).ToString("dd-MMM-yyyy");
                    }
                    else
                    {


                        worksheet.Cell(row, j + 1).Value = Data.ToString();
                    }


                }
            }




            return workbook;
        }
        public static string ActionChecking(TableName tableName, ActionEnum actionEnum, string ID)
        {
            string Err = "";

            try
            {
                if (tableName == TableName.AccomodationCost)
                {

                }
                Err = "data cannot update its using by other data";
                return Err;
            }
            catch (Exception)
            {
                return Err;
            }


        }


        public static String GetClaimValueByType(String Type, ClaimsPrincipal User)
        {
            String Result = "";
            try
            {
                List<Claim> ListClaim = User.Claims.ToList();
                if (ListClaim != null)
                {
                    Claim Claim = ListClaim.FirstOrDefault(x => x.Type == Type);
                    if (Claim != null)
                    {
                        Result = Claim.Value;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Result;
        }
        public static string GetHash(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }


        public static string GetErrorMessage(Exception ex)
        {
            String Message = ex.InnerException != null ? ex.InnerException.Message : null;
            if (ex.InnerException != null && ex.InnerException.InnerException != null)
            {
                Message += ex.InnerException.InnerException.Message;
            }

            return "Error At : \n" + ex.Message + "\n" + Message;

        }

        //public static string GetErrorMessageModelState(ModelStateDictionary Model)
        //{

        //    String Message = "";
        //    //var message = string.Join(" | ", Model.Values
        //    //    .SelectMany(v => v.Errors)
        //    //    .Select(e => e.ErrorMessage));


        //    foreach (var item in Model.Keys.ToList())
        //    {
        //        Message += "|" + item.Replace("jsonData.", "") + "*";
        //    }
        //    return Message;

        //}

        //public static async Task<bool> InsertAPILog(APILog datas, String ConnString)
        //{
        //    if (datas.JsonBody == null) datas.JsonBody = "";

        //    using (SqlConnection conn = new SqlConnection(ConnString))
        //    {
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            StringBuilder QUERY = new StringBuilder();

        //            QUERY.AppendLine("    INSERT INTO [dbo].[APILog] ");
        //            QUERY.AppendLine("    ([ID]");
        //            QUERY.AppendLine("    ,[JsonBody] ");
        //            QUERY.AppendLine("    ,[Response] ");
        //            QUERY.AppendLine("    ,[Status] ");
        //            QUERY.AppendLine("    ,[From]");
        //            QUERY.AppendLine("    ,[To]");
        //            QUERY.AppendLine("    ,[CodeData] ");
        //            QUERY.AppendLine("    ,[CreatedBy] ");
        //            QUERY.AppendLine("    ,[CreatedTime] ");
        //            QUERY.AppendLine("    ,[LastModifiedBy] ");
        //            QUERY.AppendLine("    ,[LastModifiedTime] ");
        //            QUERY.AppendLine("    ,[RowStatus]) ");
        //            QUERY.AppendLine("    VALUES (@param1 ,@param2 ,@param3 ,@param4 ,@param5 ,@param6 ,@param7 ,@param8 ,@param9 ,@param10 ,@param11 ,@param12)"); ;


        //            cmd.CommandText = QUERY.ToString();

        //            cmd.Parameters.AddWithValue("@param1", datas.ID);
        //            cmd.Parameters.AddWithValue("@param2", datas.JsonBody);
        //            cmd.Parameters.AddWithValue("@param3", datas.Response);
        //            cmd.Parameters.AddWithValue("@param4", datas.Status);
        //            cmd.Parameters.AddWithValue("@param5", datas.From);
        //            cmd.Parameters.AddWithValue("@param6", datas.To);
        //            cmd.Parameters.AddWithValue("@param7", datas.CodeData);
        //            cmd.Parameters.AddWithValue("@param8", datas.CreatedBy);
        //            cmd.Parameters.AddWithValue("@param9", datas.CreatedTime);
        //            cmd.Parameters.AddWithValue("@param10", datas.LastModifiedBy);
        //            cmd.Parameters.AddWithValue("@param11", datas.LastModifiedTime);
        //            cmd.Parameters.AddWithValue("@param12", datas.RowStatus);

        //            try
        //            {
        //                conn.Open();
        //                cmd.ExecuteNonQuery();

        //            }
        //            catch (SqlException e)
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}

        //public static bool InsertFileLogHeaderNewOnly(FileLog datas, String ConString)
        //{
        //    if (datas.Remarks == null)
        //    {
        //        datas.Remarks = "";
        //    }

        //    Console.Write(datas.Remarks);

        //    using (SqlConnection conn = new SqlConnection(ConString))
        //    {

        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {

        //            StringBuilder QUERY = new StringBuilder();

        //            QUERY.AppendLine("    INSERT INTO[dbo].[FileLogs]             ");
        //            QUERY.AppendLine("    ([ID]                                  ");
        //            QUERY.AppendLine("    ,[TableName]                           ");
        //            QUERY.AppendLine("    ,[FileName]                            ");
        //            QUERY.AppendLine("    ,[Status]                              ");
        //            QUERY.AppendLine("    ,[Remarks]                             ");
        //            QUERY.AppendLine("    ,[CreatedBy]                           ");
        //            QUERY.AppendLine("    ,[CreatedTime]                        ");
        //            QUERY.AppendLine("    ,[RowStatus])                        ");
        //            QUERY.AppendLine(" VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8)                            ");


        //            cmd.CommandText = QUERY.ToString();

        //            cmd.Parameters.AddWithValue("@param1", datas.ID);
        //            cmd.Parameters.AddWithValue("@param2", datas.TableName);
        //            cmd.Parameters.AddWithValue("@param3", datas.FileName);
        //            cmd.Parameters.AddWithValue("@param4", datas.Status);
        //            cmd.Parameters.AddWithValue("@param5", datas.Remarks);
        //            cmd.Parameters.AddWithValue("@param6", datas.CreatedBy);
        //            cmd.Parameters.AddWithValue("@param7", datas.CreatedTime);
        //            cmd.Parameters.AddWithValue("@param8", datas.RowStatus);

        //            try
        //            {
        //                conn.Open();
        //                cmd.ExecuteNonQuery();

        //            }
        //            catch (SqlException e)
        //            {
        //                return false;
        //            }

        //        }
        //    }

        //    return true;
        //}


        //public static bool UpdateLogHeader(FileLog datas, String ConString)
        //{
        //    if (datas.Remarks == null)
        //    {
        //        datas.Remarks = "";
        //    }
        //    using (SqlConnection conn = new SqlConnection(ConString))
        //    {

        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {

        //            StringBuilder QUERY = new StringBuilder();

        //            QUERY.AppendLine("    Update [dbo].[FileLogs]    set Status = '" + datas.Status + "',Remarks = '" + ConstantVariable.FINISH + "' where  ID = '" + datas.ID + "' ");


        //            cmd.CommandText = QUERY.ToString();


        //            try
        //            {
        //                conn.Open();
        //                cmd.ExecuteNonQuery();

        //            }
        //            catch (SqlException e)
        //            {
        //                return false;
        //            }

        //        }
        //    }

        //    return true;
        //}


        //public static bool UpdateLogHeaderProses(FileLog datas, String ConString, string RowNumber)
        //{
        //    if (datas.Remarks == null)
        //    {
        //        datas.Remarks = "";
        //    }
        //    using (SqlConnection conn = new SqlConnection(ConString))
        //    {

        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {

        //            StringBuilder QUERY = new StringBuilder();

        //            QUERY.AppendLine("    Update [dbo].[FileLogs]    set Status = '" + datas.Status + "',Remarks = 'ON PROCESS " + RowNumber + "' where  ID = '" + datas.ID + "' ");


        //            cmd.CommandText = QUERY.ToString();


        //            try
        //            {
        //                conn.Open();
        //                cmd.ExecuteNonQuery();

        //            }
        //            catch (SqlException e)
        //            {
        //                return false;
        //            }

        //        }
        //    }

        //    return true;
        //}



        //public static bool InsertFileLogDetail(FileLogDetail datas, String ConString, string namatable)
        //{

        //    using (SqlConnection conn = new SqlConnection(ConString))
        //    {
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {

        //            StringBuilder QUERY = new StringBuilder();
        //            QUERY.AppendLine("  INSERT INTO[dbo].[FileLogDetails]        ");
        //            QUERY.AppendLine("  ([ID]                                   ");
        //            QUERY.AppendLine("  ,[IDFileLog]                            ");
        //            QUERY.AppendLine("  ,[OrderNo]                              ");
        //            QUERY.AppendLine("  ,[Status]                               ");
        //            QUERY.AppendLine("  ,[Remarks]                              ");
        //            QUERY.AppendLine("  ,[SourceTxt]                            ");
        //            QUERY.AppendLine("  ,[CreatedBy]                            ");
        //            QUERY.AppendLine("  ,[CreatedTime]                         ");
        //            QUERY.AppendLine("  ,[CodeData]                         ");
        //            QUERY.AppendLine("  ,[RowStatus])                         ");
        //            QUERY.AppendLine(" VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@Param8, @param9, @param10)                            ");


        //            cmd.CommandText = QUERY.ToString();

        //            cmd.Parameters.AddWithValue("@param1", datas.ID);
        //            cmd.Parameters.AddWithValue("@param2", datas.IDFileLog);
        //            cmd.Parameters.AddWithValue("@param3", datas.OrderNo);
        //            cmd.Parameters.AddWithValue("@param4", datas.Status);
        //            cmd.Parameters.AddWithValue("@param5", datas.Remarks != null ? datas.Remarks : " ");
        //            cmd.Parameters.AddWithValue("@param6", datas.SourceTxt);
        //            cmd.Parameters.AddWithValue("@param7", datas.CreatedBy);
        //            cmd.Parameters.AddWithValue("@param8", datas.CreatedTime);
        //            cmd.Parameters.AddWithValue("@param9", datas.CodeData);
        //            cmd.Parameters.AddWithValue("@param10", datas.RowStatus);
        //            try
        //            {
        //                conn.Open();
        //                cmd.ExecuteNonQuery();

        //            }
        //            catch (SqlException e)
        //            {
        //                Console.WriteLine("error at : " + e.Message);
        //                return false;
        //            }

        //        }
        //    }


        //    return true;
        //}


    }
}
