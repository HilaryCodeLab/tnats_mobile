using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tnats_mobile.Services;
using tnats_mobile.Util;
using Xamarin.Forms;

namespace tnats_mobile.Views
{
    public class LoginPageCS : ContentPage
    {
        ICredentialsService storeService;
        Entry Entry_Username;
        Entry Entry_Password;
        Label messageLabel;
        public LoginPageCS()
        {
			storeService = DependencyService.Get<ICredentialsService>();

			Entry_Username = new Entry
			{
				Placeholder = "username"
			};
			Entry_Password = new Entry
			{
				IsPassword = true
			};
			messageLabel = new Label();
			var loginButton = new Button
			{
				Text = "Login"
			};
			loginButton.Clicked += LoginProcedure;

			Title = "Login Page";
			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children = {
					new Label { Text = "Username" },
					Entry_Username,
					new Label { Text = "Password" },
					Entry_Password,
					loginButton,
					messageLabel
				}
			};
		}

		async void LoginProcedure(object sender, EventArgs e)
		{
			string userName = Entry_Username.Text;
			string password = Entry_Password.Text;


			var isValid = AreCredentialsCorrect(userName, password);
			if (isValid)
			{
				bool doCredentialsExist = storeService.DoCredentialsExist();
				if (!doCredentialsExist)
				{
					storeService.SaveCredentials(userName, password);
				}

				Navigation.InsertPageBefore(new HomePage(), this);
				await Navigation.PopAsync();
			}
			else
			{
				messageLabel.Text = "Login failed";
				Entry_Password.Text = string.Empty;
			}
		}

		bool AreCredentialsCorrect(string username, string password)
		{
			return username == User_Constants.Username && password == User_Constants.Password;
		}
	}
}