using Pustovoy.Lopushok.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pustovoy.Lopushok.Presentation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? _viewModel = null;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (MainWindowViewModel)DataContext;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(_viewModel != null)
                _viewModel.Search(TextBoxInput.Text);
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.Sort(ComboBoxSort.SelectedIndex);
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.Filter(ComboBoxFilter.ItemsSource as List<string>, ComboBoxFilter.SelectedIndex);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.ShowProductWindow();
        }
    }
}
