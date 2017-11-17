using Prism.Commands;
using Prism.Mvvm;
using ScrumX.API.Model;
using ScrumX.API.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScrumX.ViewModel
{
    class AddTaskVM : BindableBase
    {
        private Action changeCanAddTaskToTrue;
        private User logedUser;
        private EfRepository repo;

        #region Properties

        private string taskTitle;
        public string TaskTitle
        {
            get { return taskTitle; }
            set
            {
                SetProperty(ref taskTitle, value);
                (AddTaskCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Project> projects;
        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set { SetProperty(ref projects, value); }
        }

        private Project taskProject;
        public Project TaskProject
        {
            get { return taskProject; }
            set
            {
                SetProperty(ref taskProject, value);
                //(AddTaskCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
                Sprints = new ObservableCollection<Sprint>(repo.SprintsRepo.GetSprintsForProject(value.IdProject));
            }
        }

        public List<int> PriorityValues { get; set; }
        public List<int> StoryPointValues { get; set; }

        private ObservableCollection<Sprint> sprints;
        public ObservableCollection<Sprint> Sprints
        {
            get { return sprints; }
            set { SetProperty(ref sprints, value); }
        }

        private Sprint taskSprint;
        public Sprint TaskSprint
        {
            get { return taskSprint; }
            set
            {
                SetProperty(ref taskSprint, value);
                (AddTaskCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private int selectedSP;
        public int SelectedSP
        {
            get { return selectedSP; }
            set
            {
                SetProperty(ref selectedSP, value);
                (AddTaskCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private int selectedPriority;
        public int SelectedPriority
        {
            get { return selectedPriority; }
            set
            {
                SetProperty(ref selectedPriority, value);
                (AddTaskCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private string taskDescription;
        public string TaskDescription
        {
            get { return taskDescription; }
            set{ SetProperty(ref taskDescription, value); }
        }

        public ICommand AddTaskCommand { get; set; }
        public ICommand CancleCommand { get; set; }

        #endregion

        public AddTaskVM(Action changeCanAddTaskToTrue, User user)
        {
            logedUser = user;
            AddTaskCommand = new DelegateCommand<Window>(AddTaskCommandExecute,AddTAskCommandCanExecute);
            CancleCommand = new DelegateCommand<Window>(CancleCommandExecute);
            this.changeCanAddTaskToTrue = changeCanAddTaskToTrue;
            repo = new EfRepository();
            Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
            TaskDescription = "";
            StoryPointValues = new List<int> { 1, 2, 3, 5, 8, 13, 20, 40, 100 };
            PriorityValues = new List<int> { 1, 2, 3, 4, 5 };
        }



        #region Command Functions

        private void AddTaskCommandExecute(Window window)
        {
            Job task = new Job { IdUser = logedUser.IdUser, IdSprint = 1,//TaskSprint.IdSprint,
                Title = TaskTitle, Desc = TaskDescription, IdProject = TaskProject.IdProject,
                SP = SelectedSP, Priority = SelectedPriority};
            repo.JobsRepo.AddJob(task);
            repo.SaveChanges();
            changeCanAddTaskToTrue.DynamicInvoke();
            window.Close();
        }

        private bool AddTAskCommandCanExecute(Window dummy)
        {
            return TaskTitle != null && TaskTitle != "";
        }

        private void CancleCommandExecute(Window window)
        {
            changeCanAddTaskToTrue.DynamicInvoke();
            window.Close();
        }

        #endregion
    }
}
