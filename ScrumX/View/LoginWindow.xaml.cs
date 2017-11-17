using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ScrumX.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScrumX.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        private readonly LoginVM _viewModel;

        public LoginWindow()
        {
            _viewModel = new LoginVM(DialogCoordinator.Instance);
            DataContext = _viewModel;


            InitializeComponent();
        }
    }
}
