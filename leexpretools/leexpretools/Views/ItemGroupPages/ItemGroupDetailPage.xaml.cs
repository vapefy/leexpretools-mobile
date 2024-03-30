using System;
using leexpretools.ViewModels;
using leexpretools.ViewModels.AreaViewModels;
using System.ComponentModel;
using leexpretools.ViewModels.ItemGroupViewModels;
using Xamarin.Forms;

namespace leexpretools.Views.ItemGroupPages {
	public partial class ItemGroupDetailPage : ContentPage {
		public ItemGroupDetailPage() {
			InitializeComponent();
			BindingContext = new ItemGroupDetailViewModel();
		}
	}
	
}