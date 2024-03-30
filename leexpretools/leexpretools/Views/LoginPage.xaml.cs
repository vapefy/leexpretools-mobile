using leexpretools.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using leexpretools.Models;
using leexpretools.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace leexpretools.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage {
		public LoginPage() {
			InitializeComponent();
			BindingContext = new LoginViewModel();

			MessagingCenter.Subscribe<LoginViewModel, MessageBoxArgs>(this, "ShowMessageBox", (sender, args) => {
				DisplayAlert(args.Title, args.Message, "OK");
			});
		}
		
		~LoginPage()
		{
			MessagingCenter.Unsubscribe<LoginViewModel, MessageBoxArgs>(this, "ShowMessageBox");
		}
		

	}
}