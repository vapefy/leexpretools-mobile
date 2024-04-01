using leexpretools.Services;
using leexpretools.Services;
using leexpretools.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace leexpretools {
	public partial class App : Application {

		private bool isLoggedIn;

		public App() {
			InitializeComponent();
			DependencyService.Register<DataStore>();
			InitializeGlobalManagerAsync();
			InitializeCredentials();

			if (isLoggedIn) {
				MainPage = new AppShell();
			} else {
				MainPage = new LoginPage();
			}
		}

		private async void InitializeCredentials() {
			isLoggedIn = await CheckCredentials();
		}

		public async Task<bool> CheckCredentials() {
			string marketIdString = await GlobalManager.Instance.LocalDataStore.GetData("market_id");
			string username = await GlobalManager.Instance.LocalDataStore.GetData("username");
			string password = await GlobalManager.Instance.LocalDataStore.GetData("password");

			bool isLogedIn = false;
			if(marketIdString != null && username != null && password != null) {
				int marketId = Int32.Parse(marketIdString.Replace("#", ""));
				isLoggedIn = (await GlobalManager.Instance.DataStore.CheckLoginCredentials(marketId, username, password)).Equals("login succeed");
			}

			return isLoggedIn;
		}
		
		private static async Task InitializeGlobalManagerAsync() {
			try {
				GlobalManager.Instance.LocalDataStore = new LocalDataStore();
				GlobalManager.Instance.DataStore = new DataStore();
				GlobalManager.Instance.AppLanguage = new Language();
				var language = await GlobalManager.Instance.LocalDataStore.GetData("language");
				GlobalManager.Instance.SetLanguage(language);
			} catch (Exception ex) {
				Console.WriteLine("HAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			}
		}
		
		protected override void OnStart() {
		}

		protected override void OnSleep() {
		}

		protected override void OnResume() {
		}
	}
}
