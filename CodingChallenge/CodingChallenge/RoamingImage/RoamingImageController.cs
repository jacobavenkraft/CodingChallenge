using CodingChallenge.Configuration;
using CodingChallenge.Framework;
using CodingChallenge.Interfaces;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CodingChallenge.RoamingImage
{
    public class RoamingImageController : BaseModel, IRoamingImageController, ISizeChangeListener, IMouseMoveListener
    {
        private Thread updateThread;
        private AutoResetEvent roamingDeactivatedEvent = new AutoResetEvent(false);

        private double minimumPositionX = 0;
        private double maximumPositionX = 0;
        private double minimumPositionY = 0;
        private double maximumPositionY = 0;

        public RoamingImageController()
        {
            RoamingSpeed = Settings.Instance.RoamingSpeed;
            ImageUri = Settings.Instance.ImageUri;
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
                case nameof(ImageUri):
                    Settings.Instance.ImageUri = ImageUri;
                    break;
                case nameof(RoamingSpeed):
                    Settings.Instance.RoamingSpeed = RoamingSpeed;
                    break;
                default:
                    break;
            }
        }

        private TimeSpan RefreshInterval { get; set; } = TimeSpan.FromMilliseconds(100);

        public string ImageUri { get => Get<string>(); set => Set(value); }

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
            maximumPositionY = minimumPositionY + ImageHeight;
        }

        private void RoamingUpdateThreadProc()
        {
            while (!roamingDeactivatedEvent.WaitOne(RefreshInterval))
            {
                double newPositionX = PositionX + (RoamingHorizontalSteps * RoamingSpeed);
                double newPositionY = PositionY + (RoamingVerticalSteps * RoamingSpeed);

                newPositionX = Math.Min(maximumPositionX, Math.Max(minimumPositionX, newPositionX));
                newPositionY = Math.Min(maximumPositionY, Math.Max(minimumPositionY, newPositionY));

                PositionX = newPositionX;
                PositionY = newPositionY;
            }
        }

        void ISizeChangeListener.SizeChanged(Size newSize)
        {
            ImageWidth = newSize.Width;
            ImageHeight = newSize.Height;
            CanvasWidth = newSize.Width * 2.0;
            CanvasHeight = newSize.Height * 2.0;
        }

        private void RefreshRoamingSteps(Point newPosition)
        {
            if (newPosition.X < ImageWidth)
            {
                RoamingHorizontalSteps = -1;
            }
            else if (newPosition.X > ImageWidth)
            {
                RoamingHorizontalSteps = 1;
            }
            else
            {
                RoamingHorizontalSteps = 0;
            }

            if (newPosition.Y < ImageHeight)
            {
                RoamingVerticalSteps = -1;
            }
            else if (newPosition.Y > ImageHeight)
            {
                RoamingVerticalSteps = 1;
            }
            else
            {
                RoamingVerticalSteps = 0;
            }
        }

        void IMouseMoveListener.MouseMove(Point newPosition)
        {
            RefreshRoamingSteps(newPosition);
        }
    }
}
