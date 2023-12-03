using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_Project.Command
{
    public class ModelViewCommand : ICommand
    {
        #region Fields
        public Action<Object> _executeAction;
        public Predicate<Object> _canExecuteAction;
        #endregion

        #region Constructor
        public ModelViewCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        public ModelViewCommand(Action<object> executeAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = null;
        }
        #endregion

        #region Event
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }
        #endregion

        #region Methods
        public bool CanExecute(object parameter)
        {
          return _canExecuteAction == null ? true : _canExecuteAction(parameter);
        }

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }
        #endregion

    }
}
