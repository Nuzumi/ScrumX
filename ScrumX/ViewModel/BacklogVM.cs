using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumX.API.Model;
using System.Windows.Input;
using Prism.Commands;
using System.Windows;
using ScrumX.ViewModel;
using ScrumX.View;
using ScrumX.HelperClasses;

namespace ScrumX.ViewModel
{
    class BacklogVM : DialogDisplay
    {
        private User logedUser;

        #region Properties

        public string UserName { get; set; }

        public ICommand GoToTableCommand { get; set; }
        #endregion

        public BacklogVM(User user) :base()
        {
            GoToTableCommand = new DelegateCommand<Window>(GoToTableCommandExecute,GoToTableCommandCanExecute);
            logedUser = user;
            UserName = user.Name;
        }

        protected override void riseGoToCommands()
        {
            if(GoToTableCommand != null)
            {
                (GoToTableCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }


        #region CommandFunctions

        private void GoToTableCommandExecute(Window window)
        {
            TableVM dataContext = new TableVM(logedUser);
            Table table = new Table();
            table.DataContext = dataContext;
            table.Show();
            window.Close();
        }

        private bool GoToTableCommandCanExecute(Window dummy)
        {
            return CanAddTask && CanAddProject && CanAddSprint;
        }
        #endregion
    }
}
