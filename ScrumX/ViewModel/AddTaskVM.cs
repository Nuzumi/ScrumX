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
        private Job jobToEdit;

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
                if(value != null)
                {
                    SetProperty(ref taskProject, value);
                    (AddTaskCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
                    Sprints = new ObservableCollection<Sprint>(repo.SprintsRepo.GetSprintsForProject(value.IdProject));
                }
            }
        }

        public List<double> PriorityValues { get; set; }
        public List<double> StoryPointValues { get; set; }

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

        private double selectedSP;
        public double SelectedSP
        {
            get { return selectedSP; }
            set
            {
                SetProperty(ref selectedSP, value);
                (AddTaskCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private double selectedPriority;
        public double SelectedPriority
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

        private int projectValue;
        public int ProjectValue
        {
            get { return projectValue; }
            set { SetProperty(ref projectValue, value);
                Console.WriteLine(value);
            }
        }

        public int SPValue { get; set; }
        public int PriorityValue { get; set; }

        public string AddTaskText { get; set; }

        public ICommand AddTaskCommand { get; set; }
        public ICommand CancleCommand { get; set; }

        #endregion

        public AddTaskVM(Action changeCanAddTaskToTrue, User user)
        {
            baseConstructor(changeCanAddTaskToTrue, user);
            AddTaskText = "Dodaj zadanie";
        }

        public AddTaskVM(Action changeCanAddTaskToTrue, User user, Job job)
        {
            baseConstructor(changeCanAddTaskToTrue, user);
            jobToEdit = job;
            TaskTitle = job.Title;
            TaskDescription = job.Desc;
            if (job.Priority.HasValue)
            {
                SelectedPriority = job.Priority.Value;
                for(int i = 0; i < PriorityValues.Count; i++)
                {
                    if(PriorityValues[i] == SelectedPriority)
                    {
                        PriorityValue = i;
                    }
                }
            }
            if (job.SP.HasValue)
            {     
                SelectedSP = job.SP.Value;
                for(int i =0;i < StoryPointValues.Count; i++)
                {
                    if(StoryPointValues[i] == SelectedSP)
                    {
                        SPValue = i;
                    }
                }
            }
            TaskProject = job.Project;
            for(int i =0; i < Projects.Count; i++)
            {
                if(Projects[i] == TaskProject)
                {
                    ProjectValue = i;
                }
            }
            AddTaskText = "zapisz";
        }

       private void baseConstructor(Action changeCanAddTaskToTrue, User user)
        {
            logedUser = user;
            AddTaskCommand = new DelegateCommand<Window>(AddTaskCommandExecute, AddTAskCommandCanExecute);
            CancleCommand = new DelegateCommand<Window>(CancleCommandExecute);
            this.changeCanAddTaskToTrue = changeCanAddTaskToTrue;
            repo = new EfRepository();
            Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
            TaskDescription = "";
            StoryPointValues = new List<double> { 1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 20.0, 40.0, 100.0 };
            PriorityValues = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0 };
        }


        #region Command Functions

        private void AddTaskCommandExecute(Window window)
        {
            if(jobToEdit == null)
            {
                Job task = new Job
                {
                    IdUser = logedUser.IdUser,
                    IdSprint = 1,//TaskSprint.IdSprint,
                    Title = TaskTitle,
                    Desc = TaskDescription,
                    IdProject = TaskProject.IdProject,
                    SP = SelectedSP,
                    Priority = SelectedPriority
                };
                repo.JobsRepo.AddJob(task);
                
            }
            else
            {
                jobToEdit.Title = TaskTitle;
                jobToEdit.Priority = SelectedPriority;
                jobToEdit.SP = SelectedSP;
                jobToEdit.Desc = TaskDescription;
                jobToEdit.Project = TaskProject;
                repo.JobsRepo.EditJob(jobToEdit);
            }
            repo.SaveChanges();
            changeCanAddTaskToTrue.DynamicInvoke();
            window.Close();
        }

        private bool AddTAskCommandCanExecute(Window dummy)
        {
            return TaskTitle != null && TaskTitle != "" && TaskProject != null;
        }

        private void CancleCommandExecute(Window window)
        {
            changeCanAddTaskToTrue.DynamicInvoke();
            window.Close();
        }

        #endregion
    }
}
