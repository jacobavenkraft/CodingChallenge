using CodingChallenge.Framework;
using CodingChallenge.Interfaces;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CodingChallenge.Transport
{
    public class TransportController : BaseModel, ITransportController
    {
        private TimeSpan lastTimeCode = TimeSpan.Zero;

        private AutoResetEvent timeCodeDeactivated = new AutoResetEvent(false);
        private Thread timeCodeRefreshThread;

        public event Action<ITransportController> RecordingStart;
        public event Action<ITransportController> RecordingStop;
        public event Action<ITransportController> PlayingStart;
        public event Action<ITransportController> PlayingStop;

        public TransportController()
        {

        }

        public TransportStatus Status { get => Get<TransportStatus>(); private set => Set(value); }

        public TimeSpan TimeCode { get => Get<TimeSpan>(); private set => Set(value); }

        public DateTime PlayBeginTimestamp { get => Get<DateTime>(); private set => Set(value); }

        public DateTime PlayEndTimestamp { get => Get<DateTime>(); private set => Set(value); }

        public DateTime RecordBeginTimestamp { get => Get<DateTime>(); private set => Set(value); }

        public DateTime RecordEndTimestamp { get => Get<DateTime>(); private set => Set(value); }

        public bool CanPlay { get => Get<bool>(); private set => Set(value); }

        public bool CanPlayCue { get => Get<bool>(); private set => Set(value); }

        public bool CanRecord { get => Get<bool>(); private set => Set(value); }

        public bool CanRecordCue { get => Get<bool>(); private set => Set(value); }

        public bool CanStop { get => Get<bool>(); private set => Set(value); }

        private TimeSpan RefreshInterval { get; set; } = TimeSpan.FromMilliseconds(100);

        private void TimeCodeRefreshThreadProc()
        {
            while (!timeCodeDeactivated.WaitOne(RefreshInterval))
            {
                RefreshTimeCode();
            }
        }

        protected override void RaisePropertyChanged([CallerMemberName] string property = "")
        {
            base.RaisePropertyChanged(property);

            switch (property)
            {
                case nameof(Status):
                    RefreshAllowableActions();
                    break;
                default:
                    break;
            }
        }

        protected virtual void RefreshAllowableActions()
        {
            var canPlay = false;
            var canStop = false;
            var canPlayCue = false;
            var canRecordCue = false;
            var canRecord = false;

            switch (Status)
            {
                case TransportStatus.PlayCued:
                    canPlay = true;
                    canStop = true;
                    break;
                case TransportStatus.RecordCued:
                    canRecord = true;
                    canStop = true;
                    break;
                case TransportStatus.Stopped:
                    canPlayCue = true;
                    canRecordCue = true;
                    break;
                case TransportStatus.Playing:
                    canStop = true;
                    break;
                case TransportStatus.Recording:
                    canStop = true;
                    break;
            }

            CanPlay = canPlay;
            CanStop = canStop;
            CanRecord = canRecord;
            CanPlayCue = canPlayCue;
            CanRecordCue = canRecordCue;
        }

        protected virtual void RefreshTimeCode(TransportStatus status)
        {
            switch (status)
            {
                case TransportStatus.Playing:
                    TimeCode = lastTimeCode + (DateTime.Now - PlayBeginTimestamp);
                    break;
                case TransportStatus.Recording:
                    TimeCode = lastTimeCode + (DateTime.Now - RecordBeginTimestamp);
                    break;
                default:
                    break;
            }
        }

        protected void RefreshTimeCode()
        {
            RefreshTimeCode(Status);
        }

        public void Play()
        {
            if (!CanPlay)
            {
                return;
            }

            StartTimeCodeThread();
            PlayBeginTimestamp = DateTime.Now;
            PlayEndTimestamp = DateTime.MaxValue;
            Status = TransportStatus.Playing;
            PlayingStart?.Invoke(this);
        }

        public void PlayCue()
        {
            if (!CanPlayCue)
            {
                return;
            }

            Status = TransportStatus.PlayCued;
        }

        public void Record()
        {
            if (!CanRecord)
            {
                return;
            }

            StartTimeCodeThread();
            RecordBeginTimestamp = DateTime.Now;
            RecordEndTimestamp = DateTime.MaxValue;
            Status = TransportStatus.Recording;
            RecordingStart?.Invoke(this);
        }

        public void RecordCue()
        {
            if (!CanRecordCue)
            {
                return;
            }

            Status = TransportStatus.RecordCued;
        }

        public void ResetTimeCode()
        {
            TimeCode = TimeSpan.Zero;
        }

        public void Stop()
        {
            if (!CanStop)
            {
                return;
            }

            TransportStatus previousStatus = Status;

            Status = TransportStatus.Stopped;
            StopTimeCodeThread();

            RefreshTimeCode(previousStatus);
            lastTimeCode = TimeCode;

            switch (previousStatus)
            {
                case TransportStatus.Playing:
                    PlayEndTimestamp = DateTime.Now;
                    PlayingStop?.Invoke(this);
                    break;
                case TransportStatus.Recording:
                    RecordEndTimestamp = DateTime.Now;
                    RecordingStop?.Invoke(this);
                    break;
                default:
                    break;
            }
        }

        public virtual void Initialize()
        {
            Status = TransportStatus.Stopped;
            RefreshAllowableActions();
        }

        public void StartTimeCodeThread()
        {
            if (timeCodeRefreshThread != null)
            {
                StopTimeCodeThread();
            }

            timeCodeRefreshThread = new Thread(TimeCodeRefreshThreadProc);
            timeCodeRefreshThread.Priority = ThreadPriority.Highest;
            timeCodeRefreshThread.IsBackground = true;
            timeCodeRefreshThread.Name = "TimeCode Refresh Thread";
            timeCodeRefreshThread.Start();
        }

        public void StopTimeCodeThread()
        {
            if (timeCodeRefreshThread == null)
            {
                return;
            }

            timeCodeDeactivated.Set();
            timeCodeRefreshThread.Join();
            timeCodeRefreshThread = null;
        }


    }
}
