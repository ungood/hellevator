using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Hellevator.Simulator.ViewModels
{
    public class RelayCommand : ICommand
    {
        #region Fields

        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute)
            : this(execute, null) {}

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if(execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        #endregion
    }
}