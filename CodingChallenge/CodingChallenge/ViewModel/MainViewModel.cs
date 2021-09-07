using CodingChallenge.Configuration;
using CodingChallenge.Framework;
using CodingChallenge.Interfaces;
using CodingChallenge.RoamingImage;
using CodingChallenge.Views;
using CommonServiceLocator;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CodingChallenge.ViewModel
{
    public enum ViewType
    {
        SeparateWindow,
        Integrated
    }

    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : BaseModel
    {
        private Window roamingImageWindow;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            GetImagePathCommand = new RelayCommand(DoGetImagePath);
            EnableDarkThemeCommand = new RelayCommand(DoEnableDarkTheme);
            EnableLightThemeCommand = new RelayCommand(DoEnableLightTheme);
            EnableAutoThemeCommand = new RelayCommand(DoEnableAutoTheme);
            ToggleViewTypeCommand = new RelayCommand(DoToggleViewType);
            RoamingImageController = ServiceLocator.Current.GetInstance<IRoamingImageController>();
            TransportController = ServiceLocator.Current.GetInstance<ITransportController>();
            TransportController.PlayingStart += TransportController_PlayingStart;
            TransportController.RecordingStart += TransportController_RecordingStart;
            TransportController.RecordingStop += TransportController_RecordingStop;
            RefreshTitle();
            RefreshThemeIndicators();
        }

        protected override void RaisePropertyChanged([CallerMemberName] string property = "")
        {
            base.RaisePropertyChanged(property);

            switch (property)
            {
                case nameof(ViewType):
                    base.RaisePropertyChanged(nameof(UseIntegratedView));
                    base.RaisePropertyChanged(nameof(UseSeparateWindowView));
                    break;
                default:
                    break;
            }
        }

        private void HideRoamingImageWindow()
        {
            roamingImageWindow?.Close();
            roamingImageWindow = null;
        }

        private void ShowRoamingImageWindow()
        {
            if (UseIntegratedView)
            {
                return;
            }

            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(ShowRoamingImageWindow));
                return;
            }

            if (roamingImageWindow != null)
            {
                roamingImageWindow.Activate();
                return;
            }

            roamingImageWindow = new RoamingImageWindow();
            roamingImageWindow.Show();
            roamingImageWindow.Activate();
            roamingImageWindow.Focus();
        }

        private void TransportController_RecordingStart(ITransportController transportController)
        {
            ShowRoamingImageWindow();
        }

        private void TransportController_PlayingStart(ITransportController transportController)
        {
            ShowRoamingImageWindow();
        }

        private void TransportController_RecordingStop(ITransportController transportController)
        {
            HideRoamingImageWindow();
        }

        public IRoamingImageController RoamingImageController { get; private set; }

        public ICommand GetImagePathCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand ToggleViewTypeCommand { get => Get<ICommand>(); set => Set(value); }

        public string ImageUri => RoamingImageController?.ImageUri ?? string.Empty;

        public string Title { get => Get<string>(); set => Set(value); }

        public ITransportController TransportController { get; private set; }

        public bool IsDarkThemeEnabled { get => Get<bool>(); set => Set(value); }

        public bool IsLightThemeEnabled { get => Get<bool>(); set => Set(value); }

        public bool IsAutoThemeEnabled { get => Get<bool>(); set => Set(value); }

        public ICommand EnableDarkThemeCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand EnableLightThemeCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand EnableAutoThemeCommand { get => Get<ICommand>(); set => Set(value); }

        public ViewType ViewType { get => Get<ViewType>(); set => Set(value); }

        public bool UseIntegratedView { get => ViewType == ViewType.Integrated; set { } }

        public bool UseSeparateWindowView { get => ViewType == ViewType.SeparateWindow; set { } }

        private void DoEnableDarkTheme() => DoSwitchThemes(ThemeMode.Dark);

        private void DoEnableLightTheme() => DoSwitchThemes(ThemeMode.Light);

        private void DoEnableAutoTheme() => DoSwitchThemes(ThemeMode.Auto);

        private void DoSwitchThemes(ThemeMode newTheme)
        {
            ThemeManager.ConfiguredMode = newTheme;
            RefreshThemeIndicators();
        }

        private void DoToggleViewType()
        {
            switch(ViewType)
            {
                case ViewType.Integrated:
                    ViewType = ViewType.SeparateWindow;
                    break;
                case ViewType.SeparateWindow:
                    ViewType = ViewType.Integrated;
                    break;
                default:
                    break;
            }
        }

        private void RefreshThemeIndicators()
        {
            IsDarkThemeEnabled = ThemeManager.ConfiguredMode == ThemeMode.Dark;
            IsLightThemeEnabled = ThemeManager.ConfiguredMode == ThemeMode.Light;
            IsAutoThemeEnabled = ThemeManager.ConfiguredMode == ThemeMode.Auto;
        }

        private void DoGetImagePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = ImageUri;
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                RoamingImageController.ImageUri = openFileDialog.FileName;
            }

            RefreshTitle();
        }

        private void RefreshTitle()
        {
            var suffix = string.Empty;

            if (File.Exists(ImageUri))
            {
                var fileName = Path.GetFileName(ImageUri);

                suffix = $" ({fileName})";
            }
            else
            {
                suffix = $" (INVALID IMAGE)";
            }

            Title = $"Coding Challenge 1{suffix}";
        }
    }
}