using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BovespaBot.Extensions
{
    public static class DataTableExt
    {
        public static void ExportToExcel(this List<System.Data.DataTable> tblList, string excelFilePath = null)
        {
            // load excel, and create a new workbook
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.DisplayAlerts = false;
            var wb = excelApp.Workbooks.Add();

            try
            {
                foreach (var tbl in tblList)
                {
                    if (tbl == null || tbl.Columns.Count == 0)
                        throw new Exception("ExportToExcel: Null or empty input table!\n");

                    // single worksheet
                    wb.Worksheets.Add();
                    Microsoft.Office.Interop.Excel._Worksheet workSheet = wb.ActiveSheet;
                    workSheet.Name = tbl.TableName;
                    // column headings
                    for (var i = 0; i < tbl.Columns.Count; i++)
                    {
                        workSheet.Cells[1, i + 1] = tbl.Columns[i].ColumnName;
                    }

                    // rows
                    for (var i = 0; i < tbl.Rows.Count; i++)
                    {
                        // to do: format datetime values before printing
                        for (var j = 0; j < tbl.Columns.Count; j++)
                        {
                            workSheet.Cells[i + 2, j + 1] = tbl.Rows[i][j];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
            finally
            {
                // check file path
                if (!string.IsNullOrEmpty(excelFilePath))
                {
                    try
                    {
                        wb.SaveAs(excelFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, true, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
                        excelApp.Quit();
                        MessageBox.Show($"Arquivo Excel Salvo em {excelFilePath}!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                                            + ex.Message);
                    }

                }
                else
                { // no file path is given
                    excelApp.Visible = true;
                }
            }
        }
    }
}
