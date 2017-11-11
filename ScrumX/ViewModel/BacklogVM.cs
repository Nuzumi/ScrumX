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
using ScrumX.API.Content;

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

        private string tag;
        public string Tag
        {
            get { return tag; }
            set
            {
                SetProperty(ref tag, value);
                SearchJobCommandExecute();
            }
        }

        private typeBacklog selectedType;
        public string SelectedType
        {
            get { return selectedType.ToString(); }
            set
            {
                switch (value)
                {
                    case "None":
                        SetProperty(ref selectedType, typeBacklog.None);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject,(int)typeBacklog.None));
                        break;

                    case "New":
                        SetProperty(ref selectedType, typeBacklog.New);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)typeBacklog.New));
                        break;

                    case "Ready":
                        SetProperty(ref selectedType, typeBacklog.Ready);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)typeBacklog.Ready));
                        break;

                    case "Scheduled":
                        SetProperty(ref selectedType, typeBacklog.Scheduled);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)typeBacklog.Scheduled));
                        break;

                    case "Completed":
                        SetProperty(ref selectedType, typeBacklog.Completed);
                        Jobs = new ObservableCollection<Job>(repo.JobsRepo.GetJobsInBacklog(SelectedProject, (int)typeBacklog.Completed));
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

        private int selectedSP;
        public int SelectedSP
        {
            get { return selectedSP; }
            set
            {
                SetProperty(ref selectedSP, value);
                if(SelectedJob != null)
                {
                    repo.JobsRepo.ChangeJobSP(SelectedJob, value, logedUser);
                }
            }
        }

        public ICommand DeleteProjectCommand { get; set; }
        public ICommand DeleteJobCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand GoToTableCommand { get; set; }
        #endregion

        public BacklogVM(User user) :base(user)
        {
            GoToTableCommand = new DelegateCommand<Window>(GoToTableCommandExecute,GoToTableCommandCanExecute);
            DeleteProjectCommand = new DelegateCommand(DeleteProjectCommandExecute);
            DeleteJobCommand = new DelegateCommand(DeleteJobCommandExecute);
            SearchCommand = new DelegateCommand(SearchJobCommandExecute);
            TypeList = new List<string> { "None", "New", "Ready", "Scheduled", "Completed" };
            StoryPointValues = new List<int> { 0, 1, 2, 3, 5, 8, 13, 20, 40, 100 };
            PriorityValues = new List<int> { 1, 2, 3, 4, 5 };
            repo = new EfRepository();
            logedUser = user;
            UserName = user.Name;
            SetProperties();
        }

        public void SetProperties()
        {
            Projects = new ObservableCollection<Project>(repo.ProjectsRepo.Projects);
            if (Projects != null && Projects.Count() > 0)
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

        private void DeleteProjectCommandExecute()
        {
            Console.WriteLine("Usuwam projekt");
            repo.ProjectsRepo.DeleteProject(SelectedProject);
            SetProperties();
        }

        private void DeleteJobCommandExecute()
        {
            Console.WriteLine("Usuwam zadanie");
            repo.JobsRepo.DeleteJob(SelectedJob);
            SetProperties();
        }

        private void SearchJobCommandExecute()
        {
            Jobs = new ObservableCollection<Job>(repo.JobsRepo.SearchJob(SelectedProject, Tag));
            Console.Write(Jobs.Count());
        }

        private bool GoToTableCommandCanExecute(Window dummy)
        {
            return CanAddTask && CanAddProject && CanAddSprint;
        }
        #endregion
    }
}
