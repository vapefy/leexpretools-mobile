using System;
using System.ComponentModel;
using leexpretools.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace leexpretools.Views {
	public partial class AboutPage : ContentPage {

		private AboutViewModel _viewmodel;
		
		public AboutPage() {
			InitializeComponent();
			_viewmodel = new AboutViewModel();
			BindingContext = _viewmodel;
		}
		
		protected override void OnAppearing() {
			base.OnAppearing();
			_viewmodel.OnAppearing();
		}
	}
}