using MyStock.Models;
using MyStock.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace MyStock.ViewModels
{
    public class UbicationsViewModel
    {
        public ObservableCollection<Pin> Pins {get; set;}

        ApiService apiService;
        MessageService messageService;

        public UbicationsViewModel()
        {
            instance = this;
            apiService = new ApiService();
            messageService = new MessageService();
        }

        public async Task LoadPins()
        {
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await messageService.SendMessage("Error",connection.Message);
                return;
            }

            var mainViewModel = MainViewModel.GetIntance();

            var response = await apiService.GetList<Ubication>("http://productszuluapi.azurewebsites.net",
                "/api","/Ubications",mainViewModel.tokenResponse.TokenType, mainViewModel.tokenResponse.AccessToken);

            if (!response.IsSuccess)
            {
                await messageService.SendMessage("Error",response.Message);
                return;
            }

            var ubicationsList = (List<Ubication>)response.Result;
            Pins = new ObservableCollection<Pin>();
            foreach (var ubi in ubicationsList)
            {
                Pins.Add(new Pin
                {
                    Address = ubi.Address,
                    Label = ubi.Description,
                    Position = new Position(ubi.Latitude, ubi.Longitude),
                    Type = PinType.Place,
                });
            }
        }

        //singlenton

        static UbicationsViewModel instance;

        public static UbicationsViewModel GetIntance()
        {
            if (instance == null)
                return new UbicationsViewModel();
            else
                return instance;
        }
    }
}
