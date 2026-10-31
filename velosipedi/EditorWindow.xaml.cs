using Microsoft.IdentityModel.Tokens;
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
using velosipedi.DB;

namespace velosipedi
{
    /// <summary>
    /// Логика взаимодействия для EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        private bool _isEdit = false;
        private Item? _item;
        private User? _user;
            
        public EditorWindow(User user)
        {
            InitializeComponent();
            _isEdit = false;
            _item = new Item();
            _user = user;
            IdLabel.Visibility = Visibility.Collapsed;
            IdTextBox.Visibility = Visibility.Collapsed;
            this.DataContext = _item;
            FillComboBox();
        }

        public EditorWindow(User user, Item item)
        {
            InitializeComponent();
            _isEdit = true;
            _item = item;
            _user = user;
            this.DataContext = _item;
            FillComboBox();
        }

        private void FillComboBox()
        {
            using (var context = new ShelkovnDeLab5Context())
            {
                CategoryComboBox.ItemsSource = context.Categories.ToList();
                ManufacturerComboBox.ItemsSource = context.Manufacturers.ToList();
                DealerComboBox.ItemsSource = context.Dealers.ToList();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Заполните наименование", "Не заполнен атрибут", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (CategoryComboBox.SelectedValue == null || ManufacturerComboBox.SelectedValue == null || DealerComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите значения из всех выпадающих списков!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DescriptionTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Заполните описание", "Не заполнен атрибут", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!decimal.TryParse(PriceTextBox.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture, out var ParsedPrice) || ParsedPrice < 0)
            {
                MessageBox.Show("Стоимость должна быть неотрицательным числом", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(CountTextBox.Text, out var ParsedAmount) || ParsedAmount < 0)
            {
                MessageBox.Show("Количество должно быть неотрицательным целым числом", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(DiscountTextBox.Text, out int discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть числом от 0 до 100!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (ArticleTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Заполните артикул", "Не заполнен атрибут", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new ShelkovnDeLab5Context())
                {
                    _item.Price = (float)ParsedPrice;
                    _item.AmountInStock = ParsedAmount;
                    _item.CurrentDiscount = discount;
                    _item.Unit = "шт.";

                    if (_isEdit)
                    {
                        context.Entry(_item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    else
                    {
                        int maxId = context.Items.ToList().Count();
                        _item.ItemId = maxId + 1;
                        context.Items.Add(_item);
                    }

                    context.SaveChanges();
                }
                BrowsingWindow window = new BrowsingWindow(_user);
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
            BrowsingWindow window = new BrowsingWindow(_user);
            window.Activate();
            this.Close();
            
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
