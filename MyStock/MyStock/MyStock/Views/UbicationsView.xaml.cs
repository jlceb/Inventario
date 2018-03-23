using MyStock.Services;
using MyStock.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MyStock.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UbicationsView : ContentPage
	{
        GeolocatorService geolocatorService;

		public UbicationsView ()
		{
			InitializeComponent ();
            geolocatorService = new GeolocatorService();
            MoveMapToCurrentPosition();
		}

        async void MoveMapToCurrentPosition()
        {
            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude != 0 || geolocatorService.Longitude != 0)
            {
                var position = new Position(geolocatorService.Latitude,geolocatorService.Longitude);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position,Distance.FromKilometers(.5)));
            }

            await DrawPinsOnMyMap();
        }

        async Task DrawPinsOnMyMap()
        {
            var ubicationsViewModel = UbicationsViewModel.GetIntance();
            await ubicationsViewModel.LoadPins();
            foreach (var itemPin in ubicationsViewModel.Pins)
            {
                MyMap.Pins.Add(itemPin);
            }
        }
	}
}