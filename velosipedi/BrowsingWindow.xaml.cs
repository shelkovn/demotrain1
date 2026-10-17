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
    }
}
