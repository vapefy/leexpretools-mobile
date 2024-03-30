using System;
using leexpretools.ViewModels;
using leexpretools.ViewModels.AreaViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace leexpretools.Views.AreaPages {
	public partial class AreaDetailPage : ContentPage {
		public AreaDetailPage() {
			InitializeComponent();
			BindingContext = new AreaDetailViewModel();
		}
	}
	
}