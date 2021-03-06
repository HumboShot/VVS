﻿using SQLite;
using System;
using System.Diagnostics;
using VVS.Database;
using VVS.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VVS.Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private int _identifier;
        private Replacement _replacement;
        private Report _report = new Report();

        public ReportPage(Replacement currentReplacement, int identifier)
        {
            if(currentReplacement == null)
                throw new ArgumentNullException(nameof(currentReplacement));
            InitializeComponent();
            _connection = DependencyService.Get<ISQLiteDB>().GetConnection();
            _replacement = currentReplacement;
            _identifier = identifier;
            _report.Time = DateTime.Now;

            if (_identifier == 1)
            {                
                _replacement.BeforeReport = _report;                
            }
            if (_identifier == 4)
            {                
                _replacement.AfterReport = _report;             
            }
        }

        private async void ButtonTakePicture_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PicturePage(_replacement, _identifier));
        }

        private async void ButtonSaveReport_Clicked(object sender, EventArgs e)
        {
            //sets report text on obj.
            string comment = ReportComment.Text;

            if (String.IsNullOrWhiteSpace(_report.PicturePath))
            {
                await DisplayAlert("Billede ikke vedhæftet", "Tag venligst billede igen", "OK");
                return;
            }
            else if (String.IsNullOrWhiteSpace(comment))
            {
                await DisplayAlert("Kommentar ikke vedhæftet", "Skriv venligst en kommentar", "OK");
                return;
            }

            _report.Comment = comment;
            try
            {
                await _connection.InsertAsync(_report);
                Debug.WriteLine("New Report ID: {0}", _report.Id);

                if (_identifier == 1)
                {
                    _replacement.Status = 2;
                    _replacement.BeforeReportId = _report.Id;
                }
                if (_identifier == 4)
                {
                    _replacement.Status = 5;
                    _replacement.AfterReportId = _report.Id;
                }

                await _connection.UpdateAsync(_replacement);                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERRRORR" + ex.Message);
            }
            await Navigation.PopAsync();
        }  
    }
}
