using Mob_Manual.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace Mob_Manual
{
    public partial class MainPage : ContentPage
    {
        private readonly DataIn retrievedData;
        private readonly SubCategory subCategory; 

        public MainPage(DataIn data, SubCategory subCat)
        {
            retrievedData = data;
            subCategory = subCat;
            InitializeComponent();
            Title = subCat.Name;
            VisualizeProducts(data);
        }

        public void VisualizeProducts(DataIn data, string searchText = "no text")
        {
            var listData = new List<Product>();
            foreach (var product in data.data)
            {
                if (product.SubCategoryId == subCategory.Id)
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
            }

            if (searchText != "no text")
            {
                listData = listData.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).ToList();
            }

            listView.ItemsSource = listData;
            listView.IsRefreshing = false;
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var product = menuItem.CommandParameter as Product;
            //DisplayAlert("Call",product.Name, "Ok");
        }

        private void listView_Refreshing(object sender, EventArgs e)
        {
            VisualizeProducts(retrievedData);
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
            var product = e.SelectedItem as Product;
            await Navigation.PushAsync(new ProductDetail(product));
            listView.SelectedItem = null;
        }
    }
}
