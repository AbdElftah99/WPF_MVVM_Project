using FontAwesome.Sharp;
using MVVM_Project.Command;
using MVVM_Project.Model;
using MVVM_Project.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVM_Project.ViewModel
{
    public class AddStudentViewModel : StudentsViewModel
    {
       

        #region DataBase
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader dr;
        #endregion

        #region Fields
        private string _firstname;
        private string _lastname;
        private int _id;
        private StudentsViewModel _studentsViewModel;
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

        public int ID
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

        public ICommand AddStudentCommand { get; }
        public ICommand CancelCommand { get; }

        public AddStudentViewModel()
        {
            AddStudentCommand = new ModelViewCommand(AddStudent);
           
            CancelCommand = new ModelViewCommand(Cancel);
        }

        public void AddStudent(object obj)
        {
            Student newStudent = new Student
            {
                FirstName = FisrtName,
                LastName = LastName,
                Id = ID
            };

            using (con = new SqlConnection(@"Data Source=DESKTOP-ET61S1V\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True"))
            {
                con.Open();

                // check if the fields are not empty
                if (!string.IsNullOrEmpty(FisrtName) && !string.IsNullOrEmpty(LastName))
                {
                    string insertQuery = "INSERT INTO Student (FirstName, LastName, ID) VALUES (@FirstName, @LastName, @Id)";
                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@FirstName", FisrtName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@Id", ID);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        //// Add the student to the StudentList collection in StudentsViewModel
                        //StudentsViewModel studentsViewModel = Application.Current.MainWindow.DataContext as StudentsViewModel;
                        //studentsViewModel?.AddStudentToList(newStudent);
                        // Insert successful
                        MessageBox.Show("Student added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        StudentsViewModel studentsViewModel = new StudentsViewModel();
                        studentsViewModel.StudentList.Add(newStudent);
                    }
                    else
                    {
                        // Insert failed
                        MessageBox.Show("Failed to add student", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

    

        public void Cancel(object obj)
        {
            // Get the reference to the AddStudentWindow
            Window addStudentWindow = Application.Current.Windows.OfType<AddStudentWindow>().FirstOrDefault();

            // Close the window if it is not null
            addStudentWindow?.Close();
        }
    }
}
