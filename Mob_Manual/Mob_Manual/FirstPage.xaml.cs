using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace Mob_Manual
{
    public partial class FirstPage : ContentPage
    {
        private MainPage.DataIn retrievedData;
        private readonly string tokenCode;

        public FirstPage(string token)
        {
            tokenCode = token;
            InitializeComponent();
            RefreshDataAsync();
        }

        public async void RefreshDataAsync(string searchText = "no text")
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenCode);
            var uri = new Uri("http://stoianpp-001-site1.htempurl.com/api/crud");

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                MainPage.DataIn data = JsonConvert.DeserializeObject<MainPage.DataIn>(content);
                retrievedData = data;
                VisualizeProducts(data);
            }
        }

        public void VisualizeProducts(MainPage.DataIn data)
        {
            var listData = new List<MainPage.SubCategory>();
            listData.Add(new MainPage.SubCategory { Name = "Full Product List" });
            listData.Add(new MainPage.SubCategory { Name = "Merchandising Rules" });
            listData.Add(new MainPage.SubCategory { Name = "Current Promotions" });
            foreach (var item in data.subCats)
            {
                Image image = new Image();
                var stream = new MemoryStream(item.Image);
                image.Source = ImageSource.FromStream(() => { return stream; });
                var currentSubCategory = new MainPage.SubCategory
                {
                    Id = item.Id,
                    Category = item.Category,
                    CategoryId = item.CategoryId,
                    Photo = image.Source,
                    Name = item.Name
                };
                listData.Add(currentSubCategory);
            }

            initialListView.ItemsSource = listData;
            initialListView.IsRefreshing = false;
        }

        public class DataIn
        {
            [JsonProperty("data")]
            public List<EndUserViewModel> data = new List<EndUserViewModel>();
            [JsonProperty("cats")]
            public List<Category> cats = new List<Category>();
            [JsonProperty("subCats")]
            public List<SubCategory> subCats = new List<SubCategory>();
        }

        public class EndUserViewModel
        {
            [JsonProperty("Id")]
            public int Id { get; set; }
            [JsonProperty("SubCategoryId")]
            public int SubCategoryId { get; set; }
            [JsonProperty("SubCategory")]
            public string SubCategory { get; set; }
            [JsonProperty("Name")]
            public string Name { get; set; }
            [JsonProperty("Image")]
            public byte[] Image { get; set; }
            [JsonProperty("LangText")]
            public string LangText { get; set; }
        }

        public class Category
        {
            [JsonProperty("Id")]
            public int Id { get; set; }
            [JsonProperty("Name")]
            public string Name { get; set; }
            [JsonProperty("Image")]
            public byte[] Image { get; set; }
        }

        public class SubCategory
        {
            [JsonProperty("Id")]
            public int Id { get; set; }
            [JsonProperty("Name")]
            public string Name { get; set; }
            [JsonProperty("Image")]
            public byte[] Image { get; set; }
            [JsonProperty("CategoryId")]
            public int CategoryId { get; set; }
            [JsonProperty("Category")]
            public string Category { get; set; }
            public ImageSource Photo { get; set; }
        }

        public class Product
        {
            public int Id { get; set; }
            public int SubCategoryId { get; set; }
            public string SubCategory { get; set; }
            public string Name { get; set; }
            public ImageSource Photo { get; set; }
            public string LangText { get; set; }
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var subCategory = menuItem.CommandParameter as MainPage.SubCategory;
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
            RefreshDataAsync(e.NewTextValue);
        }

        private async void listView_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            var subCategory = e.SelectedItem as MainPage.SubCategory;
            await Navigation.PushAsync(new MainPage(retrievedData, subCategory));

            initialListView.SelectedItem = null;
        }
    }
}
