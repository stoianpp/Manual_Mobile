using System;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Mob_Manual.MainPage;

namespace Mob_Manual
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductDetailPage : ContentPage
	{
		public ProductDetailPage (Product product)
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
			InitializeComponent ();
		}
	}
}