using System;

namespace CodingChallenge.Transport
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
    }
}
