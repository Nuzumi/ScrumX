﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using ScrumX.API.Context;
using ScrumX.API.Model;
using ScrumX.API.Repository;
using ScrumX.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScrumX.ViewModel
{
    class LoginVM : BindableBase
    {
        private User user;
        private bool loginMode = true;
        private EfRepository repo;
        IDialogCoordinator _dialogCoordinator;

        #region Properties

        private string labelContent;
        public string LabeleContent
        {
            get { return labelContent; }
            set { SetProperty(ref labelContent, value); }
        }

        private bool isVisibleLogin;
        public bool IsVisibleLogin
        {
            get { return isVisibleLogin; }
            set
            {
                SetProperty(ref isVisibleLogin, value);
                (RegisterCommand as DelegateCommand).RaiseCanExecuteChanged();
                (LoginCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        private bool isNotVisibleLogin;
        public bool IsNotVisibleLogin
        {
            get { return isNotVisibleLogin; }
            set
            {
                SetProperty(ref isNotVisibleLogin, value);
                (RegisterCommand as DelegateCommand).RaiseCanExecuteChanged();
                (LoginCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        private bool passwordCorrect;
        public bool PasswordCorrect
        {
            get { return passwordCorrect; }
            set { SetProperty(ref passwordCorrect, value); }
        }

        private bool loginCorrect;
        public bool LoginCorrect
        {
            get { return loginCorrect; }
            set { SetProperty(ref loginCorrect, value); }
        }

        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                SetProperty(ref login, value);
                (OkComamnd as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                SetProperty(ref password, value);
                (OkComamnd as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        private string passwordAgain;
        public string PasswordAgain
        {
            get { return passwordAgain; }
            set
            {
                SetProperty(ref passwordAgain, value);
                (OkComamnd as DelegateCommand<Window>).RaiseCanExecuteChanged();
            }
        }

        public ICommand OkComamnd { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        #endregion

        public LoginVM(IDialogCoordinator dialogCoordinator)
        {
            OkComamnd = new DelegateCommand<Window>(OkCommandExecute, OKCommandCanExecute);
            RegisterCommand = new DelegateCommand(RegisterCommandExecute, RegisterCommandCanExecute);
            LoginCommand = new DelegateCommand(LoginCommandExecute, LoginCommandCanExecute);
            LabeleContent = "Logowanie";
            repo = new EfRepository();
            LoginCorrect = true;
            PasswordCorrect = true;
            _dialogCoordinator = dialogCoordinator;
            repo.CheckSprints(); 
            //sprawdza czy jakies sprinty nie sa przedawnione -> zamyka je i resetuje SP w zadaniach
        }

        #region commanadFunction

        private async void OkCommandExecute(Window window)
        {
            if (loginMode)
            {
                int log = repo.UsersRepo.UserLogin(Login, Password);
                if (log == 1)
                {
                    BacklogVM dataContext = new BacklogVM(repo.UsersRepo.GetUserByName(login), _dialogCoordinator);
                    Backlog backlog = new Backlog();
                    backlog.DataContext = dataContext;
                    backlog.Show();
                    window.Close();
                }
                else if (log == 0)
                {
                    var metroWindow = (Application.Current.MainWindow as MetroWindow);
                    await metroWindow.ShowMessageAsync("Ups!", "Użytkownik nie istnieje");
                    ClearTextBoxes();
                }
                else
                {
                    var metroWindow = (Application.Current.MainWindow as MetroWindow);
                        await metroWindow.ShowMessageAsync("Ups!", "Błędne hasło");
                    ClearTextBoxes();
                }
            }
            else
            {
                if (repo.UsersRepo.RegisterUser(Login, Password))
                {
                    var metroWindow = (Application.Current.MainWindow as MetroWindow);
                    await metroWindow.ShowMessageAsync("Super!", "Zarejestrowano");
                    LoginCommandExecute();
                }
                else
                {
                    var metroWindow = (Application.Current.MainWindow as MetroWindow);
                    await metroWindow.ShowMessageAsync("Ups!", "Nick jest zajęty");
                    ClearTextBoxes();
                }
            }
        }

        private bool OKCommandCanExecute(Window dummyWindow)
        {
            if (loginMode)
            {
                return true;
            }
            else
            {
                //PasswordCorrect = (Password != string.Empty && Password != null);
                LoginCorrect = !repo.UsersRepo.UserExists(Login);

                return (!repo.UsersRepo.UserExists(Login) && Login != string.Empty && Login != null && Password != string.Empty && Password != null);
            }
        }

        private void RegisterCommandExecute()
        {
            LabeleContent = "Rejestracja";
            loginMode = false;
            IsVisibleLogin = true;
            IsNotVisibleLogin = false;
            ClearTextBoxes();
            (OkComamnd as DelegateCommand<Window>).RaiseCanExecuteChanged();
        }

        private bool RegisterCommandCanExecute()
        {
            return loginMode;
        }

        private void LoginCommandExecute()
        {
            LabeleContent = "Logowanie";
            loginMode = true;
            IsVisibleLogin = false;
            IsNotVisibleLogin = true;

            ClearTextBoxes();
            (OkComamnd as DelegateCommand<Window>).RaiseCanExecuteChanged();
        }

        private bool LoginCommandCanExecute()
        {
            return !loginMode;
        }

        #endregion

        public void ClearTextBoxes()
        {
            Password = PasswordAgain = Login = "";
        }
    }
}