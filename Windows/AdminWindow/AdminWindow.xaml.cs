using demo1.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace demo1.Windows.AdminWindow
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public ObservableCollection<Product> Products = [];
        private CollectionViewSource productsSource;
        private int? _userRoleId;
        private User _currentUser;
        public AdminWindow(User? currentUser = null)
        {
            InitializeComponent();
            if (currentUser == null)
            {
                UsernameLabel.Content = "Гость";
            }
            else
            {
                UsernameLabel.Content = currentUser.FullName;
                this.Title = this.Title + $" - {currentUser.Role.Name ?? currentUser.RoleId.ToString()}";
                _userRoleId = currentUser.RoleId;
                _currentUser = currentUser;
            }
            using (var context = new AppDbContext())
            {
                var items = context.Products.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.Provider);
                foreach (var item in items)
                {
                    Products.Add(item);
                }
            }
            productsSource = new()
            {
                Source = Products
            };
            productsSource.Filter += ItemsFilter;
            ProductList.ItemsSource = productsSource.View;
        }

        private void ItemsFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = true;
        }

        private void ToAuth_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}

