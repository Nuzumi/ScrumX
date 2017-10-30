using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumX.API.Model;
using System.Windows.Input;
using System.Windows;
using Prism.Commands;

namespace ScrumX.ViewModel
{
    class TableVM : BindableBase
    {
        private User logedUser;

        #region Properties

        public string UserName { get; set; }

        public ICommand GoToBacklogCommand { get; set; }

        #endregion


        public TableVM(User user)
        {
            GoToBacklogCommand = new DelegateCommand<Window>(goToBacklogCommandExecute);
            logedUser = user;
            UserName = user.Name;
        }

        #region CommandFunctions

        private void goToBacklogCommandExecute(Window window)
        {
            BacklogVM dataContext = new BacklogVM(logedUser); // mozna by przekazywac ta wczesniej uzywana klase by parametry wyszukiwania zostawaly
            Backlog backlog = new Backlog();
            backlog.DataContext = dataContext;
            backlog.Show();
            window.Close();
        }

        #endregion
    }
}
