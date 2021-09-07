using System.Windows.Input;

namespace CodingChallenge.Interfaces
{
    public interface ITransportViewModel
    {
        ITransportController Controller { get; set; }

        int CueCountdown { get; set; }

        bool CueCountdownVisible { get; set; }

        ICommand RecordCommand { get; set; }

        ICommand PlayCommand { get; set; }

        ICommand StopCommand { get; set; }
    }
}
