using MVVM_Project.Command;
using MVVM_Project.Model;
using MVVM_Project.View;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace MVVM_Project.ViewModel
{
    public class AddTeacherViewModel : TeachersViewModel
    {
        #region DataBase
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader dr;
        #endregion

        #region Fields
        private string _firstname;
        private string _lastname;
        private string _id;

        #endregion

        #region Prop

        public string FisrtName
        {
            get
            {
                return _firstname;
            }
            set
            {
                _firstname = value;
                OnPropertyChanged(nameof(FisrtName));
            }
        }

        public string LastName
        {
            get
            {
                return _lastname;
            }
            set
            {
                _lastname = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        #endregion

        public ICommand AddTeacherCommand { get; }
        public ICommand CancelCommand { get; }

        public AddTeacherViewModel()
        {
            AddTeacherCommand = new ModelViewCommand(AddTeacher);
            CancelCommand = new ModelViewCommand(Cancel);
        }

        public void AddTeacher(object obj)
        {
            Teacher newTeacher = new Teacher
            {
                FirstName = FisrtName,
                LastName = LastName,
                Id = Convert.ToInt32(ID)
            };

            using (con = new SqlConnection(@"Data Source=DESKTOP-ET61S1V\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True"))
            {
                con.Open();

                // check if the fields are not empty
                if (!string.IsNullOrEmpty(FisrtName) && !string.IsNullOrEmpty(LastName))
                {
                    string insertQuery = "INSERT INTO Teacher (FirstName, LastName, ID) VALUES (@FirstName, @LastName, @Id)";
                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@FirstName", FisrtName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@Id", ID);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        
                        MessageBox.Show("Teacher added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // Insert failed
                        MessageBox.Show("Failed to add teacher", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public void Cancel(object obj)
        {
            // Get the reference to the AddStudentWindow
            Window addTeacherWindow = Application.Current.Windows.OfType<AddTeacherWindow>().FirstOrDefault();

            // Close the window if it is not null
            addTeacherWindow?.Close();
        }
    }
}
