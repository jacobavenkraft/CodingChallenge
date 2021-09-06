namespace CodingChallenge.Interfaces
{
    public interface IRoamingImageController
    {
        string ImageUri { get; set; }

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
