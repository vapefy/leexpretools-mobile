using leexpretools.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace leexpretools.ViewModels.AreaViewModels
{
    public class NewAreaViewModel : BaseViewModel
    {
        private string name;
        private string description;


		public NewAreaViewModel()
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }
        

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Area newArea = new Area()
            {
                Id = 0,
                Name = Name,
                Description = Description,
                MarketId = GlobalManager.Instance.Market.Id
            };

            await DataStore.AddAreaAsync(newArea);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
