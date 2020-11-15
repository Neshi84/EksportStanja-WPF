using EksportStanja.Models;
using EksportStanja.Repository;
using EksportStanja.Services;
using EksportStanja.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EksportStanja
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainViewModel mainViewModel { get; set; }
        public IUtrosakRepository _utrosakRepo { get; set; }
        private IExcelService _excelService { get; set; }
        private IXmlService _xmlService { get; set; }
        public ObservableCollection<Lek> lista { get; set; }

        public ObservableCollection<Lek> listaCentralni { get; set; }

        public ObservableCollection<Lek> listaUlazi { get; set; }

        public MainWindow(IUtrosakRepository repo, IExcelService excelService, IXmlService xmlService)
        {
            _utrosakRepo = repo;
            _excelService = excelService;
            _xmlService = xmlService;
            lista = new ObservableCollection<Lek>();

            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                lista = _excelService.GetOdeljenske(openFileDialog.FileName);
            }
        }

        private void sacuvajBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "XML-File | *.xml",
                FileName = "StanjeOsigurani.xml"
            };
            List<Lek> nepostojeci = new List<Lek>();
            List<Lek> nereseni = new List<Lek>();
            foreach (var item in lista)
            {
                if (listaCentralni.Any(i => i.Jkl == item.Jkl && i.Kpp == "062"))
                {
                    listaCentralni.Where(c => c.Jkl == item.Jkl).Aggregate((s1, s2) => s1.DatumUlaz > s2.DatumUlaz ? s1 : s2).Kolicina += item.Kolicina;
                }
                else
                {
                    if (listaUlazi.Any(i => i.Jkl == item.Jkl && i.Kpp == "062"))
                    {
                        var nep = listaUlazi.Where(c => c.Jkl == item.Jkl && c.Kpp == "062").Aggregate((s1, s2) => s1.DatumUlaz > s2.DatumUlaz ? s1 : s2);
                        nep.Kolicina = item.Kolicina;

                        listaCentralni.Add(nep);
                        nepostojeci.Add(nep);
                    }
                    else
                    {
                        listaCentralni.Add(item);
                        nereseni.Add(item);
                    }
                }
            }

            lista = listaCentralni;

            _excelService.Export<Lek>(listaCentralni.ToList(), "StanjeUkupno.xlsx");

            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                _xmlService.saveXml(listaCentralni.ToList(), saveFileDialog.FileName);
            }
        }

        private void centralniBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                listaCentralni = _utrosakRepo.EksportStanjaCentralniPoStavkama(openFileDialog.FileName);
            }
        }

        private void ukupnoBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                listaUlazi = _utrosakRepo.EksportStanjaCentralniPoStavkama(openFileDialog.FileName);
            }
        }
    }
}