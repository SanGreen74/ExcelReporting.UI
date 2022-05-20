using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ExcelReporting.Client;
using ExcelReporting.Client.Pko;
using ExcelReporting.UI.Data;

namespace ExcelReporting.UI.Pages.Pko
{
    public partial class PkoData : UserControl
    {
        private readonly PkoDataState state = new PkoDataState();
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
            var fileReadResult = FileContentProvider.TryRead(pathToFile);
            if (fileReadResult.HasError)
            {
                MessageBox.Show(fileReadResult.ErrorDescription, "Произошла ошибка", 
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            ParseFileAsync(fileReadResult);
        }

        private void ParseFileAsync(FileReadResult fileReadResult)
        {
            var loadButtonContent = LoadFileButton.Content;
            LoadFileButton.Content = "Загрузка файла...";
            LoadFileButton.IsEnabled = false;
            
            var uiSyncContext = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(() =>
                {
                    var operationResult = RemoteServerClient.Pko.ParseReport(new PkoExcelReportParseRequest
                    {
                        ExcelContent = fileReadResult.Content
                    });
                    return operationResult;
                })
                .ContinueWith(taskOperationResult =>
                {
                    var operationResult = taskOperationResult.Result;
                    LoadFileButton.Content = loadButtonContent;
                    LoadFileButton.IsEnabled = true;
                    
                    if (operationResult.StatusCode != 200)
                    {
                        MessageBox.Show($"Произошла ошибка\n{operationResult.ErrorDescription}", "Произошла ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }

                    state.IsFileLoaded = true;
                    var result = operationResult.Result;
                    state.ShopAddress = result.ShopAddress;
                    state.DocumentNumber = result.LastDocumentNumber + 1;
                    var nextDay = result.LastComplicationDate.ToDateTime().AddDays(1);
                    state.ComplicationDate = new Date(nextDay).ToString();
                    state.ZCauseNumber = result.LastZCauseNumber + 1;
                    state.AcceptedByPersons = new ObservableCollection<string>(result.AcceptedByPersons);
                    state.AcceptedByPerson = result.LastAcceptedByPerson;
                    DebitRubTextBox.Focus();
                }, uiSyncContext);
        }

        private void OnCreateNextButtonClick(object sender, RoutedEventArgs args)
        {
            var uiSyncContext = TaskScheduler.FromCurrentSynchronizationContext();
            var request = new PkoCalculateNextRequest
            {
                Debit = new Currency
                {
                    Kopecks = state.DebitKop.GetValueOrDefault(),
                    Roubles = state.DebitRub.GetValueOrDefault()
                },
                ComplicationDate = Date.Parse(state.ComplicationDate),
                DocumentNumber = state.DocumentNumber.GetValueOrDefault(),
                ExcelContent = File.ReadAllBytes(state.PathToFile),
                AcceptedByPerson = AcceptedByPersonsComboBox.Text,
                ZCauseNumber = state.ZCauseNumber.GetValueOrDefault()
            };
            var createNextButtonContent = CreateNextButton.Content;
            CreateNextButton.Content = "Формируем следующую страницу отчета...";
            CreateNextButton.IsEnabled = false;

            Task.Factory.StartNew(() =>
                {
                    var operationResult = RemoteServerClient.Pko.CalculateNext(request);
                    return operationResult;
                })
                .ContinueWith(result =>
                {
                    CreateNextButton.IsEnabled = true;
                    CreateNextButton.Content = createNextButtonContent;

                    var operationResult = result.Result;

                    if (operationResult.StatusCode != 200)
                    {
                        MessageBox.Show($"Произошла ошибка\n{operationResult.ErrorDescription}", "Произошла ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }

                    File.WriteAllBytes(state.PathToFile, operationResult.Result.ExcelContent);
                    MessageBox.Show("В отчет добавлена новая страница", "Готово");
                }, uiSyncContext);
        }
    }
}