using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для BrowsingWindow.xaml
    /// </summary>
    /// 

    public partial class BrowsingWindow : Window
    {
        public ObservableCollection<Item> Items = [];
        private CollectionViewSource _itemSource;
        private User? _user;
        public BrowsingWindow(User? user)
        {
            InitializeComponent();
            _user = user;
            RefreshList();
        }

        public void RefreshList()
        {
            try
            {
                Items.Clear();
                using (var context = new ShelkovnDeLab5Context())
                {
                    var items = context.Items.Include(p => p.Category).Include(p => p.Dealer).Include(p => p.Manufacturer);

                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                }
                if (ItemList.ItemsSource == null)
                {
                    _itemSource = new() { Source = Items };
                    ItemList.ItemsSource = _itemSource.View;
                    _itemSource.Filter += _itemSource_Filter;
                }
                else
                {
                    _itemSource.View.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void _itemSource_Filter(object sender, FilterEventArgs e)
        {
            string? query = SearchBox.Text;
            int? discountind = DiscountRange.SelectedIndex;
            if (query.IsNullOrEmpty() && discountind == null)
            {
                e.Accepted = true;
            }
            else
            {
                bool accepted = true;
                if (e.Item is Item i)
                {
                    if (discountind != null && discountind != 0 && discountind != -1)
                    {
                        switch (discountind)
                        {
                            case 1:
                                accepted = i.CurrentDiscount < 12;
                                break;
                            case 2:
                                accepted = i.CurrentDiscount >= 12 && i.CurrentDiscount < 19;
                                break;
                            case 3:
                                accepted = i.CurrentDiscount >= 19;
                                break;
                            default: break;
                        }
                    }
                    if (!query.IsNullOrEmpty() && accepted) //if already rejected by discount rage, do not search
                    {
                        bool articleHit = i.ItemArticle?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false;
                        bool nameHit = i.ItemName?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false;
                        bool unitHit = i.Unit?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false;
                        bool descHit = i.ItemDescription?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false;
                        bool imageHit = i.ImagePath?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false;
                        bool dealerHit = i.Dealer?.DealerName?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false;
                        bool manufacturerHit = i.Manufacturer?.ManufacturerName?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false;
                        accepted = articleHit || nameHit || unitHit || descHit || imageHit || dealerHit || manufacturerHit;
                    }
                }
                e.Accepted = accepted;
            }
        }

        private void Sort()
        {
            _itemSource.SortDescriptions.Clear();
            int amountind = AmountSort.SelectedIndex;
            int priceind = PriceSort.SelectedIndex;
            int discountind = DiscountSort.SelectedIndex;

            if (amountind != -1 && amountind != 0)
                _itemSource.SortDescriptions.Add(new SortDescription("AmountInStock", amountind == 1 ? ListSortDirection.Ascending : ListSortDirection.Descending));
            if (priceind != -1 && priceind != 0)
                _itemSource.SortDescriptions.Add(new SortDescription("Price", amountind == 1 ? ListSortDirection.Ascending : ListSortDirection.Descending));
            if (discountind != -1 && discountind != 0)
                _itemSource.SortDescriptions.Add(new SortDescription("CurrentDiscount", amountind == 1 ? ListSortDirection.Ascending : ListSortDirection.Descending));

            _itemSource.View.Refresh();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_itemSource != null)
                _itemSource.View.Refresh();
        }

        private void AmountSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_itemSource != null)
            {
                Sort();
            }
        }

        private void PriceSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_itemSource != null)
            {
                Sort();
            }
        }

        private void DiscountSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_itemSource != null)
            {
                Sort();
            }
        }

        private void DiscountRange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_itemSource != null)
                _itemSource.View.Refresh();
        }
    }
}
