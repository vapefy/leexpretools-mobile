using System;
using leexpretools.ViewModels;
using leexpretools.ViewModels.AreaViewModels;
using System.ComponentModel;
using leexpretools.ViewModels.ItemGroupViewModels;
using leexpretools.ViewModels.ItemViewModels;
using Xamarin.Forms;

namespace leexpretools.Views.ItemPages {
	public partial class ItemDetailPage : ContentPage {
		public ItemDetailPage() {
			InitializeComponent();
			var model = new ItemDetailViewModel();
			BindingContext = model;

			areaPicker.ItemsSource = model.Areas;
			itemGroupPicker.ItemsSource = model.ItemGroups;		
		}
	}
	
}