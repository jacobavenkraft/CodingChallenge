using CodingChallenge.Transport;
using System;

namespace CodingChallenge.Interfaces
{
    public interface ITransportController
    {
        TransportStatus Status { get; }

        TimeSpan TimeCode { get; }

        bool CanRecord { get; }

        bool CanPlay { get; }

        bool CanStop { get; }

        bool CanRecordCue { get; }

        bool CanPlayCue { get; }

        void Stop();
        
        void RecordCue();
        
        void Record();

        void PlayCue();
        
        void Play();

        void ResetTimeCode();

        void Initialize();

        event Action<ITransportController> RecordingStart;

        event Action<ITransportController> RecordingStop;

        event Action<ITransportController> PlayingStart;

        event Action<ITransportController> PlayingStop;
    }
}
