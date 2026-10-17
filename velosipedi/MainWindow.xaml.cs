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
using velosipedi.DB;

namespace velosipedi
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

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text;
            var password = PasswordBox.Password;
            if (!login.IsNullOrEmpty() && !password.IsNullOrEmpty())
            {
                try
                {
                    using (var context = new ShelkovnDeLab5Context())
                    {
                        var user = context.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefault();
                        if (user == null)
                        {
                            MessageBox.Show("Неверный логин или пароль");
                            return;
                        }
                        var window = new BrowsingWindow(user);
                        window.Show();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Введите логин и пароль!");
            }
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new BrowsingWindow(null);
            window.Show();
            this.Close();
        }
    }
}