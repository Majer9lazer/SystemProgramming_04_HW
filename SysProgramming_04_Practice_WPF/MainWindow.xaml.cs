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

namespace SysProgramming_04_Practice_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Frame mframe;
        public MainWindow()
        {
            mframe = MainFrame;
            InitializeComponent();
            //mframe.Source = new Uri(@"C:\Users\СидоренкоЕ\Desktop\ConsoleApp2\SysProgramming_04_Practice_WPF\RegisterFrame.xaml");
        }
    }
}
