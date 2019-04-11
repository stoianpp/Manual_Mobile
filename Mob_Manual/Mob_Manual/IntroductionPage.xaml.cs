using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
			var keyValues = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("username", Mail.Text),
				new KeyValuePair<string, string>("password", Password.Text),
				new KeyValuePair<string, string>("grant_type", "password")
			};

			dynamic accessToken = null;
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Post, "http://stoianpp-001-site1.htempurl.com/Token");
				request.Content = new FormUrlEncodedContent(keyValues);
				var client = new HttpClient();
				var response = await client.SendAsync(request);
				var content = await response.Content.ReadAsStringAsync();
				var jwtDynamic = JsonConvert.DeserializeObject<dynamic>(content);
				accessToken = jwtDynamic.Value<string>("access_token");
			}
			catch (Exception ex)
			{
				await DisplayAlert("Connection Error", "Check your network and try again later, please!", "Close");
				Console.WriteLine(ex.Message);
			}


			if (accessToken != null)
			{
				await Navigation.PushAsync(new FirstPage(accessToken));
			}
			else
			{
				await DisplayAlert("Try Again", "You have entered wrong username or/and password. Pleas, try again!", "Close");
			}
		}
	}

}