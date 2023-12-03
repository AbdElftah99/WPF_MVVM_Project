using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVM_Project.CustomCommands
{
    /// <summary>
    /// Interaction logic for BindableOasswordBox.xaml
    /// </summary>
    public partial class BindableOasswordBox : UserControl
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("PassWord", typeof(SecureString) , typeof(BindableOasswordBox));

        public SecureString PassWord
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public BindableOasswordBox()
        {
            InitializeComponent();
            txtPassWord.PasswordChanged += OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PassWord = txtPassWord.SecurePassword;
        }
    }
}
