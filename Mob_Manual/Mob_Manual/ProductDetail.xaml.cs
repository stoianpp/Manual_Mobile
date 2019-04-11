using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mob_Manual.MainPage;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mob_Manual.Classes;

namespace Mob_Manual
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductDetail : TabbedPage
	{
		public ProductDetail (Product product)
		{
			InitializeComponent ();
            var clr = Color.FromHex("#009697");
            this.BarBackgroundColor = Color.Black;
            this.BarTextColor = clr;
            BindingContext = product;
            this.Children.Add(new ProductDetailPage(product) { Title = "Details"});
            this.Children.Add(new ProductPicturePage(product) { Title = "Picture" });
        }
	}
}