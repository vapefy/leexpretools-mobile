using leexpretools.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace leexpretools.ViewModels.ItemViewModels {

    public class NewItemViewModel : BaseViewModel {
        private string _name;
        private DateTime _expires;
        private string _ean;
        private string _description;
        private Area _selectedArea;
        private ItemGroup _selectedItemGroup;
        private string _price;
        private string _zone;
        
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command<Area> TappedArea { get; }
        public Command<ItemGroup> TappedItemGroup { get; }
        public Command<string> TappedZone { get; }

        public ObservableCollection<Area> Areas;
        public ObservableCollection<ItemGroup> ItemGroups;



		public NewItemViewModel() {
            Title = "Artikel hinzufügen";
            Areas = new ObservableCollection<Area>();
            LoadAreas();
            ItemGroups = new ObservableCollection<ItemGroup>();
            LoadItemGroups();
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            
            TappedArea = new Command<Area>(OnAreaSelected);
            TappedItemGroup = new Command<ItemGroup>(OnItemGroupSelected);
            TappedZone = new Command<string>(OnZoneSelected);

            Expires = DateTime.Now;


            
            
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

        private async void OnCancel()
        {
            //await Shell.Current.GoToAsync("..");


        }

        private async void OnSave() {
            Console.WriteLine(SelectedArea.Name);
            Console.WriteLine(Ean);
            Console.WriteLine(Expires.ToString());
            Console.WriteLine(Name);
            Console.WriteLine(Description);
            Console.WriteLine(SelectedItemGroup.Name);
            Console.WriteLine(Price);
            Console.WriteLine(SelectedZone);
            bool expired;
            if (Expires <= DateTime.Now) {
                expired = false;
            } else {
                expired = true;
            }
            
            var newItem = new Item() {
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

            await DataStore.AddItemAsync(newItem);

            await Shell.Current.GoToAsync("..");
        }
    }
}
