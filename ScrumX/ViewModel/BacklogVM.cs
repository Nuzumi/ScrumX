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

        private bool canAddTask;
        public bool CanAddTask
        {
            get { return canAddTask; }
            set
            {
                SetProperty(ref canAddTask, value);
                (AddTaskCommand as DelegateCommand).RaiseCanExecuteChanged();
                (GoToTableCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        public ICommand GoToTableCommand { get; set; }
        public ICommand AddTaskCommand { get; set; }
        #endregion

        public BacklogVM(User user)
        {
            GoToTableCommand = new DelegateCommand<Window>(GoToTableCommandExecute,GoToTableCommandCanExecute);
            AddTaskCommand = new DelegateCommand(AddTaskCommandExecute, AddTaskCommandCanExecute);
            CanAddTask = true;
            logedUser = user;
            UserName = user.Name;
        }

        public void changeCanAddTaskToTrue()
        {
            CanAddTask = true;
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
            return CanAddTask;
        }

        private void AddTaskCommandExecute()
        {
            CanAddTask = false;
            AddTaskVM dataContext = new AddTaskVM(changeCanAddTaskToTrue);
            AddTask dialog = new AddTask();
            dialog.DataContext = dataContext;
            dialog.Show();
        }

        private bool AddTaskCommandCanExecute()
        {
            return CanAddTask;
        }
        #endregion
    }
}
