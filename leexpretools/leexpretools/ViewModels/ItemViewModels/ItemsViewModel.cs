using leexpretools.Models;
using leexpretools.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using leexpretools.Views.ItemPages;
using Xamarin.Forms;


namespace leexpretools.ViewModels.ItemViewModels {
	public class ItemsViewModel : BaseViewModel {
		private Item _selectedItem;


		public ObservableCollection<Item> Items { get; }
		public Command LoadCommand { get; }
		public Command AddCommand { get; }
		public Command<Item> Tapped { get; }


		public ItemsViewModel() {
			Title = "Artikel";
			Items = new ObservableCollection<Item>();
			LoadCommand = new Command(async () => await ExecuteLoadItemsCommand());

			Tapped = new Command<Item>(OnItemSelected);

			AddCommand = new Command(OnAddItem);
		}
		
		

		async Task ExecuteLoadItemsCommand() {
			IsBusy = true;

			try {
				Items.Clear();
				var items = await DataStore.GetItemsAsync();
				foreach(var item in items) {
					if (item.Area.MarketId == GlobalManager.Instance.Market.Id) {
						Console.WriteLine(item.Name);
						Items.Add(item);
					}
				}
			} catch(Exception ex) {
				Debug.WriteLine(ex);
			} finally {
				IsBusy = false;
			}
		}

		public void OnAppearing() {
			IsBusy = true;
			SelectedItem = null;
		}

		public Item SelectedItem {
			get => _selectedItem;
			set {
				SetProperty(ref _selectedItem, value);
				OnItemSelected(value);
			}
		}

		private async void OnAddItem(object obj) {
			await Shell.Current.GoToAsync(nameof(NewItemPage));
		}

		async void OnItemSelected(Item item) {
			if(item == null)
				return;

			await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
		}
	}
}