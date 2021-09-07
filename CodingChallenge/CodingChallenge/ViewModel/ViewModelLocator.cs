/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:CodingChallenge"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CodingChallenge.Interfaces;
using CodingChallenge.RoamingImage;
using CodingChallenge.Transport;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace CodingChallenge.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                //SimpleIoc.Default.Register<IDataService, DesignDataService>();
                SimpleIoc.Default.Register<ITransportController, TransportController>();
                SimpleIoc.Default.Register<IRoamingImageController, RoamingImageControllerMock>();
            }
            else
            {
                // Create run time view services and models
                //SimpleIoc.Default.Register<IDataService, DataService>();
                SimpleIoc.Default.Register<ITransportController, TransportController>();
                SimpleIoc.Default.Register<IRoamingImageController, RoamingImageController>();
            }
            
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ITransportViewModel, TransportViewModel>();
            SimpleIoc.Default.Register<IRoamingImageViewModel, RoamingImageViewModel>();
            SimpleIoc.Default.Register<IntegratedViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public ITransportViewModel Transport
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ITransportViewModel>();
            }
        }

        public IRoamingImageViewModel RoamingImage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IRoamingImageViewModel>();
            }
        }

        public IntegratedViewModel Integrated
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IntegratedViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}