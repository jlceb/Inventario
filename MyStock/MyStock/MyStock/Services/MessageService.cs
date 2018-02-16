
using System.Threading.Tasks;

namespace MyStock.Services
{
    public class MessageService
    {
        public async Task SendMessage(string title, string message)
        {
            await MyStock.App.Current.MainPage.DisplayAlert(title, message, "Accept");
        }
    }
}
