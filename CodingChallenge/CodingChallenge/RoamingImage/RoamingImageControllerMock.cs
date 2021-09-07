using CodingChallenge.Interfaces;

namespace CodingChallenge.RoamingImage
{
    public class RoamingImageControllerMock : IRoamingImageController
    {
        public string ImageUri { get; set; }
        public int RoamingSpeed { get; set; } = 1;
        public int RoamingVerticalSteps { get; set; } = 0;
        public int RoamingHorizontalSteps { get; set; } = 0;
        public double PositionX { get; set; } = 0;
        public double PositionY { get; set; } = 0;
        public double ImageWidth { get; set; } = 100;
        public double ImageHeight { get; set; } = 100;
        public double CanvasWidth { get; set; } = 200;
        public double CanvasHeight { get; set; } = 200;

        public void StartRoaming()
        {            
        }

        public void StopRoaming()
        {
        }
    }
}
