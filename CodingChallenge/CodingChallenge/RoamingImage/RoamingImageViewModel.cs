using CodingChallenge.Framework;
using CodingChallenge.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace CodingChallenge.RoamingImage
{
    public class RoamingImageViewModel : BaseModel, IRoamingImageViewModel
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

        public string Title { get => Get<string>(); set => Set(value); }

        public IRoamingImageController Controller { get => Get<IRoamingImageController>(); set => Set(value); }

        public ICommand StartRoamingCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand StopRoamingCommand { get => Get<ICommand>(); set => Set(value); }

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
    }
}
