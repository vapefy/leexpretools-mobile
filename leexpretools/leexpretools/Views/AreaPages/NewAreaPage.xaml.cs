using leexpretools.Models;
using leexpretools.ViewModels;
using leexpretools.ViewModels.AreaViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace leexpretools.Views.AreaPages {
	public partial class NewAreaPage : ContentPage {
		public NewAreaPage() {
			InitializeComponent();
			BindingContext = new NewAreaViewModel();
		}
	}
}