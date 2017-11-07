using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScrumX.ViewModel
{
    class AddTaskVM : BindableBase
    {
        private Action changeCanAddTaskToTrue;

        private delegate void canShowThisWindowToTrue();
        
        #region Properties

        public ICommand AddTaskCommand { get; set; }

        #endregion

        public AddTaskVM(Action changeCanAddTaskToTrue)
        {
            AddTaskCommand = new DelegateCommand<Window>(AddTaskCommandExecute);
            this.changeCanAddTaskToTrue = changeCanAddTaskToTrue;
        }

        #region Command Functions

        private void AddTaskCommandExecute(Window window)
        {
            changeCanAddTaskToTrue.DynamicInvoke();
            window.Close();
        }

        #endregion
    }
}
