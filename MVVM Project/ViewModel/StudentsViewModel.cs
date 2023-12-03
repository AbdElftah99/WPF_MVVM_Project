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
    public class StudentsViewModel  : FirstViewModelBase
    {
        // Define the custom event
        public event EventHandler StudentAdded;

        // Method to raise the event
        public virtual void OnStudentAdded()
        {
            StudentAdded?.Invoke(this, EventArgs.Empty);
        }

        #region DataBase
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader dr;
        #endregion

        #region Fields
        private ObservableCollection<Student> _studentList;
        private Student _selectedStudent;
        private AddStudentViewModel _addStudentViewModel;

        private string _searchText;
        #endregion

        #region Prop
        public ObservableCollection<Student> StudentList
        {
            get { return _studentList; }
            set
            {
                _studentList = value;
                OnPropertyChanged();
            }
        }

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }
        public AddStudentViewModel AddStudentViewModel
        {
            get { return _addStudentViewModel; }
            set
            {
                _addStudentViewModel = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand AddStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }
        public ICommand SearchCommand { get; }
        #endregion

        #region Constructor
        public StudentsViewModel()
        {
            StudentList = new ObservableCollection<Student>();
            // Fetch data from the database and populate the StudentList
            FetchStudents();


            AddStudentCommand = new ModelViewCommand(OpenAddStudentWindow);
            DeleteStudentCommand = new ModelViewCommand(DeleteStudent, CanDeleteStudent);
            SearchCommand = new ModelViewCommand(SearchStudent);
          
            // Subscribe to the StudentAdded event from AddStudentViewModel
            //AddStudentViewModel addStudentViewModel = new AddStudentViewModel();
            //addStudentViewModel.StudentAdded += AddStudentViewModel_StudentAdded;
        }
        #endregion

        #region Methods
        //private void AddStudentViewModel_StudentAdded(object sender, EventArgs e)
        //{
        //    // Refresh the student list in response to the event
        //    FetchStudents();
        //}

        private void SearchStudent(object parameter)
        {
            // Implement your search logic using the searchText parameter
            // Connect to the database and search for students with matching first names
            using (con = new SqlConnection(@"Data Source=DESKTOP-ET61S1V\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True"))
            {
                con.Open();

                string selectQuery = "SELECT * FROM Student WHERE FirstName LIKE @SearchText";
                SqlCommand cmd = new SqlCommand(selectQuery, con);
                cmd.Parameters.AddWithValue("@SearchText", "%" + _searchText + "%");
                SqlDataReader dr = cmd.ExecuteReader();

                // Clear the existing student list
                StudentList.Clear();

                while (dr.Read())
                {
                    // Create a new Student object and populate its properties
                    Student student = new Student
                    {
                        Id = (int)dr["ID"],
                        FirstName = (string)dr["FirstName"],
                        LastName = (string)dr["LastName"]
                    };

                    // Add the student to the StudentList collection
                    StudentList.Add(student);
                }

                dr.Close();
            }

            // Clear the currently selected student
            SelectedStudent = null;

            // Select the first student in the search results
            if (StudentList.Count > 0)
            {
                SelectedStudent = StudentList[0];
            }
        }

        public void FetchStudents()
        {
            using (con = new SqlConnection(@"Data Source=DESKTOP-ET61S1V\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True"))
            {
                con.Open();

                string selectQuery = "SELECT * FROM Student";
                SqlCommand cmd = new SqlCommand(selectQuery, con);
                SqlDataReader dr = cmd.ExecuteReader();

                // Clear the existing student list
                StudentList.Clear();

                while (dr.Read())
                {
                    // Create a new Student object and populate its properties
                    Student student = new Student
                    {
                        Id = (int)dr["ID"],
                        FirstName = (string)dr["FirstName"],
                        LastName = (string)dr["LastName"]
                    };

                    // Add the student to the StudentList collection
                    StudentList.Add(student);
                }

                dr.Close();
            }
        }

        private void OpenAddStudentWindow(object parameter)
        {
            var addStudentWindow = new AddStudentWindow();
            AddStudentViewModel = new AddStudentViewModel(); // Create an instance of AddStudentViewModel
            addStudentWindow.DataContext = AddStudentViewModel; // Assign the instance to the DataContext
            addStudentWindow.ShowDialog();

            FetchStudents();
        }

        public void AddStudentToList(Student student)
        {
                StudentList.Add(student);
        }

        private void DeleteStudent(object parameter)
        {
            if (SelectedStudent != null)
            {
                // Prompt the user for confirmation
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this student?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (con = new SqlConnection(@"Data Source=DESKTOP-ET61S1V\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True"))
                    {
                        con.Open();

                        string deleteQuery = "DELETE FROM Student WHERE ID = @Id";
                        SqlCommand cmd = new SqlCommand(deleteQuery, con);
                        cmd.Parameters.AddWithValue("@Id", SelectedStudent.Id);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Deletion successful
                            MessageBox.Show("Student deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Refresh the student list
                            FetchStudents();
                        }
                        else
                        {
                            // Deletion failed
                            MessageBox.Show("Failed to delete student", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private bool CanDeleteStudent(object parameter)
        {
            if (StudentList.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //private void UpdateStudent(object parameter)
        //{
        //    // Add your logic to update the selected student
        //}
        #endregion
    }
}
