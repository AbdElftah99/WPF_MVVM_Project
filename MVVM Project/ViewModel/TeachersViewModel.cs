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
using System.Windows.Input;
using System.Windows;

namespace MVVM_Project.ViewModel
{
    public class TeachersViewModel : FirstViewModelBase
    {
        #region DataBase
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader dr;
        #endregion

        #region Fields
        private ObservableCollection<Teacher> _teacherList;
        private Teacher _selectedTeacher;
        private AddTeacherViewModel _addTeacherViewModel;

        private string _searchText;
        #endregion

        #region Prop
        public ObservableCollection<Teacher> TeacherList
        {
            get { return _teacherList; }
            set
            {
                _teacherList = value;
                OnPropertyChanged();
            }
        }

        public Teacher SelectedTeacher
        {
            get { return _selectedTeacher; }
            set
            {
                _selectedTeacher = value;
                OnPropertyChanged();
            }
        }
        public AddTeacherViewModel AddTeacherViewModel
        {
            get { return _addTeacherViewModel; }
            set
            {
                _addTeacherViewModel = value;
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

        private Teacher _foundTeacher;
        public Teacher FoundTeacher
        {
            get { return _foundTeacher; }
            set
            {
                _foundTeacher = value;
                OnPropertyChanged(nameof(FoundTeacher));
            }
        }
        #endregion

        #region Commands
        public ICommand AddTeacherCommand { get; }
        public ICommand DeleteTeacherCommand { get; }
        public ICommand SearchCommand { get; }
        #endregion

        #region Constructor
        public TeachersViewModel()
        {
            TeacherList = new ObservableCollection<Teacher>();

            // Fetch data from the database and populate the StudentList

            FetchTeachers();

            AddTeacherCommand = new ModelViewCommand(OpenAddTeacherWindow);
            DeleteTeacherCommand = new ModelViewCommand(DeleteTeacher, CanDeleteTeacher);
            SearchCommand = new ModelViewCommand(SearchTeacher);
        }
        #endregion

        #region Methods


        private void SearchTeacher(object parameter)
        {
            // Clear the currently selected teacher
            SelectedTeacher = null;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // If search text is empty or contains only white spaces,
                // reset the TeacherList to show all teachers
                FetchTeachers();
            }
            else
            {
                // Search by ID
                if (int.TryParse(SearchText, out int id))
                {
                    Teacher foundTeacherById = TeacherList.FirstOrDefault(t => t.Id == id);
                    if (foundTeacherById != null)
                    {
                        TeacherList.Clear();
                        TeacherList.Add(foundTeacherById);
                    }
                }
                else
                {
                    // Search by first name
                    Teacher foundTeacherByFirstName = TeacherList.FirstOrDefault(t => t.FirstName.ToLower().Contains(SearchText.ToLower()));
                    if (foundTeacherByFirstName != null)
                    {
                        TeacherList.Clear();
                        TeacherList.Add(foundTeacherByFirstName);
                    }
                    else
                    {
                        // Search by last name
                        Teacher foundTeacherByLastName = TeacherList.FirstOrDefault(t => t.LastName.ToLower().Contains(SearchText.ToLower()));
                        if (foundTeacherByLastName != null)
                        {
                            TeacherList.Clear();
                            TeacherList.Add(foundTeacherByLastName);
                        }
                        else
                        {
                            // If no match found for any search criteria, reset the TeacherList to show all teachers
                            FetchTeachers();
                        }
                    }
                }
            }
        }

        public void FetchTeachers()
        {
            using (con = new SqlConnection(@"Data Source=DESKTOP-ET61S1V\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True"))
            {
                con.Open();

                string selectQuery = "SELECT * FROM Teacher";
                SqlCommand cmd = new SqlCommand(selectQuery, con);
                SqlDataReader dr = cmd.ExecuteReader();

                // Clear the existing student list
                TeacherList.Clear();

                while (dr.Read())
                {
                    // Create a new teacher object and populate its properties
                    Teacher teacher = new Teacher
                    {
                        Id = (int)dr["ID"],
                        FirstName = (string)dr["FirstName"],
                        LastName = (string)dr["LastName"]
                    };

                    // Add the teacher to theteacherList collection
                    TeacherList.Add(teacher);
                }

                dr.Close();
            }
        }

        private void OpenAddTeacherWindow(object parameter)
        {
            var addTeacherWindow = new AddTeacherWindow();
            AddTeacherViewModel = new AddTeacherViewModel(); // Create an instance of AddStudentViewModel
            addTeacherWindow.DataContext = AddTeacherViewModel; // Assign the instance to the DataContext
            addTeacherWindow.ShowDialog();

            FetchTeachers();
        }

        public void AddTeacherToList(Teacher teacher)
        {
            TeacherList.Add(teacher);
        }

        private void DeleteTeacher(object parameter)
        {
            if (SelectedTeacher != null)
            {
                // Prompt the user for confirmation
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this Teacher?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (con = new SqlConnection(@"Data Source=DESKTOP-ET61S1V\SQLEXPRESS;Initial Catalog=Register;Integrated Security=True"))
                    {
                        con.Open();

                        string deleteQuery = "DELETE FROM Teacher WHERE ID = @Id";
                        SqlCommand cmd = new SqlCommand(deleteQuery, con);
                        cmd.Parameters.AddWithValue("@Id", SelectedTeacher.Id);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Deletion successful
                            MessageBox.Show("Teacher deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Refresh the student list
                            FetchTeachers();
                        }
                        else
                        {
                            // Deletion failed
                            MessageBox.Show("Failed to delete teacher", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private bool CanDeleteTeacher(object parameter)
        {
            if (TeacherList.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
