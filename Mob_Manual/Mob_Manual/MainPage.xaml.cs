using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace Mob_Manual
{
    public partial class MainPage : ContentPage
    {
        private readonly string tokenCode;

        public MainPage(string token)
        {
            this.tokenCode = token;
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
                DataIn data = JsonConvert.DeserializeObject<DataIn>(content);
                VisualizeProducts(data, searchText);
            }
        }

        public void VisualizeProducts(DataIn data, string searchText)
        {
            var listData = new List<Product>();
            foreach (var product in data.data)
            {
                Image image = new Image();
                var stream = new MemoryStream(product.Image);
                image.Source = ImageSource.FromStream(() => { return stream; });
                var currentProduct = new Product
                {
                    Id = product.Id,
                    SubCategory = product.SubCategory,
                    SubCategoryId = product.SubCategoryId,
                    Photo = image.Source,
                    Image = product.Image,
                    LangText = product.LangText,
                    Name = product.Name
                };
                listData.Add(currentProduct);
            }

            if (searchText != "no text")
            {
                listData = listData.Where(x => x.Name.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            listView.ItemsSource = listData;
            listView.IsRefreshing = false;
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
            [JsonProperty("Name")]
            public string Name { get; set; }
            [JsonProperty("Image")]
            public byte[] Image { get; set; }
        }

        public class SubCategory
        {
            [JsonProperty("Name")]
            public string Name { get; set; }
            [JsonProperty("Image")]
            public byte[] Image { get; set; }
        }

        public class Product
        {
            public int Id { get; set; }
            public int SubCategoryId { get; set; }
            public string SubCategory { get; set; }
            public string Name { get; set; }
            public ImageSource Photo { get; set; }
            public byte[] Image { get; set; }
            public string LangText { get; set; }
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var product = menuItem.CommandParameter as Product;
            //var text = new HtmlWebViewSource();
            //text.Html = product.LangText;
            DisplayAlert("Call",product.Name, "Ok");
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
            var product = e.SelectedItem as Product;
            await Navigation.PushAsync(new ProductDetailPage(product));
            listView.SelectedItem = null;
        }
    }
}
