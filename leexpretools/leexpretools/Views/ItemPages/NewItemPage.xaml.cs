using leexpretools.Models;
using leexpretools.ViewModels;
using leexpretools.ViewModels.AreaViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using leexpretools.ViewModels.ItemGroupViewModels;
using leexpretools.ViewModels.ItemViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace leexpretools.Views.ItemPages {
	public partial class NewItemPage : ContentPage {
		public NewItemPage() {
			InitializeComponent();
			var model = new NewItemViewModel();
			BindingContext = model;

			areaPicker.ItemsSource = model.Areas;
			itemGroupPicker.ItemsSource = model.ItemGroups;
			
			MessagingCenter.Subscribe<LoginViewModel, MessageBoxArgs>(this, "ShowMessageBox", (sender, args) => {
				DisplayAlert(args.Title, args.Message, "OK");
			});
		}
		
		~NewItemPage()
		{
			MessagingCenter.Unsubscribe<LoginViewModel, MessageBoxArgs>(this, "ShowMessageBox");
		}
	}
}