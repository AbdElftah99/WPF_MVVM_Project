using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MVVM_Project.View
{
    /// <summary>
    /// Interaction logic for First.xaml
    /// </summary>
    public partial class First : Window
    {
        public First()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                this.MouseDown += delegate
                {
                    DragMove();
                };
            }
            catch 
            {
            }
            
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        DragMove();
        //    }
        //}
    }
}
