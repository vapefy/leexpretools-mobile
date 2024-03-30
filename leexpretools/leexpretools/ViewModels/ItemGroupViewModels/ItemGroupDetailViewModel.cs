using leexpretools.Models;
using leexpretools.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace leexpretools.ViewModels.ItemGroupViewModels {
	[QueryProperty(nameof(ItemGroupId), nameof(ItemGroupId))]
	public class ItemGroupDetailViewModel : BaseViewModel {
		private int _itemGroupId;
		private string _name;
		private string _offset;
		private string _redFlag;
		private string _yellowFlag;

		public int Id { get; set; }

		public Command SaveCommand { get; }
		public Command DeleteCommand { get; }
		public ItemGroupDetailViewModel() {
			Title = "Artikelgruppe bearbeiten";
			DeleteCommand = new Command(DeleteItemGroupButton_OnClicked);
			SaveCommand = new Command(SaveItemGroupButton_OnClicked);

		}
		

		public string Name {
			get => _name;
			set => SetProperty(ref _name, value);
		}

		public string Offset {
			get => _offset;
			set => SetProperty(ref _offset, value);
		}
		
		public string RedFlag {
			get => _redFlag;
			set => SetProperty(ref _redFlag, value);
		}
		
		public string YellowFlag {
			get => _yellowFlag;
			set => SetProperty(ref _yellowFlag, value);
		}

		public int ItemGroupId {
			get {
				return _itemGroupId;
			}
			set {
				_itemGroupId = value;
				LoadItemGroup(value);
			}
		}
		


		private async void SaveItemGroupButton_OnClicked() {
			var updatedItemGroup = new ItemGroup() {
				Id = Id,
				Name = Name,
				Offset = Int32.Parse(Offset),
				YellowFlag = Int32.Parse(YellowFlag),
				RedFlag = Int32.Parse(RedFlag)
			};

			await DataStore.UpdateItemGroupAsync(updatedItemGroup);
			
			await Shell.Current.GoToAsync("..");
		}

		private async void DeleteItemGroupButton_OnClicked(object obj) {
			await DataStore.DeleteItemGroupAsync(Id);
			await Shell.Current.GoToAsync("..");
		}

		public async void LoadItemGroup(int itemGroupId) {
			try {
				var group = await DataStore.GetItemGroupAsync(itemGroupId);
				Id = group.Id;
				Name = group.Name;
				Offset = group.Offset.ToString();
				YellowFlag = group.YellowFlag.ToString();
				RedFlag = group.RedFlag.ToString();
			} catch(Exception) {
				Debug.WriteLine("Failed to Load Item");
			}
		}
	}
}
