using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            var layout = new StackLayout();
            Content = layout;

            foreach (var product in data.data)
            {
                Image image = new Image();
                var stream = new MemoryStream(product.Image);
                image.Source =  ImageSource.FromStream(() => { return stream; });
                layout.Children.Add(image);
            }
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
    }
}
