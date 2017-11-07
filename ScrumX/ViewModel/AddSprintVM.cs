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
    class AddSprintVM : BindableBase
    {
        private Action changeCanAddSprintToTrue;

        private delegate void canShowThisWindowToTrue();

        #region Properties

        public ICommand AddSprintCommand { get; set; }

        #endregion

        public AddSprintVM(Action changeCanAddSprintToTrue)
        {
            AddSprintCommand = new DelegateCommand<Window>(AddSprintCommandExecute);
            this.changeCanAddSprintToTrue = changeCanAddSprintToTrue;
        }

        #region Command Functions

        private void AddSprintCommandExecute(Window window)
        {
            changeCanAddSprintToTrue.DynamicInvoke();
            window.Close();
        }

        #endregion
    }
}
