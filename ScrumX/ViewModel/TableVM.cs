﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumX.API.Model;
using System.Windows.Input;
using System.Windows;
using Prism.Commands;
using ScrumX.View;
using ScrumX.HelperClasses;

namespace ScrumX.ViewModel
{
    class TableVM : DialogDisplay
    {

        #region Properties

        public string UserName { get; set; }

        public ICommand GoToBacklogCommand { get; set; }

        #endregion


        public TableVM(User user) :base(user)
        {
            GoToBacklogCommand = new DelegateCommand<Window>(GoToBacklogCommandExecute,GoToBacklogCommandCanExecute);
            logedUser = user;
            UserName = user.Name;
        }

        protected override void riseGoToCommands()
        {
            if(GoToBacklogCommand != null)
            {
                (GoToBacklogCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        #region CommandFunctions

        private void GoToBacklogCommandExecute(Window window)
        {
            BacklogVM dataContext = new BacklogVM(logedUser); // mozna by przekazywac ta wczesniej uzywana klase by parametry wyszukiwania zostawaly
            Backlog backlog = new Backlog();
            backlog.DataContext = dataContext;
            backlog.Show();
            window.Close();
        }

        private bool GoToBacklogCommandCanExecute(Window dummy)
        {
            return CanAddTask;
        }
        #endregion
    }
}
