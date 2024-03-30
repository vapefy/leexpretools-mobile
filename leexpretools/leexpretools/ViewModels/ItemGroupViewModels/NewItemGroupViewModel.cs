using leexpretools.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace leexpretools.ViewModels.ItemGroupViewModels {

    public class NewItemGroupViewModel : BaseViewModel
    {
        private string _name;
        private string _offset;
        private string _redFlag;
        private string _yellowFlag;


		public NewItemGroupViewModel() {
            Title = "Neue Artikelgruppe";
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
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

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var newItemGroup = new ItemGroup() {
                Id = 0,
                Name = Name,
                Offset = Int32.Parse(Offset),
                YellowFlag = Int32.Parse(YellowFlag),
                RedFlag = Int32.Parse(RedFlag)
            };

            await DataStore.AddItemGroupAsync(newItemGroup);

            await Shell.Current.GoToAsync("..");
        }
    }
}
