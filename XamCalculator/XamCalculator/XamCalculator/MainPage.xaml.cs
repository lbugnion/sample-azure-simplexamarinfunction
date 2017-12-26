﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamCalculator
{
	public partial class MainPage : ContentPage
	{
        private const string Url = "https://xamfunctionssample.azurewebsites.net/api/add/num1/{num1}/num2/{num2}?code=9r9jauRrThIFjGcP1nz3xRJLiriF9IAo5dmPlsLHBfS4hq0gv06E7A==";

        private HttpClient _client;

        private HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new HttpClient();
                }

                return _client;
            }
        }

		public MainPage()
		{
			InitializeComponent();

            AddButton.Clicked += async (s, e) =>
            {
                int number1 = 0, number2 = 0;

                var success = int.TryParse(Number1.Text, out number1)
                    && int.TryParse(Number2.Text, out number2);

                if (!success)
                {
                    await DisplayAlert("Error in inputs", "You must enter two integers", "OK");
                    return;
                }

                Result.Text = "Please wait...";
                Exception error = null;

                try
                {
                    var url = Url.Replace("{num1}", number1.ToString())
                        .Replace("{num2}", number2.ToString());
                    var result = await Client.GetStringAsync(url);

                    var deserialized = JsonConvert.DeserializeObject<Result>(result);

                    Result.Text = deserialized.Addition + $" {deserialized.Addition.GetType()}";
                }
                catch (Exception ex)
                {
                    error = ex;
                }

                if (error != null)
                {
                    Result.Text = "Error!!";
                    await DisplayAlert("There was an error", error.Message, "OK");
                }
            };
		}
	}
}
