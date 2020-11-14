using EksportStanja.Models;
using EksportStanja.Repository;
using EksportStanja.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EksportStanja.ViewModels
{
    public class MainViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public IUtrosakRepository _utrosakRepo { get; set; }
        public ISifrarnikRepository _sifrarnikRepository { get; set; }
        public IExcelService _excelService { get; set; }
        public ObservableCollection<Lek> lista { get; set; }
        public ObservableCollection<Lek> listaStavke { get; set; }

        public MainViewModel(IUtrosakRepository utrosak, ISifrarnikRepository sifrarnikRepository, IExcelService excelService)
        {
            _utrosakRepo = utrosak;
            _sifrarnikRepository = sifrarnikRepository;
            _excelService = excelService;
        }



       
    }
}