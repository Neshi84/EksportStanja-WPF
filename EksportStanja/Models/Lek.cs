using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace EksportStanja.Models
{
    public class Lek : INotifyPropertyChanged
    {
        [Display(Name = "JKL", Order = 1)]

        public string Jkl { get; set; }

        [Display(Name = "Fabricko ime", Order = 2)]

        public string FabrickoIme { get; set; }

        [Display(Name = "Kolicina", Order = 6)]

        public double Kolicina { get; set; }

        [Display(Name = "Cena", Order = 7)]
        public double Cena { get; set; }

        [Display(Name = "DatumRok", Order = 5)]
        public string DatumRok { get; set; } = "31.12.2021";

        [Display(Name = "DatumUlaz", Order = 4)]
        public DateTime DatumUlaz { get; set; } = DateTime.ParseExact("01.01.2020", "dd.MM.yyyy", CultureInfo.InvariantCulture);

        [Display(Name = "KPP", Order = 3)]
        public string Kpp { get; set; } = "062";

        [Display(Name = "Ukupno", Order = 8)]
        public double Ukupno => Math.Round(Cena * Kolicina, 2);

        public event PropertyChangedEventHandler PropertyChanged;
    }
}