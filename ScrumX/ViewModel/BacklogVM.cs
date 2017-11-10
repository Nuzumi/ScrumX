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
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ScrumX.API.Repository;

namespace ScrumX.ViewModel
{
    class BacklogVM : DialogDisplay
    {
        private EfRepository repo;
        #region Properties

        public List<int> PriorityValues { get; set; }
        public List<int> StoryPointValues { get; set; }
        public string UserName { get; set; }
        public List<string> TypeList { get; set; }

        private ObservableCollection<Project> projects;
        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set { SetProperty(ref projects, value); }
        }

        private Project selectedProject;
        public Project SelectedProject
        {
            get { return selectedProject; }
            set
            {
                SetProperty(ref selectedProject, value);
                Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)selectedType));
            }
        }

        private type selectedType;
        public string SelectedType
        {
            get { return selectedType.ToString(); }
            set
            {
                switch (value)
                {
                    case "None":
                        SetProperty(ref selectedType, type.None);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject,(int)type.None));
                        break;

                    case "New":
                        SetProperty(ref selectedType, type.New);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)type.New));
                        break;

                    case "Ready":
                        SetProperty(ref selectedType, type.Ready);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)type.Ready));
                        break;

                    case "Scheduled":
                        SetProperty(ref selectedType, type.Scheduled);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)type.Scheduled));
                        break;

                    case "Completed":
                        SetProperty(ref selectedType, type.Completed);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)type.Completed));
                        break;
                }
            }
        }

        private ObservableCollection<Job> jobs;
        public ObservableCollection<Job> Jobs
        {
            get { return jobs; }
            set { SetProperty(ref jobs, value); }
        }

        private Job selectedJob;
        public Job SelectedJob
        {
            get { return selectedJob; }
            set
            {
                SetProperty(ref selectedJob, value);
            }
        }


        public ICommand SearchCommand { get; set; }
        public ICommand GoToTableCommand { get; set; }
        #endregion

        public BacklogVM(User user) :base(user)
        {
            GoToTableCommand = new DelegateCommand<Window>(GoToTableCommandExecute,GoToTableCommandCanExecute);
            TypeList = new List<string> { "None", "New", "Ready", "Scheduled", "Completed" };
            StoryPointValues = new List<int> { 0, 1, 2, 3, 5, 8, 13, 20, 40, 100 };
            PriorityValues = new List<int> { 1, 2, 3, 4, 5 };
            repo = new EfRepository();
            logedUser = user;
            UserName = user.Name;
            Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
            if(Projects[0] != null)
            {
                SelectedProject = Projects[0];
                Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsForProject(SelectedProject));
            }
            else
            {
                Jobs = new ObservableCollection<Job>();
            }
        }

        protected override void riseGoToCommands()
        {
            if(GoToTableCommand != null)
            {
                (GoToTableCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
                Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject,(int)selectedType));
                Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
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
