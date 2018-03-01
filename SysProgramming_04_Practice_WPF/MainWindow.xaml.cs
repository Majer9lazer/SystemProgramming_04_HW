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
using System.Xml.Linq;
using SysProgramming_04_Practice_WPF.Model;

namespace SysProgramming_04_Practice_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame mframe;
        public MainWindow()
        {

            InitializeComponent();
            mframe = MainFrame;
            mframe.Source = new Uri(@"Pages\RegisterFrame.xaml", UriKind.RelativeOrAbsolute);
            this.Closing += Onclosing;
        }

        private void Onclosing(object e, object a_)
        {
            string filename = RegisterFrame.filename;
            RegisterFrame.GetStringDirectory();
            XDocument xdoc = XDocument.Load(filename);
            User us = RegisterFrame.GetUser();
            foreach (XElement item in xdoc.Elements("Users").Elements("User"))
            {
                XElement[] a = item.Elements().ToArray();
                if (a[1].Value == us.UserMail && a[2].Value == us.UserLogin && a[3].Value == us.UserPassword)
                {
                    a[4].Value = "No";
                    xdoc.Save(filename);
                }
            }
        }
    }
}
