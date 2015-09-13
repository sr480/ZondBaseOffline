using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace ZondBase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Dispatcher GlobalDispatcher;
        protected override void OnStartup(StartupEventArgs e)
        {
            GlobalDispatcher = this.Dispatcher;
            base.OnStartup(e);
            var win = new MainWindow();
            win.DataContext = new MainViewModel();
            win.Show();
        }
    }
}
