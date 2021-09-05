using CodingChallenge.Framework;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Timers;
using System.Windows.Input;

namespace CodingChallenge.Transport
{
    public class TransportViewModel : BaseModel
    {
        private Timer cueTimer = new Timer(1000);

        public TransportViewModel(ITransportController controller)
        {
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            controller.Initialize();
            BuildCommands();
            cueTimer.Elapsed += CueTimer_Elapsed;
        }

        private void CueTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CueCountdown -= 1;

            if (CueCountdown == 0)
            {
                ReleaseCue();
            }
        }

        private void BuildCommands()
        {
            RecordCommand = new RelayCommand(DoRecord, CanDoRecord);
            PlayCommand = new RelayCommand(DoPlay, CanDoPlay);
            StopCommand = new RelayCommand(DoStop, CanDoStop);
        }

        public ITransportController Controller { get => Get<ITransportController>(); set => Set(value); }

        public int CueCountdown { get => Get<int>(); set => Set(value); }

        public bool CueCountdownVisible { get => Get<bool>(); set => Set(value); }

        public ICommand RecordCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand PlayCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand StopCommand { get => Get<ICommand>(); set => Set(value); }

        public void DoRecord()
        {
            if (!CanDoRecord())
            {
                return;
            }

            Controller.RecordCue();
            StartCueCountdown();
        }

        public bool CanDoRecord() => Controller.CanRecordCue;

        public void DoPlay()
        {
            if (!CanDoPlay())
            {
                return;
            }

            Controller.PlayCue();
            StartCueCountdown();
        }

        public bool CanDoPlay() => Controller.CanPlayCue;

        public void DoStop()
        {
            if (!CanDoStop())
            {
                return;
            }

            Controller.Stop();
            StopCueCountdown();
        }

        public bool CanDoStop() => Controller.CanStop;

        private void StartCueCountdown()
        {
            CueCountdown = 3;
            CueCountdownVisible = true;
            cueTimer.Start();
        }

        private void StopCueCountdown()
        {
            CueCountdown = 0;
            CueCountdownVisible = false;
            cueTimer.Stop();
        }

        private void ReleaseCue()
        {
            StopCueCountdown();
            switch (Controller.Status)
            {
                case TransportStatus.PlayCued:
                    Controller.Play();
                    break;
                case TransportStatus.RecordCued:
                    Controller.Record();
                    break;
                default:
                    break;
            }
        }
    }
}
