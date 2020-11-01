using ClosedXML;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EksportStanja.Services
{
    public class ExcelService : IExcelService
    {
        public void Export<T>(List<T> data, string filePath, string sheetName = "Sheet1")
        {
            var workbook = new XLWorkbook();
            ToWorkSheet(workbook, data, sheetName);
            workbook.SaveAs(filePath);
        }

        private IXLWorksheet ToWorkSheet<T>(XLWorkbook workBook, IList<T> data, string sheetName = "Sheet1")
        {
            var genericType = typeof(T);
            var workSheet = workBook.Worksheets.Add(sheetName);
            //ColumnProperty Info
            var columnList = typeof(T).GetProperties()
                                      .Select(p => new
                                      {
                                          name = p.Name,
                                          tip = p.PropertyType.Name,
                                          red = p.GetAttributes<DisplayAttribute>()[0].Order
                                      }).OrderBy(r => r.red).ToList();

            for (int i = 0; i < columnList.Count; i++)
            {
                var column = columnList[i];
                var cell = workSheet.Cell(1, i + 1);

                cell.Value = column.name;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Font.Bold = true;
            }

            // Create Rows
            double suma = 0;

            for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
            {
                var row = data[rowIndex];

                for (int columnIndex = 0; columnIndex < columnList.Count; columnIndex++)
                {
                    var column = columnList[columnIndex];
                    var cell = workSheet.Cell(rowIndex + 2, columnIndex + 1);
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    switch (column.tip)
                    {
                        case "String":
                            cell.SetDataType(XLDataType.Text);
                            break;

                        case "Double":
                            cell.SetDataType(XLDataType.Number);
                            break;

                        default:
                            cell.SetDataType(XLDataType.Text);
                            break;
                    }

                    cell.SetValue(row.GetType().GetProperty(column.name).GetValue(row, null));
                }
            }
            if (typeof(T).Name == "UtrosakPoStavkama")
            {
                suma = data.Sum(p => double.Parse(p.GetType().GetProperty("Ukupno").GetValue(p, null).ToString()));

                workSheet.LastRowUsed().RowBelow().Cell(columnList.Count - 1).SetValue("Ukupno");
                workSheet.LastRowUsed().Cell(columnList.Count).SetValue(suma).SetDataType(XLDataType.Number);
            }

            workSheet.Columns().AdjustToContents();
            return workSheet;
        }
    }
}