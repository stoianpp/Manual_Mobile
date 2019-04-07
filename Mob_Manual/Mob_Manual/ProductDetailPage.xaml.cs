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

			InitializeComponent ();

			var browser = new WebView();
			var htmlSource = new HtmlWebViewSource();
			htmlSource.Html = product.LangText;
			browser.Source = htmlSource;
			var page = new RelativeLayout();
			Content = page;
			page.Children.Add(browser, widthConstraint: Constraint.RelativeToParent(parent => parent.Width),
                heightConstraint: Constraint.RelativeToParent(parent => parent.Height));

		}
	}
}