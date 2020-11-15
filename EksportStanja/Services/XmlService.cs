using EksportStanja.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace EksportStanja.Services
{
    class XmlService : IXmlService
    {
        private static NumberFormatInfo provider = new NumberFormatInfo()
        {
            NumberDecimalSeparator = "."
        };

        public void saveXml(List<Lek> lista, string fileName)
        {
            XElement Stanje = new XElement("Stanje");

            foreach (var item in lista)
            {
                Stanje.Add(new XElement("lek_stavka",
                    new XElement("datumRok", item.DatumRok),
                    new XElement("datumUlaz", item.DatumUlaz.ToString("dd.MM.yyyy")),
                    new XElement("kolicina", item.Kolicina.ToString(provider)),
                    new XElement("kpp", item.Kpp),
                    new XElement("sifra", item.Jkl),
                    new XElement("cenaleka", item.Cena.ToString(provider))
                    ));
            }

            Stanje.Save(fileName);
        }
    }
}
