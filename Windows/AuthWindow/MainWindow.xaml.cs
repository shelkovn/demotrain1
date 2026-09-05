using demo1.DB;
using demo1.Windows.BrowsingWindow;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;

namespace demo1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //pass user to BrowsingWindow (null for guest)
        private void AuthClick(object sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text;
            var password = PasswordBox.Password;
            if (!login.IsNullOrEmpty() && !password.IsNullOrEmpty())
            {
                using (var context = new AppDbContext())
                {
                    var user = context.Users.Include(u => u.Role).FirstOrDefault(u =>
                        u.Login == login &&
                        u.Password == password
                        );
                    if (user == null)
                    {
                        MessageBox.Show("Неверный логин или пароль");
                        return;
                    }
                    if (user.RoleId == null)
                    {
                        MessageBox.Show("Отсутствует роль пользователя");
                        return;
                    }
                    var window = new BrowsingWindow(user);
                    window.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Введите логин и пароль");
            }
        }

        private void GuestClick(object sender, RoutedEventArgs e)
        {
            var window = new BrowsingWindow(null);
            window.Show();
            this.Close();
        }
    }
}