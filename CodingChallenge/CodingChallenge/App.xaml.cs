﻿using CodingChallenge.Configuration;
using System.Windows;

namespace CodingChallenge
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ThemeManager.Initialize();
        }
    }
}
