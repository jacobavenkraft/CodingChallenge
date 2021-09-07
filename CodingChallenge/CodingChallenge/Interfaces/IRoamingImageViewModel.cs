using System.Windows.Input;

namespace CodingChallenge.Interfaces
{
    public interface IRoamingImageViewModel
    {
        string Title { get; set; }

        IRoamingImageController Controller { get; set; }

        ICommand StartRoamingCommand { get; set; }

        ICommand StopRoamingCommand { get; set; }
    }
}
