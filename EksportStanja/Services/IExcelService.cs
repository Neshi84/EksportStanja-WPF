using System.Collections.Generic;

namespace EksportStanja.Services
{
    public interface IExcelService
    {
        public void Export<T>(List<T> data, string filePath, string sheetName = "Sheet1");
    }
}