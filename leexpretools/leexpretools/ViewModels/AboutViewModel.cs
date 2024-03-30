using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using leexpretools.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace leexpretools.ViewModels {
	public class AboutViewModel : BaseViewModel {
		
		public Command NextCommand { get; }
		public Command BackCommand { get; }

		public AboutViewModel() {
			Title = "ExpireTool";
			HasItems = true;
			NewDate = DateTime.Now;
			LoadInformation();
			LoadNextYellowItem();
			NextCommand = new Command(async () => await OnNextClicked());
			BackCommand = new Command(OnBackClicked);


		}

		private string _marketplace;
		private string _description;
		private string _loggedInUser;
		private string _savedItems;
		private string _yellowItems;
		private string _redItems;
		private string _expiredItems;
		private string _street;
		private string _city;
		private string _plz;
		private string _country;
		private string _toRebate;
		private string _name;
		private string _ean;
		
		public bool HasItems { get; set; }
		
		
		public Item CurrentItem { set; get; }
		public Item LastItem { set; get; }
		
		public string ToRebate {
			get => _toRebate;
			set => SetProperty(ref _toRebate, value);
		}
		
		public string Name {
			get => _name;
			set => SetProperty(ref _name, value);
		}
		
		public string Ean {
			get => _ean;
			set => SetProperty(ref _ean, value);
		}

		public DateTime NewDate { get; set; }

		public string MarketPlace {
			get => _marketplace;
			set => SetProperty(ref _marketplace, value);
		}
		
		public string Description {
			get => _description;
			set => SetProperty(ref _description, value);
		}
		
		public string LoggedInUser {
			get => _loggedInUser;
			set => SetProperty(ref _loggedInUser, value);
		}
		
		public string SavedItems {
			get => _savedItems;
			set => SetProperty(ref _savedItems, value);
		}
		
		public string YellowFlaggedItems {
			get => _yellowItems;
			set => SetProperty(ref _yellowItems, value);
		}
		
		public string RedFlaggedItems {
			get => _redItems;
			set => SetProperty(ref _redItems, value);
		}
		
		public string ExpiredItems {
			get => _expiredItems;
			set => SetProperty(ref _expiredItems, value);
		}
		
		public string Street {
			get => _street;
			set => SetProperty(ref _street, value);
		}
		
		public string City {
			get => _city;
			set => SetProperty(ref _city, value);
		}
		
		public string PostialCode {
			get => _plz;
			set => SetProperty(ref _plz, value);
		}
		
		public string Country {
			get => _country;
			set => SetProperty(ref _country, value);
		}
		
		public void OnAppearing() {
			HasItems = true;
			LoadInformation();
			LoadNextYellowItem();
		}

		public async Task LoadInformation() {
			var market = GlobalManager.Instance.Market;
			MarketPlace = market.Name;
			Description = market.Description;
			LoggedInUser = GlobalManager.Instance.User;
			Street = market.Location.Street + " " + market.Location.StreetNo;
			City = market.Location.City + ", " + market.Location.Zip;
			Country = market.Location.Country;
			SavedItems = (await GetItems()).ToString();
			YellowFlaggedItems = (await GetItems("yellow")).ToString();
			RedFlaggedItems = (await GetItems("red")).ToString();
			ExpiredItems = (await GetItems("expired")).ToString();

		}

		public async Task OnNextClicked() {
			CurrentItem.Expires = NewDate;
			bool status = await GlobalManager.Instance.DataStore.UpdateItemAsync(CurrentItem);
			if (status) {
				LastItem = CurrentItem;
				await LoadNextYellowItem();
			}
		}


		public void OnBackClicked() {
			LoadLastItem();
		}

		public void LoadLastItem() {
			CurrentItem = LastItem;
			LastItem = null;
			Name = CurrentItem.Name;
			Ean = CurrentItem.Ean;
			ToRebate = CurrentItem.Expires.AddDays(CurrentItem.ItemGroup.Offset).ToString("dd.MM.yyyy");
			CurrentItem = CurrentItem;
		}

		public async Task LoadNextYellowItem() {
			try {
				var item = (await GlobalManager.Instance.DataStore.GetYellowItems()).First();
				CurrentItem = item;
				Name = item.Name;
				Ean = item.Ean;
				ToRebate = item.Expires.AddDays(item.ItemGroup.Offset).ToString("dd.MM.yyyy");
				HasItems = true;
			} catch (InvalidOperationException ex) {
				HasItems = false;
			}
		}

		public async Task<int> GetItems(string parameter = "") {
			int i = 0;
			var items = await GlobalManager.Instance.DataStore.GetItemsAsync();
			if (parameter.Equals("red")) {
				foreach (var item in items) {
					if (item.FlagColor == Color.IndianRed) {
						i += 1;
					}
				}
			} else if (parameter.Equals("yellow")) {
				foreach (var item in items) {
					if (item.FlagColor == Color.DarkGoldenrod) {
						i += 1;
					}
				}

			}else if(parameter.Equals("expired")) {
				foreach (var item in items) {
					if (item.Expired) {
						i += 1;
					}
				}
			} else {
				foreach (var item in items) {
					i += 1;
				}
			}
			
			return i;
		}
	}
}