using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Xamarin.Forms;

namespace Mob_Manual
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            RefreshDataAsync();
        }

        public async void RefreshDataAsync()
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
            //Indicator.IsRunning = false;
            //var scroll = new ScrollView();
            //var layout = new Grid();
            //scroll.Content = layout;
            //Content = scroll;

            //var counter = 0;
            //foreach (var product in data.data)
            //{
            //    Image image = new Image()
            //    {
            //        HeightRequest = 240,
            //        Aspect=Aspect.Fill,
            //    };
            //    var stream = new MemoryStream(product.Image);
            //    image.Source =  ImageSource.FromStream(() => { return stream; });
            //    layout.Children.Add(image,0,counter++);
            //}

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
                    LangText = product.LangText,
                    Name = product.Name
                };
                listData.Add(currentProduct);
            }

            listView.ItemsSource = listData;
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
            public string LangText { get; set; }
        }
    }
}
