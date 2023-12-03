using MVVM_Project.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var first = new First();
            first.Show();
            first.IsVisibleChanged += (s, ev) =>
            {
                if (first.IsVisible == true && first.IsLoaded)
                {
                    var mainview = new MainView();
                    mainview.Show();
            first.Close();
                  }
            };
        }

        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var first = new First();
            first.Show();
            {
                if (first.IsVisible == true && first.IsLoaded)
                {
                    var mainview = new MainView();
                    mainview.Show();
                    first.Close();
                }
            };
        }
    }
}
