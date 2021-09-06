using CodingChallenge.Framework;
using CodingChallenge.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CodingChallenge.RoamingImage
{
    public class RoamingImageViewModel : BaseModel, IMouseMoveListener
    {
        public RoamingImageViewModel(IRoamingImageController controller)
        {
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            
            if (Controller is INotifyPropertyChanged notifyingController)
            {
                notifyingController.PropertyChanged += NotifyingController_PropertyChanged;
            }

            StartRoamingCommand = new RelayCommand(DoStartRoaming);
            StopRoamingCommand = new RelayCommand(DoStopRoaming);

            RefreshTitle();
        }

        private void NotifyingController_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IRoamingImageController.ImageUri):
                    RefreshTitle();
                    break;
                default:
                    break;
            }
        }

        protected override void RaisePropertyChanged([CallerMemberName] string property = "")
        {
            base.RaisePropertyChanged(property);

            switch(property)
            {
                case nameof(MousePosition):
                    RefreshRoamingSteps();
                    break;
                default:
                    break;
            }
        }

        public string Title { get => Get<string>(); set => Set(value); }

        public IRoamingImageController Controller { get => Get<IRoamingImageController>(); set => Set(value); }

        public Point MousePosition { get => Get<Point>(); set => Set(value); }

        public ICommand StartRoamingCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand StopRoamingCommand { get => Get<ICommand>(); set => Set(value); }

        private void RefreshRoamingSteps()
        {
            if (MousePosition.X < Controller.ImageWidth)
            {
                Controller.RoamingHorizontalSteps = -1;
            }
            else if (MousePosition.X > Controller.ImageWidth)
            {
                Controller.RoamingHorizontalSteps = 1;
            }
            else
            {
                Controller.RoamingHorizontalSteps = 0;
            }

            if (MousePosition.Y < Controller.ImageHeight)
            {
                Controller.RoamingVerticalSteps = -1;
            }
            else if (MousePosition.Y > Controller.ImageHeight)
            {
                Controller.RoamingVerticalSteps = 1;
            }
            else
            {
                Controller.RoamingVerticalSteps = 0;
            }
        }

        private void RefreshTitle()
        {
            var suffix = string.Empty;

            if (File.Exists(Controller.ImageUri))
            {
                var filename = Path.GetFileName(Controller.ImageUri);
                suffix = $" ({filename})";
            }
            else
            {
                suffix = $" (INVALID IMAGE)";
            }

            Title = $"Roaming Image{suffix}";
        }

        private void DoStartRoaming()
        {
            Controller.StartRoaming();
        }

        private void DoStopRoaming()
        {
            Controller.StopRoaming();
        }

        void IMouseMoveListener.MouseMove(Point newPosition)
        {
            MousePosition = newPosition;
        }
    }
}
