using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EksportStanja.Models
{
    public class Utrosak
    {
        [Display(Name = "JKL", Order = 1)]
        
        public string Jkl { get; set; }

        [Display(Name = "Fabricko ime", Order = 2)]
        
        public string FabrickoIme { get; set; }

        [Display(Name = "Kolicina", Order = 6)]
        
        public double Kolicina { get; set; }
    }
}