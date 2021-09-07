using CodingChallenge.Framework;
using CodingChallenge.Interfaces;
using System;

namespace CodingChallenge.ViewModel
{
    public class IntegratedViewModel : BaseModel
    {
        public IntegratedViewModel(ITransportViewModel transportViewModel, IRoamingImageViewModel roamingImageViewModel)
        {
            TransportViewModel = transportViewModel ?? throw new ArgumentNullException(nameof(transportViewModel));
            RoamingImageViewModel = roamingImageViewModel ?? throw new ArgumentNullException(nameof(roamingImageViewModel));
            if (TransportController != null)
            {
                TransportController.RecordingStart += TransportController_RecordingStart;
                TransportController.RecordingStop += TransportController_RecordingStop;
            }
        }

        private void TransportController_RecordingStop(ITransportController obj)
        {
            RoamingImageController.StopRoaming();
        }

        private void TransportController_RecordingStart(ITransportController obj)
        {
            RoamingImageController.StartRoaming();
        }

        public ITransportController TransportController => TransportViewModel?.Controller;

        public IRoamingImageController RoamingImageController => RoamingImageViewModel?.Controller;

        public ITransportViewModel TransportViewModel { get => Get<ITransportViewModel>(); set => Set(value); }

        public IRoamingImageViewModel RoamingImageViewModel { get => Get<IRoamingImageViewModel>(); set => Set(value); }
    }
}
