using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.XPath;
using SysProgramming_04_Practice_WPF.Model;
namespace SysProgramming_04_Practice_WPF
{
    /// <summary>
    /// Interaction logic for RegisterFrame.xaml
    /// </summary>
    /// 

    public partial class RegisterFrame : Page
    {
        private static XDocument xdoc;
        public static string filename = "";
        static bool existed;
        private static Mutex m;
        public RegisterFrame()
        {
            InitializeComponent();
            GetStringDirectory();
        }

        public static List<User> GetUsers()
        {
            GetStringDirectory();
            xdoc = XDocument.Load(filename);
            var t = xdoc.Elements("Users").Elements("User").ToList();
            List<User> lusersList = new List<User>();

            Thread a = new Thread(delegate ()
             {
                 foreach (XElement element in t)
                 {
                     User u = new User();
                     foreach (XElement xElement in element.Elements())
                     {
                         if (xElement.Name == "UserId")
                         {
                             u.UserId = int.Parse(xElement.Value);
                         }
                         if (xElement.Name == "UserMail")
                         {
                             u.UserMail = xElement.Value;
                         }
                         if (xElement.Name == "UserLogin")
                         {
                             u.UserLogin = xElement.Value;

                         }
                         if (xElement.Name == "UserPassword")
                         {
                             u.UserPassword = xElement.Value;
                         }

                         if (xElement.Name == "IsOnline")
                         {
                             u.IsOnline = xElement.Value;
                         }
                     }
                     lusersList.Add(u);
                 }
             });
            a.Start();
            a.Join();
            return lusersList;
        }
        public static void GetStringDirectory()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent;
            if (directoryInfo?.Parent == null) return;
            DirectoryInfo dir = directoryInfo.Parent
                .GetDirectories().FirstOrDefault(w => w.Name == "LocalDataBase");
            filename = (dir?.GetFiles().FirstOrDefault()?.FullName);
        }
        static string Login = "";
        static string Mail = "";
        static string Password = "";
        private static readonly User Us = new User();
        public static User GetUser()
        {
            return Us;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login = UserLoginTextBox.Text;
            Mail = UserMailTextBox.Text;
            Password = UserPasswordTextBox.Text;
            List<Thread> lt = new List<Thread>()
            {
                new Thread(delegate()

                {

                    if(!string.IsNullOrEmpty(Login)&&!string.IsNullOrEmpty(Mail)&&!string.IsNullOrEmpty(Password))
                    {
                            xdoc=XDocument.Load(filename);

                            String maxId = xdoc.Elements("Users").Elements()
                            .Where(w => w.Name.ToString()=="User").Elements()
                            .Where(w=>w.Name=="UserId")
                            .Select(s=>s.Value.Replace('\n',' '))
                            .ToList().Max();
                        if (xdoc.Elements("Users").Elements()
                            .Where(w => w.Name.ToString()=="User")
                            .Elements().Where(w=>w.Name=="UserLogin" ||w.Name=="UserMail")
                            .Any(w=>w.Name=="UserLogin"&&w.Value==Login))
                        {
                            MessageBox.Show("Вам нельзя снова зарегестрироваться)))");
                        }
                        else
                        {
                            try
                            {
                                xdoc.Element("Users").Add(
                                    new XElement("User",
                                        new XElement("UserId"){Value = (int.Parse(maxId) + 1).ToString()},
                                        new XElement("UserMail"){Value = Mail},
                                        new XElement("UserLogin"){Value = Login},
                                        new XElement("UserPassword"){Value = Password},
                                        new XElement("IsOnline"){Value = "No"}));
                                xdoc.Save(filename);                           
                                //Db.Users_.Add(a);
                                //Db.SaveChanges();
                                MessageBox.Show("Вы были зарегестрированы успешно успешно");
                            }
                            catch (Exception ex)
                            {
                                var msg=ex.Message;
                                MessageBox.Show(msg);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Что-то пришло пустым!");
                    }
                }),
                new Thread(delegate() {
                    MessageBox.Show("Если всё будет нормально , вы будете скоро зарегестрированы)");

                })
            };
            foreach (Thread thread in lt)
            {
                thread.Start();
            }

            UserLoginTextBox.Text = null;
            UserMailTextBox.Text = null;
            UserPasswordTextBox.Text = null;
            //MessageBox.Show("Вы были зарегестрированы успешно");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Login = UserLoginTextBox.Text;
            Mail = UserMailTextBox.Text;
            Password = UserPasswordTextBox.Text;
            xdoc = XDocument.Load(filename);
            if (string.IsNullOrEmpty(Login) && string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(Mail))
            {
                MessageBox.Show("Какое то Поле  пустое!");
            }
            else
            {
                List<Thread> lt = new List<Thread>()
                {
                    new Thread(delegate()
                    {
                        foreach (XElement item in xdoc.Elements("Users").Elements("User"))
                        {
                                XElement[] a=item.Elements().ToArray();
                                if (a[1].Value == Mail && a[2].Value == Login && a[3].Value == Password)
                                {
                                    m= new Mutex(true,Mail,out existed);
                                    if (existed)
                                    {
                                        Us.UserId = int.Parse(a[0].Value);
                                        Us.UserMail = a[1].Value;
                                        Us.UserLogin = a[2].Value;
                                        Us.UserPassword = a[3].Value;
                                        Us.IsOnline = a[4].Value;
                                        MessageBox.Show($"Hii {a[2].Value}");
                                        a[4].Value = "Yes";
                                        xdoc.Save(filename);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Ohhhh User I'am so so so sorry , u have already logged in))))) ");
                                        a[4].Value = "Yes";
                                        xdoc.Save(filename);
                                    }
                                }
                        }
                    })
                };
                lt.ForEach(f => f.Start());
                IfAdmin(Mail, Password, Login);
            }
        }
        private void IfAdmin(string mail, string password, string login)
        {
            if (login == "admin" && password == "admin")
            {

                bool Ischeckd;
                m = new Mutex(true, login, out Ischeckd);
                if (Ischeckd)
                {
                    MainWindow.mframe.Source = new Uri(@"Pages\AdminPage.xaml", UriKind.RelativeOrAbsolute);
                }
                else
                {
                    MessageBox.Show("Вы хоть и админ , " +
                                    "но два раза нельзя заходить под одним и " +
                                    "тем же логином , " +
                                    "увы такова политика нашей компании)");
                }
            }

        }

    }
}
