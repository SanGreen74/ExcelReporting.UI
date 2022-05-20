using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ExcelReporting.UI.Pages.Pko
{
    public class PkoDataState : INotifyPropertyChanged
    {
        private string pathToFile;
        public string PathToFile
        {
            get => pathToFile;
            set => UpdateValue(nameof(PathToFile), ref pathToFile, ref value);
        }

        private bool isFileLoaded;
        public bool IsFileLoaded
        {
            get => isFileLoaded;
            set => UpdateValue(nameof(IsFileLoaded), ref isFileLoaded, ref value);
        }

        private string shopAddress;

        public string ShopAddress
        {
            get => shopAddress;
            set => UpdateValue(nameof(ShopAddress), ref shopAddress, ref value);
        }

        private int? documentNumber;

        public int? DocumentNumber
        {
            get => documentNumber;
            set => UpdateValue(nameof(DocumentNumber), ref documentNumber, ref value);
        }

        private string complicationDate;
        public string ComplicationDate
        {
            get => complicationDate;
            set => UpdateValue(nameof(ComplicationDate), ref complicationDate, ref value);
        }

        private int? zCauseNumber;
        public int? ZCauseNumber
        {
            get => zCauseNumber;
            set => UpdateValue(nameof(ZCauseNumber), ref zCauseNumber, ref value);
        }

        private ObservableCollection<string> acceptedByPersons;
        public ObservableCollection<string> AcceptedByPersons
        {
            get => acceptedByPersons; 
            set => UpdateValue(nameof(AcceptedByPersons), ref acceptedByPersons, ref value);
        }

        private int? debitRub;

        public int? DebitRub
        {
            get => debitRub;
            set => UpdateValue(nameof(DebitRub), ref debitRub, ref value);
        }

        private int? debitKop;

        public int? DebitKop
        {
            get => debitKop;
            set => UpdateValue(nameof(DebitKop), ref debitKop, ref value);
        }

        private string acceptedByPerson;

        public string AcceptedByPerson
        {
            get => acceptedByPerson;
            set => UpdateValue(nameof(AcceptedByPerson), ref acceptedByPerson, ref value);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateValue<T>(string propName, ref T prop, ref T value)
        {
            prop = value;
            OnPropertyChanged(propName);
        }
    }
}