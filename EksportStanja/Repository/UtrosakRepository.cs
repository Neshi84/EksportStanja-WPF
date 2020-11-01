using EksportStanja.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace EksportStanja.Repository
{
    public class UtrosakRepository : IUtrosakRepository
    {
        private ISifrarnikRepository _repo { get; set; }

        private NumberFormatInfo provider = new NumberFormatInfo()
        {
            NumberDecimalSeparator = "."
        };

        public UtrosakRepository(ISifrarnikRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Utrosak> GetAll(string xmlFilePath)
        {
            Dictionary<string, double> lekovi = new Dictionary<string, double>();

            using (XmlReader reader = XmlReader.Create(xmlFilePath))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element
                        && reader.Name == "JKL")
                    {
                        var name = XNode.ReadFrom(reader) as XElement;

                        reader.ReadToNextSibling("StanjeZaliha");
                        var zalihe = XNode.ReadFrom(reader) as XElement;

                        if (!lekovi.ContainsKey(name.Value.ToString()))
                        {
                            lekovi.Add(name.Value.ToString(), Math.Round(double.Parse(zalihe.Value.ToString(), provider), 2));
                        }
                        else
                        {
                            lekovi[name.Value.ToString()] += Math.Round(double.Parse(zalihe.Value.ToString(), provider), 2);
                        }
                    }
                }
            }

            return convertToLekObject(lekovi);
        }

        public IEnumerable<Utrosak> GetAllCentralni(string filePath)
        {
            var lista = new List<Utrosak>();
            var stanje = XElement.Load(filePath).Descendants("lek_stavka");

            foreach (var item in stanje)
            {
                Utrosak utrosak = new Utrosak()
                {
                    Jkl = item.Element("sifra").Value,
                    Kolicina = Math.Round(double.Parse(item.Element("kolicina").Value, provider), 2),
                    FabrickoIme = _repo.Get(item.Element("sifra").Value, item.Element("kpp").Value)
                };

                lista.Add(utrosak);
            }

            var suma = lista.GroupBy(l => new { l.Jkl, l.FabrickoIme })
                            .Select(g => new Utrosak
                            {
                                Jkl = g.Key.Jkl,
                                FabrickoIme = g.Key.FabrickoIme,
                                Kolicina = g.Sum(s => s.Kolicina)
                            });

            return suma;
        }

        public IEnumerable<UtrosakPoStavkama> eksportStanjaCentralniPoStavkama(string filePath)
        {
            var lista = new List<UtrosakPoStavkama>();
            var stanje = XElement.Load(filePath).Descendants("lek_stavka");

            foreach (var item in stanje)
            {
                UtrosakPoStavkama lek = new UtrosakPoStavkama()
                {
                    Jkl = item.Element("sifra").Value,
                    Kolicina = Math.Round(double.Parse(item.Element("kolicina").Value, provider), 2),
                    FabrickoIme = _repo.Get(item.Element("sifra").Value, item.Element("kpp").Value),
                    Kpp = item.Element("kpp").Value,
                    DatumRok = item.Element("datumRok").Value,
                    DatumUlaz = DateTime.ParseExact(item.Element("datumUlaz").Value, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    CenaLeka = double.Parse(item.Element("cenaleka").Value, provider)
                };

                lista.Add(lek);
            }

            return lista;
        }

        private List<Utrosak> convertToLekObject(Dictionary<string, double> lekovi)
        {
            List<Utrosak> lista = new List<Utrosak>();

            foreach (KeyValuePair<string, double> entry in lekovi)
            {
               
                Utrosak lek = new Utrosak()
                {
                    FabrickoIme = _repo.Get(entry.Key, "062"),
                    Jkl = entry.Key,
                    Kolicina = Math.Round(entry.Value, 2)
                };
                lista.Add(lek);
            }

            return lista;
        }
    }
}