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
            _isEdit = false;
            _product = new Product();
            this.DataContext = _product;

            ArticleLabel.Visibility = Visibility.Collapsed;
            ArticleTextBox.Visibility = Visibility.Collapsed;
            this.DataContext = _product;
            LoadComboBoxes();
        }

        public ProductEditorWindow(Product selectedProduct)
        {
            InitializeComponent();
            _isEdit = true;
            _product = selectedProduct;
            ArticleLabel.Visibility = Visibility.Visible;
            ArticleTextBox.Visibility = Visibility.Visible;

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
            // 1. Валидация строк и комбобоксов
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Заполните наименование товара!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (CategoryComboBox.SelectedValue == null || ManufacturerComboBox.SelectedValue == null || ProviderComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите значения из всех выпадающих списков!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text.Replace(".", ","), out decimal parsedPrice) || parsedPrice < 0)
            {
                MessageBox.Show("Стоимость должна быть неотрицательным целым или дробным числом!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int decimalPlaces = BitConverter.GetBytes(decimal.GetBits(parsedPrice)[3])[2];
            if (decimalPlaces > 2)
            {
                MessageBox.Show("Стоимость не может содержать более 2 знаков после запятой (копеек)!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(CountTextBox.Text, out int count) || count < 0)
            {
                MessageBox.Show("Количество на складе не может быть отрицательным!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(DiscountTextBox.Text, out int discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть числом от 0 до 100!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    _product.Price = (int)parsedPrice;  //TODO float migration
                    _product.Count = count;
                    _product.CurrentDiscount = discount;
                    _product.Unit = "шт.";

                    if (_isEdit)
                    {
                        context.Entry(_product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    else
                    {
                        int maxId = context.Products.ToList().Select(p => int.TryParse(p.Article, out int id) ? id : 0).DefaultIfEmpty(0).Max();
                        _product.Article = (maxId + 1).ToString();
                        context.Products.Add(_product);
                    }

                    context.SaveChanges();
                }
                BrowsingWindow.BrowsingWindow window = new BrowsingWindow.BrowsingWindow();
                window.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BrowsingWindow.BrowsingWindow window = new BrowsingWindow.BrowsingWindow();
            window.Activate();
            this.Close();
        }
    }
}

