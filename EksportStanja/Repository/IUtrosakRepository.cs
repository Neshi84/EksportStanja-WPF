using EksportStanja.Models;
using System.Collections.Generic;

namespace EksportStanja.Repository
{
    public interface IUtrosakRepository
    {
        public IEnumerable<Utrosak> GetAll(string path);

        public IEnumerable<Utrosak> GetAllCentralni(string path);
        public IEnumerable<UtrosakPoStavkama> eksportStanjaCentralniPoStavkama(string filePath);
    }
}