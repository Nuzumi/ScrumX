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
using ScrumX.View;

namespace ScrumX.ViewModel
{
    class TableVM : BindableBase
    {
        private User logedUser;

        #region Properties

        public string UserName { get; set; }

        private bool canAddTask = true;
        public bool CanAddTask
        {
            get { return canAddTask; }
            set
            {
                SetProperty(ref canAddTask, value);
                (AddTaskCommand as DelegateCommand).RaiseCanExecuteChanged();
                (GoToBacklogCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        public ICommand GoToBacklogCommand { get; set; }
        public ICommand AddTaskCommand { get; set; }

        #endregion


        public TableVM(User user)
        {
            GoToBacklogCommand = new DelegateCommand<Window>(GoToBacklogCommandExecute,GoToBacklogCommandCanExecute);
            AddTaskCommand = new DelegateCommand(AddTaskCommandExecute, AddTaskCommandCanExecute);
            logedUser = user;
            UserName = user.Name;
        }

        public void changeCanAddTaskToTrue()
        {
            CanAddTask = true;
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
