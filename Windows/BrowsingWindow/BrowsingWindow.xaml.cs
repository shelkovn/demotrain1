using demo1.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

namespace demo1.Windows.BrowsingWindow
{
    /// <summary>
    /// Логика взаимодействия для BrowsingWindow.xaml
    /// </summary>
    public partial class BrowsingWindow : Window
    {
        public ObservableCollection<Product> Products = [];
        private CollectionViewSource productsSource;
        private int? _userRoleId;
        private User _currentUser;
        public BrowsingWindow(User? currentUser = null)
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
            if (SearchBar.Text.IsNullOrEmpty())
            {
                e.Accepted = true;
            }
            else
            {
                if (e.Item is Product p)
                {
                    bool nameHit = p.Name?.Contains(SearchBar.Text, StringComparison.OrdinalIgnoreCase) ?? false;
                    bool articleHit = p.Article?.Contains(SearchBar.Text, StringComparison.OrdinalIgnoreCase) ?? false;
                    bool unitHit = p.Unit?.Contains(SearchBar.Text, StringComparison.OrdinalIgnoreCase) ?? false;
                    bool descriptionHit = p.Description?.Contains(SearchBar.Text, StringComparison.OrdinalIgnoreCase) ?? false;

                    bool manufacturerHit = p.Manufacturer?.Name?.Contains(SearchBar.Text, StringComparison.OrdinalIgnoreCase) ?? false;
                    bool providerHit = p.Provider?.Name?.Contains(SearchBar.Text, StringComparison.OrdinalIgnoreCase) ?? false;
                    bool categoryHit = p.Category?.Name?.Contains(SearchBar.Text, StringComparison.OrdinalIgnoreCase) ?? false;

                    e.Accepted = nameHit || articleHit || unitHit || descriptionHit || manufacturerHit || providerHit || categoryHit;
                }
            }
        }

        private void ToAuth_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            productsSource.View.Refresh();
        }
    }
}
