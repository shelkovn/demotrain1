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
            
        public EditorWindow()
        {
            InitializeComponent();
            _isEdit = false;
            _item = new Item();
            IdLabel.Visibility = Visibility.Collapsed;
            IdTextBox.Visibility = Visibility.Collapsed;
            this.DataContext = _item;
        }

        public EditorWindow(Item item)
        {
            InitializeComponent();
            _isEdit = true;
            _item = item;
            this.DataContext = _item;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
