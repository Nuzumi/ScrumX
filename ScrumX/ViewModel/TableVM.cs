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

        private bool canAddTask;
        public bool CanAddTask
        {
            get { return canAddTask; }
            set
            {
                SetProperty(ref canAddTask, value);
                riseCanExecuteForDialog();
                (GoToBacklogCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private bool canAddSprint;
        public bool CanAddSprint
        {
            get { return canAddSprint; }
            set
            {
                SetProperty(ref canAddSprint, value);
                riseCanExecuteForDialog();
                (GoToBacklogCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private bool canAddProject;
        public bool CanAddProject
        {
            get { return canAddProject; }
            set
            {
                SetProperty(ref canAddProject, value);
                riseCanExecuteForDialog();
                (GoToBacklogCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        public ICommand GoToBacklogCommand { get; set; }
        public ICommand AddTaskCommand { get; set; }
        public ICommand AddSprintCommand { get; set; }
        public ICommand AddProjectCommand { get; set; }

        #endregion


        public TableVM(User user)
        {
            GoToBacklogCommand = new DelegateCommand<Window>(GoToBacklogCommandExecute,GoToBacklogCommandCanExecute);
            AddTaskCommand = new DelegateCommand(AddTaskCommandExecute, AddTaskCommandCanExecute);
            AddSprintCommand = new DelegateCommand(AddSprintCommandExecuted, AddSprintCommandCanExecute);
            AddProjectCommand = new DelegateCommand(AddProjectCommandExecute, AddProjectCommandCanExecute);
            CanAddTask = true;
            CanAddSprint = true;
            CanAddProject = true;
            logedUser = user;
            UserName = user.Name;
        }

        public void changeCanAddTaskToTrue()
        {
            CanAddTask = true;
        }

        public void changeCanAddSprintToTrue()
        {
            CanAddSprint = true;
        }

        public void changeCanAddProjectToTrue()
        {
            CanAddProject = true;
        }

        private void riseCanExecuteForDialog()
        {
            (AddTaskCommand as DelegateCommand).RaiseCanExecuteChanged();
            (AddSprintCommand as DelegateCommand).RaiseCanExecuteChanged();
            (AddProjectCommand as DelegateCommand).RaiseCanExecuteChanged();
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

        #region DialogCommand
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
            return CanAddTask && CanAddSprint && CanAddProject;
        }

        private void AddSprintCommandExecuted()
        {
            CanAddSprint = false;
            AddSprintVM dataContext = new AddSprintVM(changeCanAddSprintToTrue);
            AddSprint dialog = new AddSprint();
            dialog.DataContext = dataContext;
            dialog.Show();
        }

        private bool AddSprintCommandCanExecute()
        {
            return CanAddSprint && CanAddTask && CanAddProject;
        }

        private void AddProjectCommandExecute()
        {
            CanAddProject = false;
            AddProjectVM dataContext = new AddProjectVM(changeCanAddProjectToTrue);
            AddProject dialog = new AddProject();
            dialog.DataContext = dataContext;
            dialog.Show();
        }

        private bool AddProjectCommandCanExecute()
        {
            return CanAddProject && CanAddSprint && CanAddTask;
        }
        #endregion
        #endregion
    }
}
