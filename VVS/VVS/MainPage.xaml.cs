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
        private ObservableCollection<Replacement> _replacements;
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

            if (_replacements.Count < 1)
            {
                await _connection.InsertAllAsync(gen.ListMeters);
                await _connection.InsertAllAsync(gen.ListLocations);
                await _connection.InsertAllAsync(gen.ListReplacements);
                await LoadData();
            }

            replacementsListView.ItemsSource = _replacements;

            base.OnAppearing();
        }

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

            var replacements = await _connection.Table<Replacement>().ToListAsync();
            var locations = await _connection.Table<Location>().ToListAsync();
            foreach (var item in replacements)
            {
                item.Location = locations.Find(x => x.Id == item.LocId);
            }
            _replacements = new ObservableCollection<Replacement>(replacements);
            Debug.WriteLine("Loaded " + _replacements.Count + " replacements");
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

        private async void Debug_OnClicked(object sender, EventArgs e)
        {
            try
            {   
                //TODO get the right replacement.             
                await Navigation.PushAsync(new ReplacementPage(new Replacement()));
            }
            catch (Exception)
            {
                Debug.WriteLine("Replacement didn't work");
                throw;
            }
        }
    }
}
