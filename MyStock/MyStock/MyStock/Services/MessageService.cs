
using System.Threading.Tasks;

namespace MyStock.Services
{
    public class MessageService
    {
        public async Task SendMessage(string title, string message)
        {
            await MyStock.App.Current.MainPage.DisplayAlert(title, message, "Accept");
        }

        public async Task<bool> ConfirmMessage(string title, string message)
        {
            return await MyStock.App.Current.MainPage.DisplayAlert(title, message, "Yes","No");
        }

        public async Task<string> ShowImageOptions()
        {
            return await MyStock.App.Current.MainPage.DisplayActionSheet("Where do you take the image?", "Cancel",null, "From Gallery", "From Camera");
        }
    }
}
