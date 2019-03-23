using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;

namespace Mob_Manual
{
    public partial class FirstPage : ContentPage
    {
        public FirstPage()
        {
            InitializeComponent();
            RefreshDataAsync();
        }

        public async void RefreshDataAsync(string searchText = "no text")
        {
            var client = new HttpClient();
            var uri = new Uri("http://stoianpp-001-site1.htempurl.com/api/crud");

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                DataIn data = JsonConvert.DeserializeObject<DataIn>(content);
                VisualizeProducts(data);
            }
        }

        public void VisualizeProducts(DataIn data)
        {
            var listData = new List<SubCategory>();
            listData.Add(new SubCategory { Name = "Full Product List" });
            listData.Add(new SubCategory { Name = "Merchandising Rules" });
            listData.Add(new SubCategory { Name = "Current Promotions" });
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

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var product = menuItem.CommandParameter as Product;
            //var text = new HtmlWebViewSource();
            //text.Html = product.LangText;
            DisplayAlert("Call", product.Name, "Ok");
        }

        private void listView_Refreshing(object sender, EventArgs e)
        {
            RefreshDataAsync();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshDataAsync(e.NewTextValue);
        }
    }
}
