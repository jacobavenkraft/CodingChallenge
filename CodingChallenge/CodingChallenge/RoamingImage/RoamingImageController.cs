using CodingChallenge.Framework;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CodingChallenge.RoamingImage
{
    public class RoamingImageController : BaseModel, IRoamingImageController
    {
        private Thread updateThread;
        private AutoResetEvent roamingDeactivatedEvent = new AutoResetEvent(false);

        private double minimumPositionX = 0;
        private double maximumPositionX = 0;
        private double minimumPositionY = 0;
        private double maximumPositionY = 0;

        public RoamingImageController()
        {

        }

        protected override void RaisePropertyChanged([CallerMemberName] string property = "")
        {
            base.RaisePropertyChanged(property);

            switch(property)
            {
                case nameof(ImageHeight):
                case nameof(ImageWidth):
                    ReCalculateRoamingBoundaries();
                    break;
                default:
                    break;
            }
        }

        private TimeSpan RefreshInterval { get; set; } = TimeSpan.FromMilliseconds(100);

        public int RoamingSpeed { get => Get<int>(); set => Set(value); }

        public int RoamingVerticalSteps { get => Get<int>(); set => Set(value); }

        public int RoamingHorizontalSteps { get => Get<int>(); set => Set(value); }

        public double PositionX { get => Get<double>(); set => Set(value); }

        public double PositionY { get => Get<double>(); set => Set(value); }

        public double ImageWidth { get => Get<double>(); set => Set(value); }

        public double ImageHeight { get => Get<double>(); set => Set(value); }

        public double CanvasWidth { get => Get<double>(); set => Set(value); }

        public double CanvasHeight { get => Get<double>(); set => Set(value); }

        public void StartRoaming()
        {
            if (updateThread != null)
            {
                StopRoaming();
            }

            updateThread = new Thread(RoamingUpdateThreadProc);
            updateThread.IsBackground = true;
            updateThread.Name = "Image Roaming Thread";
            updateThread.Start();
        }

        public void StopRoaming()
        {
            if (updateThread == null)
            {
                return;
            }

            roamingDeactivatedEvent.Set();
            updateThread.Join();
            updateThread = null;
        }

        private void ReCalculateRoamingBoundaries()
        {
            minimumPositionX = 0.0 - (ImageWidth / 2.0);
            minimumPositionY = 0.0 - (ImageHeight / 2.0);
            maximumPositionX = minimumPositionX + ImageWidth;
            maximumPositionY = maximumPositionY + ImageHeight;
        }

        private void RoamingUpdateThreadProc()
        {
            while (!roamingDeactivatedEvent.WaitOne(RefreshInterval))
            {
                double newPositionX = PositionX + (RoamingHorizontalSteps * RoamingSpeed);
                double newPositionY = PositionY + (RoamingVerticalSteps * RoamingSpeed);

                if (newPositionX >= minimumPositionX && newPositionX <= maximumPositionX)
                {
                    PositionX = newPositionX;
                }

                if (newPositionY >= minimumPositionY && newPositionY <= maximumPositionY)
                {
                    PositionY = newPositionY;
                }
            }
        }
    }
}
