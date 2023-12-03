using FontAwesome.Sharp;
using MVVM_Project.Command;
using MVVM_Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_Project.ViewModel
{
    public class MainViewViewModel : FirstViewModelBase
    {
        private FirstViewModelBase currentChildView;
        private string caption;
        private IconChar icon;

        #region Prop
        public FirstViewModelBase CurrentChildView
        {
            get
            {
                return currentChildView;
            }
            set
            {
                currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        public IconChar Icon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
        #endregion

        public ICommand ShowHomeViewCommand { get;}
        public ICommand ShowStudentViewCommand { get; }
        public ICommand ShowCoursesViewCommand { get; }
        public ICommand ShowTeacherViewCommand { get; }

        public MainViewViewModel()
        {
            //Initialize Component
            ShowHomeViewCommand = new ModelViewCommand(ExecuteShowHomeViewCommand);
            ShowStudentViewCommand = new ModelViewCommand(ExecuteShowStudentViewCommand);
            //ShowCoursesViewCommand = new ModelViewCommand(ExecuteShowCoursesViewCommand);
            ShowTeacherViewCommand = new ModelViewCommand(ExecuteTeacherViewCommand);

            //Default View
            ExecuteShowHomeViewCommand(null);
        }

        private void ExecuteTeacherViewCommand(object obj)
        {

            CurrentChildView = new TeachersViewModel();
            Caption = "Teacher";
        }

        //private void ExecuteShowCoursesViewCommand(object obj)
        //{
        //    CurrentChildView = new CoursesViewModel();
        //    Caption = "Courses";
            
        //}

        private void ExecuteShowStudentViewCommand(object obj)
        {
            CurrentChildView = new StudentsViewModel();
            Caption = "Student";
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Home";
            Icon = IconChar.Home;
        }
    }
}
