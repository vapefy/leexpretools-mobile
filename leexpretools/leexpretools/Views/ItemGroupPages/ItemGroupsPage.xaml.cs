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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace leexpretools.Views.ItemGroupPages {
	public partial class ItemGroupsPage : ContentPage {
		ItemGroupsViewModel _viewModel;

		public ItemGroupsPage() {
			InitializeComponent();

			BindingContext = _viewModel = new ItemGroupsViewModel();
		}

		protected override void OnAppearing() {
			base.OnAppearing();
			_viewModel.OnAppearing();
		}
	}
}