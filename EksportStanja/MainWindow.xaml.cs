using ClosedXML;
using EksportStanja.Models;
using EksportStanja.Repository;
using EksportStanja.Services;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EksportStanja
{
    public partial class MainWindow : Window
    {
        public IUtrosakRepository _utrosakRepo { get; set; }
        public ISifrarnikRepository _sifrarnikRepository { get; set; }

        public IExcelService _excelService { get; set; }

        public List<Utrosak> lista { get; set; }

        public MainWindow(IUtrosakRepository utrosak, ISifrarnikRepository sifrarnikRepository, IExcelService excelService)
        {
            _utrosakRepo = utrosak;
            _sifrarnikRepository = sifrarnikRepository;
            _excelService = excelService;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                lista = _utrosakRepo.GetAll(openFileDialog.FileName).ToList();
                dataGrid.ItemsSource = lista;
            }
        }

        private void sacuvajBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel | *.xlsx";
            saveFileDialog.DefaultExt = "xlsx";
            if (saveFileDialog.ShowDialog() == true)
            {
                _excelService.Export(lista, saveFileDialog.FileName);
            }

            MessageBox.Show("Gotovo!");
        }

        private void centralniBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var lista = _utrosakRepo.eksportStanjaCentralniPoStavkama(openFileDialog.FileName).ToList();
                dataGrid.ItemsSource = lista;

                var col = typeof(UtrosakPoStavkama).GetProperties().OrderBy(i=> i.GetAttributes<DisplayAttribute>()[0].Order);

                dataGrid.ItemsSource = lista;
                dataGrid.AutoGenerateColumns=false;

                foreach (var item in col)
                {
                    DataGridTextColumn textColumn = new DataGridTextColumn();
                    textColumn.Header =item.Name;
                    textColumn.Binding =new Binding(item.Name);
                    dataGrid.Columns.Add(textColumn);
                }
                

            }
        }
    }
}