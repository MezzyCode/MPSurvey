using ClosedXML.Excel;
using Database.JsonModels.Setting;
using Model.JsonModels.Setting;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers
{
    public class ExcelHelper
    {
        public static void GenerateExcelFileFromFileLogDetail(String PathName, List<JsonFileLogDetail> logDetails)
        {
            // Below code is create datatable and add one row into datatable.  

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cell(1, 1).Value = "No.";
                worksheet.Cell(1, 2).Value = "Success";
                worksheet.Cell(1, 3).Value = "Remarks";

                var logOrdered = logDetails.OrderBy(x => x.OrderNo).ToList();

                for (int index = 1; index <= logDetails.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = logDetails[index - 1].OrderNo;
                    worksheet.Cell(index + 1, 2).Value = logDetails[index - 1].Status.ToString();
                    worksheet.Cell(index + 1, 3).Value = logDetails[index - 1].Remarks;

                }
                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                //return stream;
                string FilePath = PathName;

                //Write to file using file stream  
                FileStream file = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
                stream.WriteTo(file);
                file.Close();
                stream.Close();
            }
            // Declare HSSFWorkbook object for create sheet  
            //var workbook = new HSSFWorkbook();
            //var sheet = workbook.CreateSheet("Sheet1");

            //var headerRow = sheet.CreateRow(0);


            //var cellHeader = headerRow.CreateCell(0);
            //cellHeader.SetCellValue("No");

            //var cellHeader1 = headerRow.CreateCell(1);
            //cellHeader1.SetCellValue("Succes");
            //var cellHeader2 = headerRow.CreateCell(2);
            //cellHeader2.SetCellValue("Remarks");
            //int i = 0;
            //foreach (var item in logDetails.ToList().OrderBy(x => x.OrderNo).ToList())
            //{

            //    var rowIndex = i + 1;
            //    var row = sheet.CreateRow(rowIndex);


            //    var cell0 = row.CreateCell(0);
            //    cell0.SetCellValue(item.OrderNo.ToString());

            //    var cell1 = row.CreateCell(1);
            //    cell1.SetCellValue(item.Status.ToString());
            //    var cell2 = row.CreateCell(2);
            //    cell2.SetCellValue(item.Remarks.ToString());
            //    i++;
            //}



            //// Declare one MemoryStream variable for write file in stream  
            //var stream = new MemoryStream();
            //workbook.Write(stream);
            ////return stream;
            //string FilePath = PathName;

            ////Write to file using file stream  
            //FileStream file = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
            //stream.WriteTo(file);
            //file.Close();
            //stream.Close();
        }
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
        public static void GenerateExcelFile(String PathName, DataTable DT)
        {
            // Below code is create datatable and add one row into datatable.  

            // Declare HSSFWorkbook object for create sheet  
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Sheet1");

            var headerRow = sheet.CreateRow(0);

            //Below loop is create header  
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                var cell = headerRow.CreateCell(i);
                cell.SetCellValue(DT.Columns[i].ColumnName.ToUpper());
            }

            //Below loop is fill content  
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                var rowIndex = i + 1;
                var row = sheet.CreateRow(rowIndex);

                for (int j = 0; j < DT.Columns.Count; j++)
                {

                    var cell = row.CreateCell(j);
                    var Data = DT.Rows[i][j];
                    DateTime temp;
                    if (DateTime.TryParse(Data.ToString(), out temp))
                    {
                        cell.SetCellValue(Convert.ToDateTime(Data.ToString()).ToString("dd-MMM-yyyy"));
                    }
                    else
                    {
                        cell.SetCellValue(Data.ToString());
                    }


                }
            }

            // Declare one MemoryStream variable for write file in stream  
            var stream = new MemoryStream();
            workbook.Write(stream);
            //return stream;
            string FilePath = PathName;

            //Write to file using file stream  
            FileStream file = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
            stream.WriteTo(file);
            file.Close();
            stream.Close();
        }

        public static HSSFWorkbook GenerateExcelFileToMainStream(DataTable DT)
        {
            // Below code is create datatable and add one row into datatable.  

            // Declare HSSFWorkbook object for create sheet  
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Sheet1");

            var headerRow = sheet.CreateRow(0);

            //Below loop is create header  
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                var cell = headerRow.CreateCell(i);
                cell.SetCellValue(DT.Columns[i].ColumnName.ToUpper());
            }

            //Below loop is fill content  
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                var rowIndex = i + 1;
                var row = sheet.CreateRow(rowIndex);

                for (int j = 0; j < DT.Columns.Count; j++)
                {

                    var cell = row.CreateCell(j);
                    var Data = DT.Rows[i][j];
                    DateTime temp;
                    if (DateTime.TryParse(Data.ToString(), out temp))
                    {
                        cell.SetCellValue(Convert.ToDateTime(Data.ToString()).ToString("dd-MMM-yyyy"));
                    }
                    else
                    {
                        cell.SetCellValue(Data.ToString());
                    }


                }
            }

            return workbook;
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
        public static DataTable GetRequestsDataFromExcel(string fullFilePath, int startRow)
        {
            try
            {
                var sh = GetFileStream(fullFilePath);
                var dtExcelTable = new DataTable();
                dtExcelTable.Rows.Clear();
                dtExcelTable.Columns.Clear();
                var headerRow = sh.GetRow(startRow);
                int colCount = headerRow.LastCellNum;
                for (var c = 0; c < colCount; c++)
                    dtExcelTable.Columns.Add(headerRow.GetCell(c).ToString().Trim());
                var i = startRow + 1;
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
                                        //? cell.DateCellValue.ToString(CultureInfo.InvariantCulture)
                                        ? cell.DateCellValue.ToString("yyyy-MM-dd")
                                        : cell.NumericCellValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataTable GetRequestsDataFromExcelSheets(string fullFilePath)
        {
            try
            {

                IWorkbook workbook;
                using (var stream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(stream);
                }

                int sheetCount = workbook.NumberOfSheets;
                var dataTable = new DataTable();
                int currSheet = 0;
                while (currSheet < sheetCount)
                {
                    var sheet = workbook.GetSheetAt(currSheet);
                    var currDataTable = new DataTable(sheet.SheetName);
                    //var colCount = sheet.GetRow(0).LastCellNum;

                    // header row if its the first sheet
                    //if (currSheet == 0)
                    //{
                    //    var headerRow = sheet.GetRow(0);
                    //    foreach (var headerCell in headerRow)
                    //    {
                    //        currDataTable.Columns.Add(headerCell.ToString());
                    //    }
                    //}

                    var headerRow = sheet.GetRow(0);
                    var colCount = headerRow.LastCellNum;
                    foreach (var headerCell in headerRow)
                    {
                        currDataTable.Columns.Add(headerCell.ToString());
                    }

                    var i = 1;

                    var currentRow = sheet.GetRow(i);
                    while (currentRow != null)
                    {
                        var dr = currDataTable.NewRow();
                        for (var j = 0; j < colCount; j++)
                        {
                            var cell = currentRow.GetCell(j);

                            if (cell != null)
                            {
                                switch (cell.CellType)
                                {
                                    case CellType.Numeric:
                                        dr[j] = DateUtil.IsCellDateFormatted(cell)
                                            ? cell.DateCellValue.ToString("yyyy-MM-dd")
                                            : cell.NumericCellValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                        break;
                                    case CellType.String:
                                        dr[j] = cell.StringCellValue;
                                        break;
                                    case CellType.Blank:
                                        dr[j] = string.Empty;
                                        break;
                                }
                            }
                        }
                        currDataTable.Rows.Add(dr);
                        i++;
                        currentRow = sheet.GetRow(i);
                    }
                    dataTable.Merge(currDataTable);
                    currSheet++;
                }

                return dataTable;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}