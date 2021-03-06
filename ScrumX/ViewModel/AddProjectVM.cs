﻿using Prism.Commands;
using Prism.Mvvm;
using ScrumX.API.Model;
using ScrumX.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScrumX.ViewModel
{
    class AddProjectVM : BindableBase
    {
        private Action changeCanAddProjectToTrue;
        private EfRepository repo;
        private User logedUser;
        private Project projectToEdit;

        #region Properties

        public string ButtonText { get; set; }

        private string projectName;
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                SetProperty(ref projectName, value);
                (AddProjectCommand as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddProjectCommand { get; set; }
        public ICommand CancleCommand { get; set; }

        #endregion

        public AddProjectVM(Action changeCanAddProjectToTrue, User user)
        {
            baseConstructor(changeCanAddProjectToTrue, user);
            ButtonText = "Dodaj projekt";
        }

        public AddProjectVM(Action changeCanAddProjectToTrue,User user,Project projectToEdit)
        {
            baseConstructor(changeCanAddProjectToTrue, user);
            projectName = projectToEdit.Name;
            this.projectToEdit = projectToEdit;
            ButtonText = "zapisz";
        }
        
        private void baseConstructor(Action changeCanAddProjectToTrue,User user)
        {
            logedUser = user;
            AddProjectCommand = new DelegateCommand<Window>(AddProjectCommandExecute, AddProjectCommandCanExecute);
            CancleCommand = new DelegateCommand<Window>(CancleCommandExecute);
            this.changeCanAddProjectToTrue = changeCanAddProjectToTrue;
            repo = new EfRepository();
        }

        #region Command Functions

        private void AddProjectCommandExecute(Window window)
        {
            if(projectToEdit == null)
            {
                repo.ProjectsRepo.AddProject(ProjectName);
                
            }
            else
            {
                Project project = repo.ProjectsRepo.FindProjectByName(projectToEdit.Name);
                repo.ProjectsRepo.ChangeName(project, ProjectName);
            }
            repo.SaveChanges();
            changeCanAddProjectToTrue.DynamicInvoke();
            window.Close();
        }

        private bool AddProjectCommandCanExecute(Window dummy)
        {
            return ProjectName != null && ProjectName != "";
        }

        private void CancleCommandExecute(Window window)
        {
            changeCanAddProjectToTrue.DynamicInvoke();
            window.Close();
        }

        #endregion
    }
}
