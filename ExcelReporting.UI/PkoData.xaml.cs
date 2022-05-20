using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ExcelReporting.Client;
using ExcelReporting.Client.Pko;

namespace ExcelReporting.UI
{
    public partial class PkoData : UserControl
    {
        private readonly PkoDataTracker state = new PkoDataTracker();
        public PkoData()
        {
            InitializeComponent();
            DataContext = state;
            LoadFileButton.Click += OnLoadFileButtonClick;
            CreateNextButton.Click += OnCreateNextButtonClick;
        }

        private void OnLoadFileButtonClick(object sender, RoutedEventArgs args)
        {
            var pathToFile = state.PathToFile;
            var allBytes = File.ReadAllBytes(pathToFile);
            var operationResult = RemoteServerClient.Pko.ParseReport(new PkoExcelReportParseRequest
            {
                ExcelContent = allBytes
            });
            if (operationResult.StatusCode != 200)
            {
                MessageBox.Show("ERROR " + operationResult.ErrorDescription);
            }
            else
            {
                var result = operationResult.Result;
                state.ShopAddress = result.ShopAddress;
                state.DocumentNumber = result.LastDocumentNumber + 1;
                var nextDay = result.LastComplicationDate.ToDateTime().AddDays(1);
                state.ComplicationDate = new Date(nextDay).ToString();
                state.ZCauseNumber = result.LastZCauseNumber + 1;
                state.AcceptedByPersons = new ObservableCollection<string>(result.AcceptedByPersons);
                AcceptedByPersonsComboBox.SelectedValue = state.AcceptedByPersons.Last();
            }
        }

        private void OnCreateNextButtonClick(object sender, RoutedEventArgs args)
        {
            var request = new PkoCalculateNextRequest()
            {
                Debit = new Currency
                {
                    Kopecks = state.DebitKop,
                    Roubles = state.DebitRub
                },
                ComplicationDate = Date.Parse(state.ComplicationDate),
                DocumentNumber = state.DocumentNumber,
                ExcelContent = File.ReadAllBytes(state.PathToFile),
                AcceptedByPerson = AcceptedByPersonsComboBox.Text,
                ZCauseNumber = state.ZCauseNumber
            };
            var operationResult = RemoteServerClient.Pko.CalculateNext(request);
            File.WriteAllBytes(state.PathToFile, operationResult.Result.ExcelContent);
        }
    }
}