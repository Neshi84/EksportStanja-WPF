using EksportStanja.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EksportStanja.Repository
{
    public interface IUtrosakRepository
    {
        public ObservableCollection<Lek> GetAll(string path);

        public ObservableCollection<Lek> GetAllCentralni(string path);
        public ObservableCollection<Lek> EksportStanjaCentralniPoStavkama(string filePath);
    }
}