using leexpretools.Models;
using leexpretools.ViewModels;
using leexpretools.ViewModels.AreaViewModels;
using leexpretools.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace leexpretools.Views.AreaPages {
	public partial class AreasPage : ContentPage {
		AreasViewModel _viewModel;

		public AreasPage() {
			InitializeComponent();

			BindingContext = _viewModel = new AreasViewModel();
		}

		protected override void OnAppearing() {
			base.OnAppearing();
			_viewModel.OnAppearing();
		}
	}
}