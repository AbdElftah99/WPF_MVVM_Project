using MVVM_Project.Command;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using MVVM_Project.View;
using MVVM_Project.Model;
using MVVM_Project.Repositories;
using System.Threading;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace MVVM_Project.ViewModel
{
    public class FirstViewModel : FirstViewModelBase
    {
        #region DataBase
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader dr;
        #endregion

        #region Fields
        private string _username;
        private SecureString _password;
        private string _errormsg;
        private bool _isviewvisible = true;

        private IUserRepository _userrepository;
        #endregion

        #region Prop
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public SecureString PassWord
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(PassWord));
            }
        }

        public string Errormsg
        {
            get
            {
                return _errormsg;
            }
            set
            {
                _errormsg = value;
                OnPropertyChanged();
            }
        }

        public bool IsViewVisible
        {
            get
            {
                return _isviewvisible;
            }
            set
            {
                _isviewvisible = value;
                OnPropertyChanged();
            }
        }


        public static SecureString pass2;
        #endregion

        #region Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }
        #endregion

        #region Constructor
        public FirstViewModel()
        {
            pass2 = PassWord;
            _userrepository = new UserRepository();

            LoginCommand = new ModelViewCommand(ExecuteLoginCommand , CanExecuteLoginCommand);
           RecoverPasswordCommand = new ModelViewCommand(p=>ExecuteRecoverPasswordCommand("",""));
        }

        private void ExecuteRecoverPasswordCommand(string username , string email)
        {
            throw new NotImplementedException();
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            return true;
        }


        public static string ConvertToUnsecureString(SecureString secureString)
        {
            if (secureString == null)
                throw new ArgumentNullException(nameof(secureString));

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                // Zero out the memory before freeing it
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        // Method to convert the SecureString property to a regular string
        //public string GetPasswordAsString()
        //{
        //    return FirstViewModel.ConvertToUnsecureString(PassWord);
        //}

        

        private void ExecuteLoginCommand(object obj)
        {
            string pass = FirstViewModel.ConvertToUnsecureString(PassWord);

            var isvaliduser = _userrepository.AuthenticateUser(new System.Net.NetworkCredential(Username, PassWord));
            if (isvaliduser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                var mainview = new MainView();
                mainview.Show();
                IsViewVisible = false;
            }
            else
            {
                Errormsg = "* Invalid username or password";
            }


            //con = new SqlConnection(@"Data Source=DESKTOP-ET61S1V\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True");
            //con.Open(); // open the connection

            //string SelectQuery = "Select * From Login_new where username = '" + Username + "' and password = '" + pass + "' ";
            //cmd = new SqlCommand(SelectQuery, con);
            //dr = cmd.ExecuteReader();
            //if (dr.Read())
            //{
            //    dr.Close();
            //    IsViewVisible = false;  //to close and open the next window
            //                            //open the new window
            //    var mainview = new MainView();
            //    mainview.Show();
            //}
            //else
            //{
            //    dr.Close();
            //    Errormsg = "* Invalid username or password";
            //    //MessageBox.Show("No Account avilable with this username and password ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            //con.Close(); // close the connection after use
        }
        #endregion
    }
}
