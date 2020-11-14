using EksportStanja.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EksportStanja.Services
{
    public interface IExcelService
    {
        public void Export<T>(List<T> data, string filePath, string sheetName = "Sheet1");
        public ObservableCollection<Lek> GetOdeljenske(string excel);
    }
}