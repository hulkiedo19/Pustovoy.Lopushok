using Microsoft.EntityFrameworkCore;
using Pustovoy.Lopushok.Domain.Entities;
using Pustovoy.Lopushok.Infrastucture.Persistence;
using Pustovoy.Lopushok.Presentation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pustovoy.Lopushok.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<Product> _products;
        private List<string> _comboBoxSort;
        private List<string> _comboBoxFilter;
        private int _selectedIndex;

        public List<Product> Products
        {
            get => _products;
            set => Set(ref _products, value, nameof(Products));
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value, nameof(SelectedIndex));
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

        public ICommand AddItem => new AddItemCommand(this);
        public ICommand EditItem => new EditItemCommand(this);
        public ICommand DeleteItem => new DeleteItemCommand(this);

        public MainWindowViewModel()
        {
            _products = new List<Product>();
            GetProducts();
            InitializeComboBoxes();
        }

        public void Search(string text)
        {
            if (text == "")
                GetProducts();
            else
                GetProductsWithText(text);
        }

        public void Sort(int index)
        {
            switch(index)
            {
                case 0:
                    Products = Products.OrderBy(p => p.Title).ToList();
                    break;
                case 1:
                    Products = Products.OrderByDescending(p => p.Title).ToList();
                    break;
                case 2:
                    Products = Products.OrderBy(p => p.ProductionWorkshopNumber).ToList();
                    break;
                case 3:
                    Products = Products.OrderByDescending(p => p.ProductionWorkshopNumber).ToList();
                    break;
                case 4:
                    Products = Products.OrderBy(p => p.MinCostForAgent).ToList();
                    break;
                case 5:
                    Products = Products.OrderByDescending(p => p.MinCostForAgent).ToList();
                    break;
            }
        }

        public void Filter(List<string> Types, int Index)
        {
            GetProducts();
            Products = Products.Where(p => p.ProductType.Title == Types[Index]).ToList();
        }

        private void GetProducts()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Products = context.Products
                    .Include(pm => pm.ProductMaterials)
                    .ThenInclude(m => m.Material)
                    .Include(p => p.ProductType)
                    .ToList();
            }
        }

        private void GetProductsWithText(string text)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Products = context.Products
                    .Include(pm => pm.ProductMaterials)
                    .ThenInclude(m => m.Material)
                    .Include(p => p.ProductType)
                    .Where(t => t.Title.ToLower().Contains(text.ToLower()))
                    .ToList();
            }
        }

        private void InitializeComboBoxes()
        {
            ComboBoxSort = new List<string>()
            {
                "Наименование >",
                "Наименование <",
                "Номер цеха >",
                "Номер цеха <",
                "Мин стоимость >",
                "Мин стоимость <"
            };

            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                List<string> types = new List<string>();
                List<ProductType> productTypes = context.ProductTypes
                    .ToList();

                foreach (var pt in productTypes)
                    types.Add(pt.Title);

                ComboBoxFilter = types;
            }
        }
    }
}
