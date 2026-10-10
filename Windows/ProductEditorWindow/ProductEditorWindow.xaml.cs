using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using demo1.DB;

namespace demo1.Windows.ProductEditorWindow
{
    /// <summary>
    /// Логика взаимодействия для ContactEditorWindow.xaml
    /// </summary>
    public partial class ProductEditorWindow : Window
    {
        private Product? _product;
        private bool _isEdit = false;
        public ProductEditorWindow()
        {
            InitializeComponent();
            _product = new Product { Article = Guid.NewGuid().ToString().Substring(0, 10) };
            this.DataContext = _product;
            LoadComboBoxes();
        }

        public ProductEditorWindow(Product selectedProduct)
        {
            InitializeComponent();
            _isEdit = true;
            _product = selectedProduct;

            this.DataContext = _product;
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            using (var context = new AppDbContext())
            {
                CategoryComboBox.ItemsSource = context.ProductCategories.ToList();
                ManufacturerComboBox.ItemsSource = context.Companies.ToList();
                ProviderComboBox.ItemsSource = context.Companies.ToList();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_product.Name))
            {
                MessageBox.Show("Заполните наименование товара!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CategoryComboBox.SelectedValue == null ||
                ManufacturerComboBox.SelectedValue == null ||
                ProviderComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите категорию, производителя и поставщика!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    if (_isEdit)
                    {
                        context.Entry(_product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    else
                    {
                        context.Products.Add(_product);
                    }

                    context.SaveChanges();
                }
                this.DialogResult = true;
                BrowsingWindow.BrowsingWindow window = new BrowsingWindow.BrowsingWindow();
                window.Activate();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            BrowsingWindow.BrowsingWindow window = new BrowsingWindow.BrowsingWindow();
            window.Activate();
            this.Close();
        }
    }
}

