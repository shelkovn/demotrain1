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
using velosipedi.DB;

namespace velosipedi.Elements
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
            if (DataContext is Item p)
            {
                if (p.CurrentDiscount >= 20)
                {
                    ProductGrid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#483D8B")!;
                }
                else
                {
                    ProductGrid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff")!;
                }
                DescriptionText.Text = $"Описание товара: {p.ItemDescription}";
                ManufacturerLabel.Content = $"Производитель: {p.Manufacturer.ManufacturerName}";
                ProviderLabel.Content = $"Поставщик: {p.Dealer.DealerName}";
                UnitLabel.Content = $"Единица измерения: {p.Unit}";
                AmountLabel.Content = $"Количество на складе: {p.AmountInStock}";
                DiscountLabel.Content = $"{p.CurrentDiscount}%";

                if (p.CurrentDiscount != 0)
                {
                    OldPriceText.Text = $"Цена: {p.Price}";
                    PriceText.Text = $" {p.Price - p.Price * p.CurrentDiscount * 0.01f:F2}";
                }
                else
                {
                    PriceText.Text = $"Цена: {p.Price}";
                }

                if (!string.IsNullOrEmpty(p.ImagePath))
                {
                    try
                    {
                        ItemImage.Source = new BitmapImage(new Uri($"/Pictures/{p.ImagePath}", UriKind.RelativeOrAbsolute));
                    }
                    catch
                    {
                        ItemImage.Source = new BitmapImage(new Uri("/Pictures/picture.png", UriKind.Relative));
                    }
                }
                else
                {
                    ItemImage.Source = new BitmapImage(new Uri("/Pictures/picture.png", UriKind.Relative));
                }

            }
        }
    }
}
