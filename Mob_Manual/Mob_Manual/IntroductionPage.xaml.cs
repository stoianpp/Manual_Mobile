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
            if (Application.Current.Properties.ContainsKey("user") && Application.Current.Properties.ContainsKey("pass") && Application.Current.Properties["user"] != null && Application.Current.Properties["pass"] != null)
            {
                Next(Application.Current.Properties["user"] as string, Application.Current.Properties["pass"] as string);
            }
			InitializeComponent();
		}

        async void Next_Clicked(object sender, EventArgs e)
        {
            string name, password;
            if (Application.Current.Properties.ContainsKey("user") && Application.Current.Properties.ContainsKey("pass") && Application.Current.Properties["user"] != null && Application.Current.Properties["pass"] != null)
            {
                name = Application.Current.Properties["user"] as string;
                password = Application.Current.Properties["pass"] as string;
            }
            else
            {
                name = Mail.Text;
                password = Password.Text;
            }

            Indicator1.IsRunning = true;
            Indicator1.IsVisible = true;

            dynamic accessToken = null;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "http://stoianpp-001-site2.htempurl.com/api/values/login?username=" + name + "&password=" + password);
                //var request = new HttpRequestMessage(HttpMethod.Post, "http://stoianpp-001-site1.htempurl.com/Token");
                //request.Content = new FormUrlEncodedContent(keyValues);
                var client = new HttpClient();
                var response = await client.SendAsync(request);
                //var content = await response.Content.ReadAsStringAsync();
                var content = await response.Content?.ReadAsStringAsync();
                var jwtDynamic = JsonConvert.DeserializeObject<dynamic>(content);
                //accessToken = jwtDynamic.Value<string>("access_token");
                accessToken = jwtDynamic.Value<string>("token");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Check your network, username and password and try again, please!", "Close");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Indicator1.IsRunning = false;
                Indicator1.IsVisible = false;
            }

            Mail.Text = String.Empty;
            Password.Text = String.Empty;

            if (accessToken != null)
            {
                Application.Current.Properties["user"] = name;
                Application.Current.Properties["pass"] = password;
                Application.Current.MainPage = new NavigationPage(new FirstPage(accessToken));
                //await Navigation.PushAsync(new FirstPage(accessToken));
            }
        }

        async void Next(string name, string password)
		{
            //Indicator1.IsRunning = true;
            //Indicator1.IsVisible = true;

			dynamic accessToken = null;
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, "http://stoianpp-001-site2.htempurl.com/api/values/login?username="+ name +"&password="+ password);
                //var request = new HttpRequestMessage(HttpMethod.Post, "http://stoianpp-001-site1.htempurl.com/Token");
                //request.Content = new FormUrlEncodedContent(keyValues);
                var client = new HttpClient();
				var response = await client.SendAsync(request);
                //var content = await response.Content.ReadAsStringAsync();
                var content = await response.Content?.ReadAsStringAsync();
                var jwtDynamic = JsonConvert.DeserializeObject<dynamic>(content);
                //accessToken = jwtDynamic.Value<string>("access_token");
                accessToken = jwtDynamic.Value<string>("token");
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", "Check your network, username and password and try again, please!", "Close");
				Console.WriteLine(ex.Message);
			}
			finally
			{
				//Indicator1.IsRunning = false;
				//Indicator1.IsVisible = false;
			}

            Mail.Text = String.Empty;
			Password.Text = String.Empty;

			if (accessToken != null)
			{
                Application.Current.Properties["user"] = name;
                Application.Current.Properties["pass"] = password;
                Application.Current.MainPage = new NavigationPage(new FirstPage(accessToken));
                //await Navigation.PushAsync(new FirstPage(accessToken));
			}
		}
    }

}