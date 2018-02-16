
namespace MyStock.Infrastructure
{
    using ViewModels;

    public class InstanceLocator
    {
        public MainViewModel MvM { get; set; }

        public InstanceLocator()
        {
            MvM = new MainViewModel();
        }
    }
}
