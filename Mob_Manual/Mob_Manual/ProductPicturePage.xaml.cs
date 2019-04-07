using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mob_Manual.MainPage;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace Mob_Manual
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductPicturePage : ContentPage
	{
        public ProductPicturePage(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException();
            }

            Image image = new Image();
            var stream = new MemoryStream(product.Image);
            image.Source = ImageSource.FromStream(() => { return stream; });

            product.Photo = image.Source;
            BindingContext = product;
            InitializeComponent();
        }
    }
}