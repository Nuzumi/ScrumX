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

namespace ScrumX.ViewModel
{
    class BacklogVM : BindableBase
    {
        private User logedUser;

        #region Properties

        public string UserName { get; set; }

        public ICommand GoToTableCommand { get; set; }
        #endregion

        public BacklogVM(User user)
        {
            GoToTableCommand = new DelegateCommand<Window>(GoToTableExecute);
            logedUser = user;
            UserName = user.Name;
        }

        #region CommandFunctions

        private void GoToTableExecute(Window window)
        {
            TableVM dataContext = new TableVM(logedUser);
            Table table = new Table();
            table.DataContext = dataContext;
            table.Show();
            window.Close();
        }

        #endregion
    }
}
