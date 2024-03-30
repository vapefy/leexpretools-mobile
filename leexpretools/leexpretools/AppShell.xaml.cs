using leexpretools.ViewModels;
using leexpretools.Views;
using leexpretools.Views.AreaPages;
using System;
using System.Collections.Generic;
using leexpretools.Views.ItemGroupPages;
using leexpretools.Views.ItemPages;
using Xamarin.Forms;

namespace leexpretools {
	public partial class AppShell : Xamarin.Forms.Shell {
		public AppShell() {
			InitializeComponent();
			Routing.RegisterRoute(nameof(AreaDetailPage), typeof(AreaDetailPage));
			Routing.RegisterRoute(nameof(NewAreaPage), typeof(NewAreaPage));

			Routing.RegisterRoute(nameof(ItemGroupDetailPage), typeof(ItemGroupDetailPage));
			Routing.RegisterRoute(nameof(NewItemGroupPage), typeof(NewItemGroupPage));
			
			Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
			Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
			
			
		}

		private async void OnMenuItemClicked(object sender, EventArgs e) {
			await Shell.Current.GoToAsync("//LoginPage");
		}
	}
}
