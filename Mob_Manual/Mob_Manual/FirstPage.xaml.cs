﻿using Mob_Manual.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mob_Manual
{
    public partial class FirstPage : ContentPage
    {
        private DataIn retrievedData;
        private readonly string tokenCode;
        private IFileSystem fileSystem;

        public FirstPage(string token)
        {
            tokenCode = token;
            InitializeComponent();
            fileSystem = DependencyService.Get<IFileSystem>();
            RefreshDataAsync();
        }

        public async void RefreshDataAsync(string searchText = "no text")
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenCode);
            //to implement with local stored timestamp if null => then 0
            client.DefaultRequestHeaders.Add("Timestamp",await GetCurrentTimestamp());
            //var uri = new Uri("http://stoianpp-001-site1.htempurl.com/api/values");
            var uri = new Uri("http://stoianpp-001-site2.htempurl.com/api/values");

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                DataIn data = JsonConvert.DeserializeObject<DataIn>(content);
                retrievedData = data;
                var newTimestamp = data.lastUpdated;
                if (data.longTimestamp.Split(new char[] { ' ' }).Length > 1)
                {
                    newTimestamp += " " + data.longTimestamp.Split(new char[] { ' ' })[1];
                }

                await fileSystem.WriteTextAsync("datain", content);
                await fileSystem.WriteTextAsync("timestamp", newTimestamp);

                Indicator.IsRunning = false;
                Indicator.IsVisible = false;
                VisualizeProducts(data);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var content = await fileSystem.GetFile("datain");
                DataIn data = JsonConvert.DeserializeObject<DataIn>(content);

                retrievedData = data;

                Indicator.IsRunning = false;
                Indicator.IsVisible = false;
                VisualizeProducts(data);
            }
        }

        public void VisualizeProducts(DataIn data, string searchText = "no text")
        {
            var listData = new List<SubCategory>();
            foreach (var item in data.subCats)
            {
                Image image = new Image();
                var stream = new MemoryStream(item.Image);
                image.Source = ImageSource.FromStream(() => { return stream; });
                var currentSubCategory = new SubCategory
                {
                    Id = item.Id,
                    Category = item.Category,
                    CategoryId = item.CategoryId,
                    Photo = image.Source,
                    Name = item.Name
                };
                listData.Add(currentSubCategory);
            }

            if (searchText != "no text")
            {
                listData = listData.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).ToList();
            }

            initialListView.ItemsSource = listData;
            initialListView.IsRefreshing = false;
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var subCategory = menuItem.CommandParameter as SubCategory;
            await Navigation.PushAsync(new MainPage(retrievedData, subCategory));
            //var text = new HtmlWebViewSource();
            //text.Html = product.LangText;
            //DisplayAlert("Call", product.Name, "Ok");
        }

        private void listView_Refreshing(object sender, EventArgs e)
        {
            RefreshDataAsync();
        }


        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            VisualizeProducts(retrievedData, e.NewTextValue);
        }

        private async void listView_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var subCategory = e.SelectedItem as SubCategory;
            await Navigation.PushAsync(new MainPage(retrievedData, subCategory));

            initialListView.SelectedItem = null;
        }

        public async Task<string> GetCurrentTimestamp()
        {
            try
            {
                var timestampStr = await fileSystem.GetFile("timestamp");
                return timestampStr;
            }
            catch (Exception)
            {

                return "0";
            }
        }


        protected override bool OnBackButtonPressed()
        {
            return true; // true prevent navigation back and false to allow      
        }

        void LogoutButtonClicked(object sender, EventArgs e)
        {
            Application.Current.Properties["user"] = null;
            Application.Current.Properties["pass"] = null;
            Application.Current.MainPage = new IntroductionPage();
        }
    }
}
