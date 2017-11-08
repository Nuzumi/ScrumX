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
    class AddProjectVM : BindableBase
    {
        private Action changeCanAddProjectToTrue;

        #region Properties

        public ICommand AddProjectCommand { get; set; }

        #endregion

        public AddProjectVM(Action changeCanAddProjectToTrue)
        {
            AddProjectCommand = new DelegateCommand<Window>(AddProjectCommandExecute);
            this.changeCanAddProjectToTrue = changeCanAddProjectToTrue;
        }

        #region Command Functions

        private void AddProjectCommandExecute(Window window)
        {
            changeCanAddProjectToTrue.DynamicInvoke();
            window.Close();
        }

        #endregion
    }
}
