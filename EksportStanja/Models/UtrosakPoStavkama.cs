using System;
using System.ComponentModel.DataAnnotations;

namespace EksportStanja.Models
{
    public class UtrosakPoStavkama : Utrosak
    {
        [Display(Name = "Cena", Order = 7)]
        public double CenaLeka { get; set; }

        [Display(Name = "DatumRok", Order = 5)]
        public string DatumRok { get; set; }

        [Display(Name = "DatumUlaz", Order = 4)]
        public DateTime DatumUlaz { get; set; }

        [Display(Name = "KPP", Order = 3)]
        public string Kpp { get; set; }

        [Display(Name = "Ukupno", Order = 8)]
        public double Ukupno => Math.Round(CenaLeka * Kolicina, 2);
    }
}