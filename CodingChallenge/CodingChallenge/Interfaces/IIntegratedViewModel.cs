namespace CodingChallenge.Interfaces
{
    public interface IIntegratedViewModel
    {
        ITransportController TransportController { get; }

        IRoamingImageController RoamingImageController { get; }

        ITransportViewModel TransportViewModel { get; }

        IRoamingImageViewModel RoamingImageViewModel { get; }
    }
}
