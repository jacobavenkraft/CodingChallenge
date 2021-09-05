using CodingChallenge.Framework;
using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CodingChallenge.RoamingImage
{
    public class RoamingImageViewModel : BaseModel
    {
        public RoamingImageViewModel(IRoamingImageController controller)
        {
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
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

        public string ImageUri { get => Get<string>(); set => Set(value); }

        public IRoamingImageController Controller { get => Get<IRoamingImageController>(); set => Set(value); }

        public Point MousePosition { get => Get<Point>(); set => Set(value); }

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
    }
}
