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
using SysProgramming_04_Practice_WPF.Model;
namespace SysProgramming_04_Practice_WPF
{
    /// <summary>
    /// Interaction logic for RegisterFrame.xaml
    /// </summary>
    /// 

    public partial class RegisterFrame : Page
    {
        static Users Db = new Users();
        public RegisterFrame()
        {
            InitializeComponent();
        }
        static string Login = "";
        static string Mail = "";
        static string Password = "";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login = UserLoginTextBox.Text;
            Mail = UserMailTextBox.Text;
            Password = UserPasswordTextBox.Text;
            List<Thread> lt = new List<Thread>()
            {
                new Thread(delegate()
                {
                    string msg="";
                    if(!string.IsNullOrEmpty(Login)&&!string.IsNullOrEmpty(Mail)&&!string.IsNullOrEmpty(Password))
                    {
                            User a= new User();
                            a.UserLogin=Login;
                            a.UserMail=Login;
                            a.UserPassword=Login;
                      try
                        {
                            Db.Users_.Add(a);
                            Db.SaveChanges();
                            MessageBox.Show("Добавление в базу произошло успешно");
                        }
                        catch (Exception ex)
                        {
                          msg=ex.Message;
                        MessageBox.Show(msg);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Что-то пришло пустым!");
                    }
                })

            };
            foreach (Thread thread in lt)
            {
                thread.Start();
                thread.Join();
            }
            MessageBox.Show("Вы были зарегестрированы успешно");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
