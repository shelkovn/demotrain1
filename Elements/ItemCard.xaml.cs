using demo1.DB;
using demo1.Windows.BrowsingWindow;
using demo1.Windows.ProductEditorWindow;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace demo1.Elements
{
    /// <summary>
    /// Логика взаимодействия для ItemCard.xaml
    /// </summary>
    public partial class ItemCard : UserControl
    {
        public ItemCard()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is Product p)
            {
                if (p.CurrentDiscount >= 20)
                {
                    ProductGrid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#C8A2C8")!;
                }
                else
                {
                    ProductGrid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#f8f2f8")!;
                }
                DescriptionText.Text = $"Описание товара: {p.Description}";
                ManufacturerLabel.Content = $"Производитель: {p.Manufacturer.Name}";
                ProviderLabel.Content = $"Поставщик: {p.Provider.Name}";
                UnitLabel.Content = $"Единица измерения: {p.Unit}";
                AmountLabel.Content = $"Количество на складе: {p.Count}";
                DiscountLabel.Content = $"{p.CurrentDiscount}%";

                if (p.CurrentDiscount != 0)
                {
                    OldPriceText.Text = $"Цена: {p.Price}";
                    PriceText.Text = $" {p.Price - p.Price*p.CurrentDiscount*0.01}";
                }
                else
                {
                    PriceText.Text = $"Цена: {p.Price}";
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Product p && Window.GetWindow(this) is BrowsingWindow window)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить {p.Name}?",
                                     "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using (var _context = new AppDbContext())
                    {
                        bool isInOrders = _context.OrderProducts.Any(op => op.Article == p.Article);

                        if (isInOrders)
                        {
                            MessageBox.Show("Запрещено удалять товар, так как он присутствует в заказах!");
                            return;
                        }
                        var product = _context.Products.Find(p.Article);
                        if (product != null)
                        {
                            _context.Products.Remove(product);
                            _context.SaveChanges();
                            window.RefreshData();
                        }
                    }
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ProductEditorWindow window = new ProductEditorWindow();
            window.Activate();
            if (Window.GetWindow(this) is BrowsingWindow w)
            {
                w.Close();
            }
        }
    }
}
