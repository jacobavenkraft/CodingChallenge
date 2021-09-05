using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge.RoamingImage
{
    public interface IRoamingImageController
    {
        int RoamingSpeed { get; set; }

        int RoamingVerticalSteps { get; set; }

        int RoamingHorizontalSteps { get; set; }

        double PositionX { get; set; }

        double PositionY { get; set; }

        double ImageWidth { get; set; }

        double ImageHeight { get; set; }

        double CanvasWidth { get; set; }

        double CanvasHeight { get; set; }

        void StartRoaming();

        void StopRoaming();
    }
}
