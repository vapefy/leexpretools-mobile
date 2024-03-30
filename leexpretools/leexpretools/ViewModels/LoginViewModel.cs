using leexpretools.Views;
using System;
using System.Collections.Generic;
using System.Text;
using leexpretools.Models;
using leexpretools.Services;
using Xamarin.Forms;

namespace leexpretools.ViewModels {
	public class LoginViewModel : BaseViewModel {
		public Command LoginCommand { get; }
		public LoginViewModel() {
			Title = "Einloggen";
			LoginCommand = new Command(OnLoginClicked);
			LoadCredentials();
		}
		
		private string marketId = string.Empty;
		public string MarketId {
			get { return marketId; }
			set { SetProperty(ref marketId, value); }
		}
		
		private string username = string.Empty;
		public string Username {
			get { return username; }
			set { SetProperty(ref username, value); }
		}
		
		string password = string.Empty;
		public string Password {
			get { return password; }
			set { SetProperty(ref password, value); }
		}
		
		private bool _isChecked;
		public bool SaveCredentialsChecked {
			get { return _isChecked; }
			set
			{
				if (_isChecked != value)
				{
					_isChecked = value;
					OnPropertyChanged(nameof(SaveCredentialsChecked));
				}
			}
		}

		private async void OnLoginClicked(object obj) {
			int marketIdent = 0;

			try {
				marketIdent = Int32.Parse(MarketId.Replace("#", ""));

			} catch (FormatException ex) {
				MessagingCenter.Send(this, "ShowMessageBox", new MessageBoxArgs() { Title = "Error", Message = "Bitte eine gültige Markt ID eingeben." });
			} finally {
				string status = await DataStore.CheckLoginCredentials(marketIdent, Username, Password);

				if (status.Equals("login succeed")) {
					SaveCredentials(SaveCredentialsChecked);
					var market = await GlobalManager.Instance.DataStore.GetMarketAsync(marketIdent);

					GlobalManager.Instance.Market = market;
					GlobalManager.Instance.User = Username;
					Application.Current.MainPage = new AppShell();  
					await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
				} else {
					MessagingCenter.Send(this, "ShowMessageBox", new MessageBoxArgs() { Title = "Error", Message = status });
				}
			}
		}

		private async void SaveCredentials(bool savePassword) {
			await LocalDataStore.SaveData("username", Username);
			await LocalDataStore.SaveData("market_id", MarketId);

			if (savePassword) {
				await LocalDataStore.SaveData("password", Password);
			} else {
				await LocalDataStore.SaveData("password", null);
			}
		}

		private async void LoadCredentials() {
			string loadedMarketplace = await LocalDataStore.GetData("market_id");
			string loadedUsername = await LocalDataStore.GetData("username");
			string loadedPassword = await LocalDataStore.GetData("password");

			if (loadedMarketplace != null && loadedUsername != null && loadedPassword != null) {
				MarketId = loadedMarketplace;
				Username = loadedUsername;
				Password = loadedPassword;
			}
		}
	}
}
