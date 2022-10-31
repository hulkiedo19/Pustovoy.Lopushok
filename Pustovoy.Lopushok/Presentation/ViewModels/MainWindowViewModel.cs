using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Pustovoy.Lopushok.Domain.Entities;
using Pustovoy.Lopushok.Infrastucture.Persistence;
using Pustovoy.Lopushok.Presentation.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Pustovoy.Lopushok.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<Product> _allProducts = new List<Product>();
        private List<string> _comboBoxSort = new List<string>();
        private List<string> _comboBoxFilter = new List<string>();
        private List<Button> _buttonList = new List<Button>();
        private List<Product> _product = new List<Product>();
        private int _selectedIndex;
        private int _currentPage = 0;
        private int _itemsCountPage = 20;
        private int _maxPage = 1;

        public List<Product> Products
        {
            get => _product;
            set => Set(ref _product, value, nameof(Products));
        }
        public List<string> ComboBoxSort 
        {
            get => _comboBoxSort;
            set => Set(ref _comboBoxSort, value, nameof(ComboBoxSort));
        }
        public List<string> ComboBoxFilter
        {
            get => _comboBoxFilter;
            set => Set(ref _comboBoxFilter, value, nameof(ComboBoxFilter));
        }
        public List<Button> ButtonsList
        {
            get => _buttonList;
            set => Set(ref _buttonList, value, nameof(ButtonsList));
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value, nameof(SelectedIndex));
        }

        public MainWindowViewModel()
        {
            GetProducts(null);
            InitializePages();
            InitializeComboBoxes();
        }

        // main methods
        private void InitializeComboBoxes()
        {
            ComboBoxSort.Add("Наименование >");
            ComboBoxSort.Add("Наименование <");
            ComboBoxSort.Add("Номер цеха >");
            ComboBoxSort.Add("Номер цеха <");
            ComboBoxSort.Add("Мин стоимость >");
            ComboBoxSort.Add("Мин стоимость <");

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach (var pt in context.ProductTypes.ToList())
                    ComboBoxFilter.Add(pt.Title);
            }
        }

        private void GetProducts(string? text)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if(text == null)
                {
                    _allProducts = context.Products
                        .Include(pm => pm.ProductMaterials)
                        .ThenInclude(m => m.Material)
                        .Include(p => p.ProductType)
                        .ToList();
                }
                else
                {
                    _allProducts = context.Products
                        .Include(pm => pm.ProductMaterials)
                        .ThenInclude(m => m.Material)
                        .Include(p => p.ProductType)
                        .Where(t => t.Title.ToLower().Contains(text.ToLower()))
                        .ToList();
                }
            }
        }

        // page methods
        private void InitializePages()
        {
            _currentPage = 0;
            _maxPage = (_allProducts.Count / _itemsCountPage) + ((_allProducts.Count % _itemsCountPage) > 0 ? 1 : 0);
            SetPage();
            SetButtons();
        }

        private void SetPage()
        {
            Products = _allProducts
                .Skip(_currentPage * _itemsCountPage)
                .Take(_itemsCountPage)
                .ToList();
        }

        private void SetButtons()
        {
            List<Button> buttons = new List<Button>();

            Button leftPage = new Button();
            leftPage.Content = "<";
            leftPage.Margin = new System.Windows.Thickness(0, 0, 2, 0);
            leftPage.BorderBrush = new SolidColorBrush(Colors.White);
            leftPage.BorderThickness = new System.Windows.Thickness(0);
            leftPage.Background = new SolidColorBrush(Colors.White);
            leftPage.Click += LeftPage_Click;
            buttons.Add(leftPage);

            for (int i = 0; i < _maxPage; i++)
            {
                Button specifiedNumberPage = new Button();
                specifiedNumberPage.Content = $"{i + 1}";
                specifiedNumberPage.Margin = new System.Windows.Thickness(0, 0, 2, 0);
                specifiedNumberPage.BorderBrush = new SolidColorBrush(Colors.White);
                specifiedNumberPage.BorderThickness = new System.Windows.Thickness(0);
                specifiedNumberPage.Background = new SolidColorBrush(Colors.White);
                specifiedNumberPage.Click += SpecifiedNumberPage_Click;
                buttons.Add(specifiedNumberPage);
            }

            Button rightPage = new Button();
            rightPage.Content = ">";
            rightPage.Margin = new System.Windows.Thickness(0, 0, 2, 0);
            rightPage.BorderBrush = new SolidColorBrush(Colors.White);
            rightPage.BorderThickness = new System.Windows.Thickness(0);
            rightPage.Background = new SolidColorBrush(Colors.White);
            rightPage.Click += RightPage_Click;
            buttons.Add(rightPage);

            ButtonsList = buttons;
        }

        private void SpecifiedNumberPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var content = sender as Button;
            int index = Convert.ToInt32(content.Content);

            _currentPage = index - 1;
            SetPage();
        }

        private void RightPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var content = sender as Button;

            if (_currentPage >= _maxPage - 1)
                return;

            _currentPage++;
            SetPage();
        }

        private void LeftPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var content = sender as Button;

            if (_currentPage <= 0)
                return;

            _currentPage--;
            SetPage();
        }

        // called methods
        public void Search(string text)
        {
            if (text == "")
                GetProducts(null);
            else
                GetProducts(text);

            InitializePages();
        }

        public void Sort(int index)
        {
            switch(index)
            {
                case 0:
                    _allProducts = _allProducts.OrderBy(p => p.Title).ToList();
                    break;
                case 1:
                    _allProducts = _allProducts.OrderByDescending(p => p.Title).ToList();
                    break;
                case 2:
                    _allProducts = _allProducts.OrderBy(p => p.ProductionWorkshopNumber).ToList();
                    break;
                case 3:
                    _allProducts = _allProducts.OrderByDescending(p => p.ProductionWorkshopNumber).ToList();
                    break;
                case 4:
                    _allProducts = _allProducts.OrderBy(p => p.MinCostForAgent).ToList();
                    break;
                case 5:
                    _allProducts = _allProducts.OrderByDescending(p => p.MinCostForAgent).ToList();
                    break;
            }

            InitializePages();
        }

        public void Filter(List<string> Types, int Index)
        {
            GetProducts(null);
            _allProducts = _allProducts.Where(p => p.ProductType.Title == Types[Index]).ToList();
            InitializePages();
        }

        public void DeleteItem()
        {
            var product = Products.ElementAt(_selectedIndex);

            // Delete all ProductMaterials items with Product.Id
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var productMaterials = context.ProductMaterials
                    .Where(p => p.ProductId == product.Id)
                    .ToList();

                if (productMaterials.Count == 0)
                    return;

                foreach (var pm in productMaterials)
                    context.ProductMaterials.Remove(pm);

                context.SaveChanges();
            }

            // Delete product item
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }

            GetProducts(null);
            InitializePages();
        }

        public void AddItem()
        {
            ProductItemEdit window = new ProductItemEdit(null);
            window.ShowDialog();
            GetProducts(null);
            InitializePages();
        }

        public void EditItem()
        {
            var product = _product.ElementAt(_selectedIndex);

            ProductItemEdit window = new ProductItemEdit(product);
            window.ShowDialog();
            GetProducts(null);
            InitializePages();
        }
    }
}
