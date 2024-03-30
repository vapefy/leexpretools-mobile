using leexpretools.Models;
using leexpretools.ViewModels;
using leexpretools.ViewModels.AreaViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using leexpretools.ViewModels.ItemGroupViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace leexpretools.Views.ItemGroupPages {
	public partial class NewItemGroupPage : ContentPage {
		public NewItemGroupPage() {
			InitializeComponent();
			BindingContext = new NewItemGroupViewModel();
		}
	}
}