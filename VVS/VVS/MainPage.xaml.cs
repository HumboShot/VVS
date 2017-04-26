using System;
using System.Diagnostics;
using VVS.Layout;
using VVS.Model;
using Xamarin.Forms;
using SQLite;
using System.Collections.ObjectModel;
using VVS.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VVS
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Replacement> _ObservableReplacementList;
        private List<Replacement> _replacementList = new List<Replacement>();
        private SQLiteAsyncConnection _connection;        
        private Replacement _selectedReplacement;

        public MainPage()
        {
            InitializeComponent();            
            _connection = DependencyService.Get<ISQLiteDB>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            GenerateData gen = new GenerateData();
            await LoadData();

            if (_replacementList.Count < 1)
            {
                await _connection.InsertAllAsync(gen.ListMeters);
                await _connection.InsertAllAsync(gen.ListLocations);
                await _connection.InsertAllAsync(gen.ListReplacements);
                await LoadData();
            }

            List<Replacement> tempList = _replacementList.FindAll(x => x.Status != 6 && x.Status != -1);
            SetObservableCollection(tempList);
            base.OnAppearing();
        }

        private void ShowNewReplacements(object sender, EventArgs e)
        {            
            List<Replacement> tempList = _replacementList.FindAll(x => x.Status != 6 && x.Status !=-1);
            SetObservableCollection(tempList);
        }

        private void ShowDoneReplacements(object sender, EventArgs e)
        {            
            List<Replacement> tempList = _replacementList.FindAll(x => x.Status == 6);
            SetObservableCollection(tempList);
        }

        private void ShowImpossibleReplacements(object sender, EventArgs e)
        {            
            List<Replacement> tempList = _replacementList.FindAll(x => x.Status == -1);
            SetObservableCollection(tempList);
        }

        //Generate Tables, Insert TestData, retrive Data from DB.
        private async Task LoadData()
        {
            try
            {
                await _connection.CreateTableAsync<Meter>();
                await _connection.CreateTableAsync<Report>();
                await _connection.CreateTableAsync<Location>();
                await _connection.CreateTableAsync<Replacement>();
            }
            catch(Exception e)
            {
                Debug.WriteLine("Fejl " + e.ToString());
            }

            _replacementList = await _connection.Table<Replacement>().ToListAsync();
            var locations = await _connection.Table<Location>().ToListAsync();
            var meters = await _connection.Table<Meter>().ToListAsync();
            var reports = await _connection.Table<Report>().ToListAsync();
            foreach (var item in _replacementList)
            {
                item.Location = locations.Find(x => x.Id == item.LocId);
                item.OldMeter = meters.Find(x => x.SerialNumber == item.OldMeterId);
                item.NewMeter = meters.Find(x => x.SerialNumber == item.NewMeterId);
                item.BeforeReport = reports.Find(x => x.Id == item.BeforeReportId);
                item.AfterReport = reports.Find(x => x.Id == item.AfterReportId);
            }
        }

        private void SetObservableCollection(List<Replacement> tempList)
        {
            _ObservableReplacementList = new ObservableCollection<Replacement>(tempList);
            replacementsListView.ItemsSource = _ObservableReplacementList;
            Debug.WriteLine("Loaded " + _ObservableReplacementList.Count + " replacements");
        }

        private async void OnReplaceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (replacementsListView.SelectedItem == null)
                return;

            _selectedReplacement = e.SelectedItem as Replacement;

            replacementsListView.SelectedItem = null;

            if (_selectedReplacement.Status !=6)
            {
                var replacementPage = new ReplacementPage(_selectedReplacement);
                await Navigation.PushAsync(replacementPage);
            }
            else
            {
                await DisplayAlert("Udskiftningen er færdig", "er registreret", "OK");
                    return;
            }

        }
    }
}
