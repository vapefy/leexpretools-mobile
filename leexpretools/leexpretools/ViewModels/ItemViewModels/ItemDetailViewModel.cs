using leexpretools.Models;
using leexpretools.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace leexpretools.ViewModels.ItemViewModels {
	[QueryProperty(nameof(ItemId), nameof(ItemId))]
	public class ItemDetailViewModel : BaseViewModel {
		private int _itemId;
		private string _name;
		private DateTime _expires;
		private string _ean;
		private string _description;
		private Area _selectedArea;
		private ItemGroup _selectedItemGroup;
		private string _price;
		private string _zone;
		
		public Command<Area> TappedArea { get; }
		public Command<ItemGroup> TappedItemGroup { get; }
		public Command<string> TappedZone { get; }
		
		public ObservableCollection<Area> Areas;
		public ObservableCollection<ItemGroup> ItemGroups;
		

		public Command SaveCommand { get; }
		public Command DeleteCommand { get; }
		public ItemDetailViewModel() {
			Title = "Artikel bearbeiten";
			DeleteCommand = new Command(DeleteItemButton_OnClicked);
			SaveCommand = new Command(SaveItemButton_OnClicked);
			
			Areas = new ObservableCollection<Area>();
			LoadAreas();
			ItemGroups = new ObservableCollection<ItemGroup>();
			LoadItemGroups();

		}
		
		public int Id { get; set; }
		
		public int ItemId {
			get => _itemId;
			set {
				_itemId = value;
				LoadItem(value);
			}		
		}
		public string Name {
			get => _name;
			set => SetProperty(ref _name, value);
		}
		
		public DateTime Expires {
			get => _expires;
			set => _expires = value;
		}

		public string Ean {
			get => _ean;
			set => SetProperty(ref _ean, value);
		}
		
		public string Description {
			get => _description;
			set => SetProperty(ref _description, value);
		}
        
		public string Price {
			get => _price;
			set => SetProperty(ref _price, value);
		}
        
		public string SelectedZone {
			get => _zone;
			set {
				if (_zone != value) {
					_zone = value;
					OnPropertyChanged(nameof(SelectedZone));
				}
			}
            
		}
		
		public ItemGroup SelectedItemGroup {
			get => _selectedItemGroup;
			set {
				if (_selectedItemGroup != value) {
					_selectedItemGroup = value;
					OnPropertyChanged(nameof(SelectedItemGroup));
				}
			}
		}
        
		public Area SelectedArea {
			get => _selectedArea;
			set {
				if (_selectedArea != value) {
					_selectedArea = value;
					OnPropertyChanged(nameof(SelectedArea));
				}
			}
		}

		public void OnAreaSelected(Area area) {
			SelectedArea = area;
		}
        
		public void OnZoneSelected(string zone) {
			SelectedZone = zone;
		}
        
		public void OnItemGroupSelected(ItemGroup group) {
			SelectedItemGroup = group;
		}


		private async void SaveItemButton_OnClicked() {
			bool expired;
			if (Expires <= DateTime.Now) {
				expired = false;
			} else {
				expired = true;
			}
			
			
			var updatedItem = new Item() {
				Id = 0,
				Name = Name,
				Expires = Expires,
				Ean = Ean,
				Description = Description,
				Area = SelectedArea,
				ItemGroup = SelectedItemGroup,
				Expired = expired,
				Price = float.Parse(Price),
				Zone = SelectedZone
			};

			await DataStore.UpdateItemAsync(updatedItem);
			
			await Shell.Current.GoToAsync("..");
		}

		private async void DeleteItemButton_OnClicked(object obj) {
			await DataStore.DeleteItemAsync(ItemId);
			await Shell.Current.GoToAsync("..");
		}

		public async void LoadItem(int itemId) {
			try {
				var item = await DataStore.GetItemAsync(itemId);
				Id = item.Id;
				Name = item.Name;
				Expires = item.Expires;
				Ean = item.Ean;
				Description = item.Description;
				SelectedArea = item.Area;
				SelectedItemGroup = item.ItemGroup;
				Price = item.Price.ToString();
				SelectedZone = item.Zone;
			} catch(Exception) {
				Debug.WriteLine("Failed to Load Item");
			}
		}
		
		async Task LoadAreas() {
			IsBusy = true;

			try {
				Areas.Clear();
				var areas = await DataStore.GetAreasAsync();
				foreach(var area in areas) {
					if (area.MarketId == GlobalManager.Instance.Market.Id) {
						Areas.Add(area);
					}
				}
			} catch(Exception ex) {
				Debug.WriteLine(ex);
			} finally {
				IsBusy = false;
			}
		}
        
		async Task LoadItemGroups() {
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
	}
}
