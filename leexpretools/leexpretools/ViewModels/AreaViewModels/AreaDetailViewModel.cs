using leexpretools.Models;
using leexpretools.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace leexpretools.ViewModels.AreaViewModels {
	[QueryProperty(nameof(AreaId), nameof(AreaId))]
	public class AreaDetailViewModel : BaseViewModel {
		private int _areaId;
		private string _name;
		private string _description;
		public int Id { get; set; }

		public Command SaveAreaCommand { get; }
		public Command DeleteAreaCommand { get; }
		public AreaDetailViewModel() {
			Title = "Ladenbereich bearbeiten";
			DeleteAreaCommand = new Command(DeleteAreaButton_OnClicked);
			SaveAreaCommand = new Command(SaveAreaButton_OnClicked);

		}

		public string Name {
			get => _name;
			set => SetProperty(ref _name, value);
		}

		public string Description {
			get => _description;
			set => SetProperty(ref _description, value);
		}

		public int AreaId {
			get {
				return _areaId;
			}
			set {
				_areaId = value;
				LoadAreaId(value);
			}
		}
		


		private async void SaveAreaButton_OnClicked() {
			Area updatedArea = new Area() {
				Id = Id,
				Name = Name,
				Description = Description
			};

			await DataStore.UpdateAreaAsync(updatedArea);
			
			await Shell.Current.GoToAsync("..");
		}

		private async void DeleteAreaButton_OnClicked(object obj) {
			await DataStore.DeleteAreaAsync(Id);
			await Shell.Current.GoToAsync("..");
		}

		public async void LoadAreaId(int areaId) {
				var area = await DataStore.GetAreaAsync(areaId);
				Id = area.Id;
				Name = area.Name;
				Description = area.Description;

		}
	}
}
