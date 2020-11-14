using EksportStanja.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Lek> GetAll(string xmlFilePath)
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

            return ConvertToLekObject(lekovi);
        }

        public ObservableCollection<Lek> GetAllCentralni(string filePath)
        {
            var lista = new List<Lek>();
            var stanje = XElement.Load(filePath).Descendants("lek_stavka");

            foreach (var item in stanje)
            {
                Lek utrosak = new Lek()
                {
                    Jkl = item.Element("sifra").Value,
                    Kolicina = Math.Round(double.Parse(item.Element("kolicina").Value, provider), 2),
                    FabrickoIme = _repo.Get(item.Element("sifra").Value, item.Element("kpp").Value)
                };

                lista.Add(utrosak);
            }

            var suma = lista.GroupBy(l => new { l.Jkl, l.FabrickoIme })
                            .Select(g => new Lek
                            {
                                Jkl = g.Key.Jkl,
                                FabrickoIme = g.Key.FabrickoIme,
                                Kolicina = g.Sum(s => s.Kolicina)
                            });

            return new ObservableCollection<Lek>(suma);
        }

        public ObservableCollection<Lek> EksportStanjaCentralniPoStavkama(string filePath)
        {
            var lista = new ObservableCollection<Lek>();
            var stanje = XElement.Load(filePath).Descendants("lek_stavka");

            foreach (var item in stanje)
            {
                Lek lek = new Lek()
                {
                    Jkl = item.Element("sifra").Value,
                    Kolicina = Math.Round(double.Parse(item.Element("kolicina").Value, provider), 2),
                    FabrickoIme = _repo.Get(item.Element("sifra").Value, item.Element("kpp").Value),
                    Kpp = item.Element("kpp").Value,
                    DatumRok = item.Element("datumRok").Value,
                    DatumUlaz = DateTime.ParseExact(item.Element("datumUlaz").Value, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    Cena = double.Parse(item.Element("cenaleka").Value, provider)
                };

                lista.Add(lek);
            }

            return lista;
        }

        private ObservableCollection<Lek> ConvertToLekObject(Dictionary<string, double> lekovi)
        {
           var lista = new ObservableCollection<Lek>();

            foreach (KeyValuePair<string, double> entry in lekovi)
            {
               
                Lek lek = new Lek()
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