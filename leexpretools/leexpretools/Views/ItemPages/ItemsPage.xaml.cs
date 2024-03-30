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
using leexpretools.ViewModels.ItemGroupViewModels;
using leexpretools.ViewModels.ItemViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace leexpretools.Views.ItemPages {
	public partial class ItemsPage : ContentPage {
		ItemsViewModel _viewModel;

		public ItemsPage() {
			InitializeComponent();

			BindingContext = _viewModel = new ItemsViewModel();
		}

		protected override void OnAppearing() {
			base.OnAppearing();
			_viewModel.OnAppearing();
		}
	}
}