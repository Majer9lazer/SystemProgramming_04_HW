using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using SysProgramming_04_Practice_WPF.Model;

namespace SysProgramming_04_Practice_WPF.Pages
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        DispatcherTimer timer = new DispatcherTimer();
        public AdminPage()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += SetOnlineList;
            timer.Start();
        }
        private void SetOnlineList(object obj,object e)
        {
            try
            {      
                OnlineUsersListView.ItemsSource = RegisterFrame.GetUsers();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
