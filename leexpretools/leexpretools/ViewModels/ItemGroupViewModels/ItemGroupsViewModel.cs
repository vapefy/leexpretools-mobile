using leexpretools.Models;
using leexpretools.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using leexpretools.Views.ItemGroupPages;
using Xamarin.Forms;


namespace leexpretools.ViewModels.ItemGroupViewModels {
	public class ItemGroupsViewModel : BaseViewModel {
		private ItemGroup _selectedItemGroup;

		public ObservableCollection<ItemGroup> ItemGroups { get; }
		public Command LoadCommand { get; }
		public Command AddCommand { get; }
		public Command<ItemGroup> Tapped { get; }


		public ItemGroupsViewModel() {
			Title = "Artikelgruppen";
			ItemGroups = new ObservableCollection<ItemGroup>();
			LoadCommand = new Command(async () => await ExecuteLoadItemGroupsCommand());

			Tapped = new Command<ItemGroup>(OnItemGroupSelected);

			AddCommand = new Command(OnAddItemGroup);
		}
		
		

		async Task ExecuteLoadItemGroupsCommand() {
			IsBusy = true;

			try {
				ItemGroups.Clear();
				var groups = await DataStore.GetItemGroupsAsync();
				foreach(var group in groups) {
					if (group.MarketId == GlobalManager.Instance.Market.Id) {
						ItemGroups.Add(group);
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
			SelectedItemGroup = null;
		}

		public ItemGroup SelectedItemGroup {
			get => _selectedItemGroup;
			set {
				SetProperty(ref _selectedItemGroup, value);
				OnItemGroupSelected(value);
			}
		}

		private async void OnAddItemGroup(object obj) {
			await Shell.Current.GoToAsync(nameof(NewItemGroupPage));
		}

		async void OnItemGroupSelected(ItemGroup group) {
			if(group == null)
				return;
			Console.WriteLine(group.Id);

			await Shell.Current.GoToAsync($"{nameof(ItemGroupDetailPage)}?{nameof(ItemGroupDetailViewModel.ItemGroupId)}={group.Id}");
		}
	}
}