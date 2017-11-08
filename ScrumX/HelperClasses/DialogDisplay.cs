using Prism.Commands;
using Prism.Mvvm;
using ScrumX.View;
using ScrumX.ViewModel;
using System.Windows.Input;

namespace ScrumX.HelperClasses
{
    abstract class DialogDisplay : BindableBase
    {

        #region properties
        private bool canAddTask;
        public bool CanAddTask
        {
            get { return canAddTask; }
            set
            {
                SetProperty(ref canAddTask, value);
                riseCanExecuteForDialog();
                riseGoToCommands();
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
                riseGoToCommands();
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
                riseGoToCommands();
            }
        }

        public ICommand AddTaskCommand { get; set; }
        public ICommand AddSprintCommand { get; set; }
        public ICommand AddProjectCommand { get; set; }

        #endregion

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

        protected abstract void riseGoToCommands();

        public DialogDisplay()
        {
            AddTaskCommand = new DelegateCommand(AddTaskCommandExecute, AddTaskCommandCanExecute);
            AddSprintCommand = new DelegateCommand(AddSprintCommandExecuted, AddSprintCommandCanExecute);
            AddProjectCommand = new DelegateCommand(AddProjectCommandExecute, AddProjectCommandCanExecute);
            CanAddTask = true;
            CanAddSprint = true;
            CanAddProject = true;
        }

        #region Command Functions

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

    }
}
