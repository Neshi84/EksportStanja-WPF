using EksportStanja.Models;
using System.Collections.Generic;

namespace EksportStanja.Services
{
    public interface IXmlService
    {
        public void saveXml(List<Lek> lista, string fileName);
    }
}
