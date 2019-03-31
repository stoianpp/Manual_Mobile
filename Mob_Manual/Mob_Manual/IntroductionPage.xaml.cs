using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mob_Manual
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IntroductionPage : ContentPage
	{
		public IntroductionPage ()
		{
            InitializeComponent();
		}

		async void Next_Clicked(object sender, EventArgs e)
		{
            var email = Mail.Text;
            var password = Password.Text;
			await Navigation.PushAsync(new MainPage());
		}
	}

}