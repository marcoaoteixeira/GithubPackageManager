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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Nameless.Client.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace Nameless.Client.Views.Pages {
    public partial class AppConfigurationPage : INavigationAware {
        #region Public Properties

        public AppConfigurationViewModel ViewModel { get; }

        #endregion

        #region Public Constructors

        public AppConfigurationPage(AppConfigurationViewModel viewModel) {
            ArgumentNullException.ThrowIfNull(viewModel, nameof(viewModel));

            ViewModel = viewModel;

            InitializeComponent();
        }

        #endregion

        #region INavigationAware Members

        public void OnNavigatedTo() { }

        public void OnNavigatedFrom() { }

        #endregion
    }
}
