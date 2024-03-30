using leexpretools.Models;
using leexpretools.Views;
using leexpretools.Views.AreaPages;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace leexpretools.ViewModels.AreaViewModels  {
	public class AreasViewModel : BaseViewModel {
		private Area _selectedArea;

		public ObservableCollection<Area> Areas { get; }
		public Command LoadAreasCommand { get; }
		public Command AddAreaCommand { get; }
		public Command<Area> AreaTapped { get; }


		public AreasViewModel() {
			Title = "Ladenbereiche";
			Areas = new ObservableCollection<Area>();
			LoadAreasCommand = new Command(async () => await ExecuteLoadAreasCommand());

			AreaTapped = new Command<Area>(OnAreaSelected);

			AddAreaCommand = new Command(OnAddArea);
		}
		
		

		async Task ExecuteLoadAreasCommand() {
			IsBusy = true;

			try {
				Areas.Clear();
				var areas = await DataStore.GetAreasAsync();
				foreach(var area in areas) {
					Console.WriteLine(area.Name);
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

		public void OnAppearing() {
			IsBusy = true;
			SelectedArea = null;
		}

		public Area SelectedArea {
			get => _selectedArea;
			set {
				SetProperty(ref _selectedArea, value);
				OnAreaSelected(value);
			}
		}

		private async void OnAddArea(object obj) {
			await Shell.Current.GoToAsync(nameof(NewAreaPage));
		}

		async void OnAreaSelected(Area area) {
			if(area == null)
				return;
			Console.WriteLine(area.Id);

			// This will push the AreaDetailPage onto the navigation stack
			await Shell.Current.GoToAsync($"{nameof(AreaDetailPage)}?{nameof(AreaDetailViewModel.AreaId)}={area.Id}");
		}
	}
}