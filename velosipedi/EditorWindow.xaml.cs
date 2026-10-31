using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string? _img;
        private string? _oldimg;
            
        public EditorWindow(User user)
        {
            InitializeComponent();
            _isEdit = false;
            _item = new Item();
            _user = user;
            IdLabel.Visibility = Visibility.Collapsed;
            IdTextBox.Visibility = Visibility.Collapsed;
            this.DataContext = _item;
            PreviewImage.Source = new BitmapImage(new Uri("/Pictures/picture.png", UriKind.Relative));
            FillComboBox();
        }

        public EditorWindow(User user, Item item)
        {
            InitializeComponent();
            _isEdit = true;
            _item = item;
            _user = user;
            _oldimg = item.ImagePath;
            this.DataContext = _item;
            if (!string.IsNullOrEmpty(_item.ImagePath))
            {
                try
                {
                    PreviewImage.Source = new BitmapImage(new Uri($"/Pictures/{_item.ImagePath}", UriKind.RelativeOrAbsolute));
                }
                catch
                {
                    PreviewImage.Source = new BitmapImage(new Uri("/Pictures/picture.png", UriKind.Relative));
                }
            }
            else
            {
                PreviewImage.Source = new BitmapImage(new Uri("/Pictures/picture.png", UriKind.Relative));
            }
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

                    //TODO _item.ImagePath = _img;


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


                    if (_img != null)
                    {
                        if (_oldimg != null)
                        {
                            //TODO delete old file
                        }
                        string filename = $"{_item.ItemId}_{DateTime.Now.ToString().Replace('.', '_').Replace(':', '_').Replace(' ', '_')}";
                        string extension = System.IO.Path.GetExtension(_img);
                        string directory = System.IO.Path.GetDirectoryName(_img);
                        string path = System.IO.Path.Combine(directory, filename + extension);

                        _item.ImagePath = $"{filename + extension}";
                        File.Copy(_img, path, true);

                        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        string targetDirectory = "";

                        if (baseDirectory.Contains(@"\bin\"))
                        {
                            DirectoryInfo projectDir = Directory.GetParent(baseDirectory).Parent.Parent.Parent;

                            if (projectDir != null)
                            {
                                targetDirectory = System.IO.Path.Combine(projectDir.FullName, "Pictures");
                            }
                        }
                        else
                        {
                            targetDirectory = System.IO.Path.Combine(baseDirectory, "Pictures");
                        }

                        if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);

                        File.Move(path, System.IO.Path.Combine(targetDirectory, filename + extension), true);
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
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Изображения| *.jpg;*.jpeg;*.png";
                dialog.CheckPathExists = true;
                if (dialog.ShowDialog() == true)
                {
                    BitmapImage image = new BitmapImage();
                    using (var stream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                    }
                    image.Freeze();
                    if (image.PixelWidth > 300 || image.PixelHeight > 200)
                    {
                        MessageBox.Show($"Изображение должно быть меньше 300х200 пикселей", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    _img = dialog.FileName;
                    PreviewImage.Source = image;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошбика при получении изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
